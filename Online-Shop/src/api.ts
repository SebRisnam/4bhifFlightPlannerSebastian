import type {Product} from './types'

const API_BASE_URL = 'http://localhost:5000/api';

export async function fetchProducts(): Promise<Product[]> {
    const response = await fetch(`${API_BASE_URL}/products`);
    if (!response.ok) {
        throw new Error('Failed to fetch products');
    }
    return response.json();
}

export async function fetchProduct(productId: string): Promise<Product> {
    const response = await fetch(`${API_BASE_URL}/products/${productId}`);
    if (!response.ok) {
        throw new Error('Failed to fetch product');
    }
    return response.json();
}

export interface OrderItem {
    productId: string;
    quantity: number;
}

export interface CustomerData {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    address: string;
    city: string;
    postalCode: string;
    country: string;
}

export interface CreateOrderResponse {
    message: string;
    orderId: string;
}

export async function createOrder(
    items: OrderItem[],
    customer: CustomerData,
    deliveryAddress?: string
): Promise<CreateOrderResponse> {
    const response = await fetch(`${API_BASE_URL}/orders`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ items, customer, deliveryAddress }),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.error || 'Failed to create order');
    }

    return response.json();
}

export async function fetchOrders(): Promise<any[]> {
    const response = await fetch(`${API_BASE_URL}/orders`);
    if (!response.ok) {
        throw new Error('Failed to fetch orders');
    }
    return response.json();
}

export async function fetchCustomerOrders(email: string): Promise<any[]> {
    const response = await fetch(`${API_BASE_URL}/orders/customer/${encodeURIComponent(email)}`);
    if (!response.ok) {
        throw new Error('Failed to fetch customer orders');
    }
    return response.json();
}

