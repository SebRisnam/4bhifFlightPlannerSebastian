import type { FC } from 'react'
import type { Product } from '../types'
import { ProductCard } from './ProductCard'

interface ProductListProps {
  products: Product[]
  onViewDetails: (product: Product) => void
  onAddToCart: (productId: string) => void
}

export const ProductList: FC<ProductListProps> = ({ products, onViewDetails, onAddToCart }) => {
  return (
    <section id="products" className="max-w-5xl mx-auto px-4 py-10">
      <h2 className="text-3xl font-semibold text-center mb-8 text-black">Product Overview</h2>
      <div className="grid gap-6 md:grid-cols-3">
        {products.map((p) => (
          <ProductCard
            key={p.id}
            product={p}
            onViewDetails={onViewDetails}
            onAddToCart={onAddToCart}
          />
        ))}
      </div>
    </section>
  )
}

