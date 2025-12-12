import { useState } from 'react'
import { PRODUCTS } from './products'
import type { CartItem, Product } from './types'
import { Header } from './components/Header'
import { ProductList } from './components/ProductList'
import { CartSidebar } from './components/CartSidebar'
import { ProductDetailsPanel } from './components/ProductDetailsPanel'

function App() {
  const [cartItems, setCartItems] = useState<CartItem[]>([])
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null)
  const [isCartOpen, setIsCartOpen] = useState(false)

  const totalItems = cartItems.reduce((sum, item) => sum + item.quantity, 0)
  const totalPrice = cartItems.reduce((sum, item) => sum + item.product.price * item.quantity, 0)

  const handleAddToCart = (productId: string) => {
    const product = PRODUCTS.find((p) => p.id === productId)
    if (!product) return

    setCartItems((prev) => {
      const existing = prev.find((item) => item.product.id === productId)
      if (!existing) {
        return [...prev, { product, quantity: 1 }]
      }
      return prev.map((item) =>
        item.product.id === productId
          ? { ...item, quantity: item.quantity + 1 }
          : item,
      )
    })
  }

  const handleRemoveFromCart = (productId: string) => {
    setCartItems((prev) => {
      return prev
        .map((item) =>
          item.product.id === productId
            ? { ...item, quantity: item.quantity - 1 }
            : item,
        )
        .filter((item) => item.quantity > 0)
    })
  }

  const handleToggleCart = () => setIsCartOpen((open) => !open)

  const handleViewDetails = (product: Product) => {
    setSelectedProduct(product)
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

  const handleCloseDetails = () => setSelectedProduct(null)

  return (
    <div className="min-h-screen bg-gray-100">
      <Header totalItems={totalItems} onToggleCart={handleToggleCart} />

      <main className="pt-4 pb-20">
        <ProductList
          products={PRODUCTS}
          onViewDetails={handleViewDetails}
          onAddToCart={handleAddToCart}
        />
        <ProductDetailsPanel
          product={selectedProduct}
          onClose={handleCloseDetails}
          onAddToCart={handleAddToCart}
        />
      </main>

      <CartSidebar
        isOpen={isCartOpen}
        items={cartItems}
        totalPrice={totalPrice}
        onClose={handleToggleCart}
        onRemoveItem={handleRemoveFromCart}
      />
    </div>
  )
}

export default App
