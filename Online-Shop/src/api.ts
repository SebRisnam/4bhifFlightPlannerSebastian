import type {Product} from './types'

const API_BASE_URL = 'http://localhost:5000/api';

export async function fetchProducts(): Promise<Product[]> {
    const response = await fetch(`${API_BASE_URL}/products`);
    if (!response.ok) {
        throw new Error('Failed to fetch products');
    }
    return response.json();
}

export interface OrderItem {
    productId: string;
    name: string;
    price: number;
    quantity: number;
}

export interface CustomerData {
    name: string;
    email: string;
    address: string;
    city: string;
    postalCode: string;
}

export interface CreateOrderResponse {
    message: string;
    orderId: string;
}

export async function createOrder(
    items: OrderItem[],
    customer: CustomerData
): Promise<CreateOrderResponse> {
    const response = await fetch(`${API_BASE_URL}/orders`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ items, customer }),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.error || 'Failed to create order');
    }

    return response.json();
}

