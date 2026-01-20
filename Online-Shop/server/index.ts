import express from "express";
import cors from "cors";
import { connectDB } from "./db.js";
import { Product, Order } from "./models.js";
import { seedProducts } from "./seed.js";

const app = express();
const PORT = process.env.PORT ? Number(process.env.PORT) : 5000;

app.use(cors());
app.use(express.json());

// Health check endpoint
app.get("/api/health", (_req, res) => {
  res.json({ status: "ok" });
});

// Get all products
app.get("/api/products", async (_req, res) => {
  try {
    const products = await Product.find();
    // Map to frontend format (id instead of productId)
    const mappedProducts = products.map((p) => ({
      id: p.productId,
      name: p.name,
      description: p.description,
      imageUrl: p.imageUrl,
      price: p.price,
    }));
    res.json(mappedProducts);
  } catch (error) {
    console.error("Error fetching products:", error);
    res.status(500).json({ error: "Failed to fetch products" });
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

// Create a new order
app.post("/api/orders", async (req, res) => {
  try {
    const { items, customer } = req.body;

    if (!items || !Array.isArray(items) || items.length === 0) {
      res.status(400).json({ error: "Order must contain at least one item" });
      return;
    }

    if (!customer || !customer.name || !customer.email || !customer.address) {
      res.status(400).json({ error: "Customer information is required" });
      return;
    }

    const totalPrice = items.reduce(
      (sum: number, item: { price: number; quantity: number }) =>
        sum + item.price * item.quantity,
      0
    );

    const order = new Order({
      items,
      customer,
      totalPrice,
    });

    await order.save();
    res.status(201).json({ message: "Order created successfully", orderId: order._id });
  } catch (error) {
    console.error("Error creating order:", error);
    res.status(500).json({ error: "Failed to create order" });
  }
});

// Get all orders (for admin purposes)
app.get("/api/orders", async (_req, res) => {
  try {
    const orders = await Order.find().sort({ createdAt: -1 });
    res.json(orders);
  } catch (error) {
    console.error("Error fetching orders:", error);
    res.status(500).json({ error: "Failed to fetch orders" });
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
