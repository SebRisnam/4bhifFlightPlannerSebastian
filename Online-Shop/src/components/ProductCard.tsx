import type {FC} from 'react'
import type {Product} from '../types'

interface ProductCardProps {
    product: Product
    onViewDetails: (product: Product) => void
    onAddToCart: (productId: string) => void
}

export const ProductCard: FC<ProductCardProps> = ({product, onViewDetails, onAddToCart}) => {
    return (
        <div className="bg-white rounded-lg shadow-md p-4 flex flex-col items-center text-center">
            <img
                src={product.imageUrl}
                alt={product.name}
                className="w-full h-40 object-contain mb-4"
            />
            <h3 className="text-lg font-semibold mb-2">{product.name}</h3>
            <p className="text-gray-700 font-bold mb-4">€ {product.price.toFixed(2)}</p>
            <div className="flex flex-col gap-2 w-full">
                <button
                    className="bg-yellow-400 hover:bg-yellow-500 text-gray-900 font-medium py-2 rounded"
                    onClick={() => onViewDetails(product)}
                >
                    View Details
                </button>
                <button
                    className="bg-yellow-400 hover:bg-yellow-500 text-gray-900 font-medium py-2 rounded"
                    onClick={() => onAddToCart(product.id)}
                >
                    Add to cart
                </button>
            </div>
        </div>
    )
}

