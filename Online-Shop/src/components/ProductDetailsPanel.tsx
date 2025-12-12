import type { FC } from 'react'
import type { Product } from '../types'

interface ProductDetailsPanelProps {
  product: Product | null
  onClose: () => void
  onAddToCart: (productId: string) => void
}

export const ProductDetailsPanel: FC<ProductDetailsPanelProps> = ({ product, onClose, onAddToCart }) => {
  if (!product) return null

  return (
    <section className="max-w-5xl mx-auto px-4 pb-10">
      <div className="mt-6 bg-white rounded-lg shadow-md p-6 flex flex-col md:flex-row gap-6">
        <img
          src={product.imageUrl}
          alt={product.name}
          className="w-full md:w-1/3 h-48 object-contain"
        />
        <div className="flex-1">
          <div className="flex justify-between items-start mb-2">
            <h2 className="text-2xl font-semibold">{product.name}</h2>
            <button
              className="text-sm text-gray-500 hover:text-gray-700"
              onClick={onClose}
            >
              ✕ Close
            </button>
          </div>
          <p className="text-gray-700 mb-4">{product.description}</p>
          <p className="text-xl font-bold mb-4">€ {product.price.toFixed(2)}</p>
          <button
            className="bg-yellow-400 hover:bg-yellow-500 text-gray-900 font-medium py-2 px-4 rounded"
            onClick={() => onAddToCart(product.id)}
          >
            Add to cart
          </button>
        </div>
      </div>
    </section>
  )
}

