import { Product } from './models.js';

// Initial product data for seeding
export const SEED_PRODUCTS = [
    {
        productId: 'city-police-station',
        name: 'City Police Station',
        description: 'Keep the city safe with this feature-packed police station playset.',
        imageUrl: 'https://www.lego.com/cdn/cs/set/assets/blt03b83a89e42d9e0a/60246.jpg',
        price: 89.99,
        quantity: 50,
    },
    {
        productId: 'city-fire-truck',
        name: 'City Fire Truck',
        description: 'Race to the rescue with a fully-equipped LEGO City fire truck.',
        imageUrl: 'https://www.lego.com/cdn/cs/set/assets/bltb24e963aed6f6a78/60374.png',
        price: 39.99,
        quantity: 75,
    },
    {
        productId: 'city-passenger-train',
        name: 'City Passenger Train',
        description: 'High-speed passenger train for fast city travel fun.',
        imageUrl: 'https://www.lego.com/cdn/cs/set/assets/blt1d4fad9359e63c1d/60337.png',
        price: 129.99,
        quantity: 30,
    },
];

export async function seedProducts(): Promise<void> {
    try {
        const count = await Product.countDocuments();
        if (count === 0) {
            await Product.insertMany(SEED_PRODUCTS);
            console.log('Database seeded with initial products');
        } else {
            console.log('Database already contains products, skipping seed');
        }
    } catch (error) {
        console.error('Error seeding database:', error);
    }
}
