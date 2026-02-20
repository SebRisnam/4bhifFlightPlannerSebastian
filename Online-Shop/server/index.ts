import express from "express";
import cors from "cors";
import { connectDB } from "./db.js";
import { Product, Order, Customer } from "./models.js";
import { seedProducts } from "./seed.js";

const app = express();
const PORT = process.env.PORT ? Number(process.env.PORT) : 5000;

app.use(cors());
app.use(express.json());

// Health check endpoint
app.get("/api/health", (_req, res) => {
  res.json({ status: "ok" });
});

// =====================
// Product endpoints
// =====================

// Get all products
app.get("/api/products", async (_req, res) => {
  try {
    const products = await Product.find();
    const mappedProducts = products.map((p) => ({
      id: p.productId,
      name: p.name,
      description: p.description,
      imageUrl: p.imageUrl,
      price: p.price,
      quantity: p.quantity,
    }));
    res.json(mappedProducts);
  } catch (error) {
    console.error("Error fetching products:", error);
    res.status(500).json({ error: "Failed to fetch products" });
  }
});

// Get single product by productId
app.get("/api/products/:productId", async (req, res) => {
  try {
    const product = await Product.findOne({ productId: req.params.productId });
    if (!product) {
      res.status(404).json({ error: "Product not found" });
      return;
    }
    res.json({
      id: product.productId,
      name: product.name,
      description: product.description,
      imageUrl: product.imageUrl,
      price: product.price,
      quantity: product.quantity,
    });
  } catch (error) {
    console.error("Error fetching product:", error);
    res.status(500).json({ error: "Failed to fetch product" });
  }
});

// Seed products endpoint (manual trigger)
app.post("/api/products/seed", async (_req, res) => {
  try {
    await Product.deleteMany({});
    await seedProducts();
    res.json({ message: "Products seeded successfully" });
  } catch (error) {
    console.error("Error seeding products:", error);
    res.status(500).json({ error: "Failed to seed products" });
  }
});

// =====================
// Customer endpoints
// =====================

// Create a new customer (or find existing by email)
app.post("/api/customers", async (req, res) => {
  try {
    const { firstName, lastName, email, phone, address, city, postalCode, country } = req.body;

    if (!firstName || !lastName || !email) {
      res.status(400).json({ error: "firstName, lastName and email are required" });
      return;
    }

    // Upsert: find by email and update, or create new
    const customer = await Customer.findOneAndUpdate(
      { email },
      { firstName, lastName, email, phone: phone || '', address, city, postalCode, country: country || 'Austria' },
      { new: true, upsert: true, runValidators: true }
    );

    res.status(201).json(customer);
  } catch (error) {
    console.error("Error creating customer:", error);
    res.status(500).json({ error: "Failed to create customer" });
  }
});

// Get all customers
app.get("/api/customers", async (_req, res) => {
  try {
    const customers = await Customer.find().sort({ lastName: 1 });
    res.json(customers);
  } catch (error) {
    console.error("Error fetching customers:", error);
    res.status(500).json({ error: "Failed to fetch customers" });
  }
});

// Get customer by ID
app.get("/api/customers/:id", async (req, res) => {
  try {
    const customer = await Customer.findById(req.params.id);
    if (!customer) {
      res.status(404).json({ error: "Customer not found" });
      return;
    }
    res.json(customer);
  } catch (error) {
    console.error("Error fetching customer:", error);
    res.status(500).json({ error: "Failed to fetch customer" });
  }
});

// =====================
// Order endpoints
// =====================

// Create a new order
app.post("/api/orders", async (req, res) => {
  try {
    const { items, customer: customerData, deliveryAddress } = req.body;

    // Validate items
    if (!items || !Array.isArray(items) || items.length === 0) {
      res.status(400).json({ error: "Order must contain at least one item" });
      return;
    }

    // Validate customer data
    if (!customerData || !customerData.firstName || !customerData.lastName || !customerData.email || !customerData.address) {
      res.status(400).json({ error: "Customer information (firstName, lastName, email, address) is required" });
      return;
    }

    // Create or update customer
    const customer = await Customer.findOneAndUpdate(
      { email: customerData.email },
      {
        firstName: customerData.firstName,
        lastName: customerData.lastName,
        email: customerData.email,
        phone: customerData.phone || '',
        address: customerData.address,
        city: customerData.city,
        postalCode: customerData.postalCode,
        country: customerData.country || 'Austria',
      },
      { new: true, upsert: true, runValidators: true }
    );

    // Resolve product references and build line items
    const lineItems = [];
    let totalPrice = 0;

    for (const item of items) {
      const product = await Product.findOne({ productId: item.productId });
      if (!product) {
        res.status(400).json({ error: `Product not found: ${item.productId}` });
        return;
      }

      if (product.quantity < item.quantity) {
        res.status(400).json({ error: `Insufficient stock for ${product.name}. Available: ${product.quantity}` });
        return;
      }

      lineItems.push({
        product: product._id,
        unitPrice: product.price,
        quantity: item.quantity,
      });

      totalPrice += product.price * item.quantity;

      // Decrease product stock
      product.quantity -= item.quantity;
      await product.save();
    }

    // Build delivery address string
    const finalDeliveryAddress = deliveryAddress ||
      `${customerData.address}, ${customerData.postalCode} ${customerData.city}, ${customerData.country || 'Austria'}`;

    const order = new Order({
      customer: customer._id,
      lineItems,
      deliveryAddress: finalDeliveryAddress,
      totalPrice,
    });

    await order.save();
    res.status(201).json({ message: "Order created successfully", orderId: order._id });
  } catch (error) {
    console.error("Error creating order:", error);
    res.status(500).json({ error: "Failed to create order" });
  }
});

// Get all orders (populated with customer and product data)
app.get("/api/orders", async (_req, res) => {
  try {
    const orders = await Order.find()
      .populate('customer')
      .populate('lineItems.product')
      .sort({ createdAt: -1 });
    res.json(orders);
  } catch (error) {
    console.error("Error fetching orders:", error);
    res.status(500).json({ error: "Failed to fetch orders" });
  }
});

// Get single order by ID
app.get("/api/orders/:id", async (req, res) => {
  try {
    const order = await Order.findById(req.params.id)
      .populate('customer')
      .populate('lineItems.product');
    if (!order) {
      res.status(404).json({ error: "Order not found" });
      return;
    }
    res.json(order);
  } catch (error) {
    console.error("Error fetching order:", error);
    res.status(500).json({ error: "Failed to fetch order" });
  }
});

// Get orders by customer email
app.get("/api/orders/customer/:email", async (req, res) => {
  try {
    const customer = await Customer.findOne({ email: req.params.email });
    if (!customer) {
      res.json([]);
      return;
    }
    const orders = await Order.find({ customer: customer._id })
      .populate('customer')
      .populate('lineItems.product')
      .sort({ createdAt: -1 });
    res.json(orders);
  } catch (error) {
    console.error("Error fetching customer orders:", error);
    res.status(500).json({ error: "Failed to fetch customer orders" });
  }
});

// Connect to database and start server
connectDB().then(() => {
  // Seed products on startup if database is empty
  seedProducts();

  app.listen(PORT, () => {
    console.log(`Express server listening on http://localhost:${PORT}`);
  });
});
