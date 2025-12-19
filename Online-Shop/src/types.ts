export interface CartItem {
    quantity: number
    product: Product
}

export interface Product {

    price: number
    imageUrl: string
    description: string
    name: string
    id: string
}

