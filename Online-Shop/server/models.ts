import mongoose, { Schema, Document } from 'mongoose';

// Product Interface & Schema
export interface IProduct extends Document {
    productId: string;
    name: string;
    description: string;
    imageUrl: string;
    price: number;
}

const ProductSchema = new Schema<IProduct>({
    productId: { type: String, required: true, unique: true },
    name: { type: String, required: true },
    description: { type: String, required: true },
    imageUrl: { type: String, required: true },
    price: { type: Number, required: true },
});

export const Product = mongoose.model<IProduct>('Product', ProductSchema);

// Order Item Interface
export interface IOrderItem {
    productId: string;
    name: string;
    price: number;
    quantity: number;
}

// Customer Interface
export interface ICustomer {
    name: string;
    email: string;
    address: string;
    city: string;
    postalCode: string;
}

// Order Interface & Schema
export interface IOrder extends Document {
    items: IOrderItem[];
    customer: ICustomer;
    totalPrice: number;
    createdAt: Date;
}

const OrderItemSchema = new Schema<IOrderItem>({
    productId: { type: String, required: true },
    name: { type: String, required: true },
    price: { type: Number, required: true },
    quantity: { type: Number, required: true },
});

const CustomerSchema = new Schema<ICustomer>({
    name: { type: String, required: true },
    email: { type: String, required: true },
    address: { type: String, required: true },
    city: { type: String, required: true },
    postalCode: { type: String, required: true },
});

const OrderSchema = new Schema<IOrder>({
    items: { type: [OrderItemSchema], required: true },
    customer: { type: CustomerSchema, required: true },
    totalPrice: { type: Number, required: true },
    createdAt: { type: Date, default: Date.now },
});

export const Order = mongoose.model<IOrder>('Order', OrderSchema);
