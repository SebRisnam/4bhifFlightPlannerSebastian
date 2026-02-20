import mongoose, { Schema, Document, Types } from 'mongoose';

// =====================
// Product
// =====================
export interface IProduct extends Document {
    productId: string;
    name: string;
    description: string;
    imageUrl: string;
    price: number;
    quantity: number; // stock quantity
}

const ProductSchema = new Schema<IProduct>({
    productId: { type: String, required: true, unique: true },
    name: { type: String, required: true },
    description: { type: String, required: true },
    imageUrl: { type: String, required: true },
    price: { type: Number, required: true },
    quantity: { type: Number, required: true, default: 100 },
});

export const Product = mongoose.model<IProduct>('Product', ProductSchema);

// =====================
// Customer (own collection, referenced by Order)
// =====================
export interface ICustomer extends Document {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    address: string;
    city: string;
    postalCode: string;
    country: string;
}

const CustomerSchema = new Schema<ICustomer>({
    firstName: { type: String, required: true },
    lastName: { type: String, required: true },
    email: { type: String, required: true },
    phone: { type: String, default: '' },
    address: { type: String, required: true },
    city: { type: String, required: true },
    postalCode: { type: String, required: true },
    country: { type: String, required: true, default: 'Austria' },
});

export const Customer = mongoose.model<ICustomer>('Customer', CustomerSchema);

// =====================
// LineItem (embedded in Order)
// =====================
export interface ILineItem {
    product: Types.ObjectId; // reference to Product
    unitPrice: number;
    quantity: number;
}

const LineItemSchema = new Schema<ILineItem>({
    product: { type: Schema.Types.ObjectId, ref: 'Product', required: true },
    unitPrice: { type: Number, required: true },
    quantity: { type: Number, required: true, min: 1 },
});

// =====================
// Order
// =====================
export interface IOrder extends Document {
    customer: Types.ObjectId; // reference to Customer
    lineItems: ILineItem[];
    deliveryAddress: string;
    totalPrice: number;
    createdAt: Date;
}

const OrderSchema = new Schema<IOrder>({
    customer: { type: Schema.Types.ObjectId, ref: 'Customer', required: true },
    lineItems: { type: [LineItemSchema], required: true },
    deliveryAddress: { type: String, required: true },
    totalPrice: { type: Number, required: true },
    createdAt: { type: Date, default: Date.now },
});

export const Order = mongoose.model<IOrder>('Order', OrderSchema);
