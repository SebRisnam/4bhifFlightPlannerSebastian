import type { FC } from 'react'
import type { CartItem } from '../types'

interface CartSidebarProps {
  isOpen: boolean
  items: CartItem[]
  totalPrice: number
  onClose: () => void
  onRemoveItem: (productId: string) => void
}

export const CartSidebar: FC<CartSidebarProps> = ({ isOpen, items, totalPrice, onClose, onRemoveItem }) => {
  return (
    <div
      className={`fixed inset-y-0 right-0 w-80 bg-white shadow-lg transform transition-transform duration-200 flex flex-col ${
        isOpen ? 'translate-x-0' : 'translate-x-full'
      }`}
    >
      <div className="flex items-center justify-between px-4 py-3 border-b">
        <h2 className="text-lg font-semibold">Your Cart</h2>
        <button className="text-sm text-gray-500 hover:text-gray-700" onClick={onClose}>
          Close
        </button>
      </div>
      <div className="flex-1 overflow-y-auto px-4 py-3">
        {items.length === 0 ? (
          <p className="text-sm text-gray-600">Your cart is empty.</p>
        ) : (
          <ul className="space-y-3">
            {items.map((item) => (
              <li key={item.product.id} className="flex justify-between items-center text-sm">
                <div>
                  <div className="font-medium">{item.product.name}</div>
                  <div className="text-gray-600">
                    {item.quantity} x € {item.product.price.toFixed(2)}
                  </div>
                </div>
                <button
                  className="text-red-600 text-xs hover:underline"
                  onClick={() => onRemoveItem(item.product.id)}
                >
                  Remove
                </button>
              </li>
            ))}
          </ul>
        )}
      </div>
      <div className="px-4 py-3 border-t">
        <div className="flex justify-between font-semibold mb-2">
          <span>Total:</span>
          <span>€ {totalPrice.toFixed(2)}</span>
        </div>
        <button
          className="w-full bg-yellow-400 hover:bg-yellow-500 text-gray-900 font-medium py-2 rounded disabled:opacity-60"
          disabled={items.length === 0}
        >
          Checkout
        </button>
      </div>
    </div>
  )
}

