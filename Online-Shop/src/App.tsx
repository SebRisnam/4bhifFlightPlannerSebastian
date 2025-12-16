import { useEffect, useState } from 'react'
import type { CartItem, Product } from './types'
import { fetchProducts } from './api'
import { Header } from './components/Header'
import { ProductList } from './components/ProductList'
import { CartSidebar } from './components/CartSidebar'
import { ProductDetailsPanel } from './components/ProductDetailsPanel'

function App() {
  const [products, setProducts] = useState<Product[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [cartItems, setCartItems] = useState<CartItem[]>([])
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null)
  const [isCartOpen, setIsCartOpen] = useState(false)

  useEffect(() => {
    let isMounted = true

    const loadProducts = async () => {
      try {
        setIsLoading(true)
        setError(null)
        const data = await fetchProducts()
        if (isMounted) {
          setProducts(data)
        }
      } catch (err) {
        if (isMounted) {
          setError(err instanceof Error ? err.message : 'Unknown error')
        }
      } finally {
        if (isMounted) {
          setIsLoading(false)
        }
      }
    }

    loadProducts()

    return () => {
      isMounted = false
    }
  }, [])

  const totalItems = cartItems.reduce((sum, item) => sum + item.quantity, 0)
  const totalPrice = cartItems.reduce((sum, item) => sum + item.product.price * item.quantity, 0)

  const handleAddToCart = (productId: string) => {
    const product = products.find((p) => p.id === productId)
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
    <div className="min-h-screen min-w-screen bg-gray-100">
      <Header totalItems={totalItems} onToggleCart={handleToggleCart} />

      <main className="pt-4 pb-20">
        {isLoading && (
          <div className="px-8 py-10 text-center text-gray-700">Loading products...</div>
        )}

        {error && !isLoading && (
          <div className="px-8 py-10 text-center text-red-600">{error}</div>
        )}

        {!isLoading && !error && (
          <ProductList
            products={products}
            onViewDetails={handleViewDetails}
            onAddToCart={handleAddToCart}
          />
        )}
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
