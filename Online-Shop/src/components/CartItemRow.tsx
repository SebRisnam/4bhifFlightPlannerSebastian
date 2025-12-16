import type { FC } from 'react'
import type { CartItem } from '../types'

interface CartItemRowProps {
  item: CartItem
  onRemove: (productId: string) => void
}

export const CartItemRow: FC<CartItemRowProps> = ({ item, onRemove }) => {
  return (
    <li className="flex items-center gap-3 text-sm">
      <img
        src={item.product.imageUrl}
        alt={item.product.name}
        className="w-12 h-12 object-contain rounded border border-gray-200 bg-white"
      />
      <div className="flex-1">
        <div className="font-medium">{item.product.name}</div>
        <div className="text-gray-600 text-xs">
          {item.quantity} x € {item.product.price.toFixed(2)}
        </div>
      </div>
      <button
        className="text-red-600 text-xs hover:underline"
        onClick={() => onRemove(item.product.id)}
      >
        Remove
      </button>
    </li>
  )
}
