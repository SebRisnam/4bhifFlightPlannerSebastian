import {useState, type FC} from 'react'
import type {CartItem} from '../types'
import {CartItemRow} from './CartItemRow'
import {createOrder, type CustomerData, type OrderItem} from '../api'

interface CartSidebarProps {
    isOpen: boolean
    items: CartItem[]
    totalPrice: number
    onClose: () => void
    onRemoveItem: (productId: string) => void
    onClearCart: () => void
}

export const CartSidebar: FC<CartSidebarProps> = ({isOpen, items, totalPrice, onClose, onRemoveItem, onClearCart}) => {
    const [showCheckout, setShowCheckout] = useState(false)
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [orderSuccess, setOrderSuccess] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [formData, setFormData] = useState<CustomerData>({
        firstName: '',
        lastName: '',
        email: '',
        phone: '',
        address: '',
        city: '',
        postalCode: '',
        country: 'Austria',
    })

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = e.target
        setFormData(prev => ({...prev, [name]: value}))
    }

    const handleSubmitOrder = async (e: React.FormEvent) => {
        e.preventDefault()
        setError(null)
        setIsSubmitting(true)

        try {
            const orderItems: OrderItem[] = items.map(item => ({
                productId: item.product.id,
                quantity: item.quantity,
            }))

            await createOrder(orderItems, formData)
            setOrderSuccess(true)
            onClearCart()
            setFormData({firstName: '', lastName: '', email: '', phone: '', address: '', city: '', postalCode: '', country: 'Austria'})

            // Reset after 3 seconds
            setTimeout(() => {
                setOrderSuccess(false)
                setShowCheckout(false)
            }, 3000)
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Order failed')
        } finally {
            setIsSubmitting(false)
        }
    }

    const handleBackToCart = () => {
        setShowCheckout(false)
        setError(null)
    }

    return (
        <div
            className={`fixed inset-y-0 right-0 w-96 bg-white shadow-lg transform transition-transform duration-200 flex flex-col ${
                isOpen ? 'translate-x-0' : 'translate-x-full'
            }`}
        >
            <div className="flex items-center justify-between px-4 py-3 border-b">
                <h2 className="text-lg font-semibold">
                    {showCheckout ? 'Checkout' : 'Your Cart'}
                </h2>
                <button className="text-sm text-gray-500 hover:text-gray-700" onClick={onClose}>
                    Close
                </button>
            </div>

            {orderSuccess ? (
                <div className="flex-1 flex items-center justify-center px-4">
                    <div className="text-center">
                        <div className="text-green-600 text-4xl mb-4">✓</div>
                        <h3 className="text-xl font-semibold text-green-600">Order Placed!</h3>
                        <p className="text-gray-600 mt-2">Thank you for your purchase.</p>
                    </div>
                </div>
            ) : showCheckout ? (
                <form onSubmit={handleSubmitOrder} className="flex-1 flex flex-col">
                    <div className="flex-1 overflow-y-auto px-4 py-3 space-y-4">
                        <button
                            type="button"
                            onClick={handleBackToCart}
                            className="text-sm text-blue-600 hover:underline"
                        >
                            ← Back to Cart
                        </button>

                        {error && (
                            <div className="bg-red-100 text-red-700 p-3 rounded text-sm">
                                {error}
                            </div>
                        )}

                        <div className="grid grid-cols-2 gap-3">
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    First Name *
                                </label>
                                <input
                                    type="text"
                                    name="firstName"
                                    value={formData.firstName}
                                    onChange={handleInputChange}
                                    required
                                    className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Last Name *
                                </label>
                                <input
                                    type="text"
                                    name="lastName"
                                    value={formData.lastName}
                                    onChange={handleInputChange}
                                    required
                                    className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
                                />
                            </div>
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Email *
                            </label>
                            <input
                                type="email"
                                name="email"
                                value={formData.email}
                                onChange={handleInputChange}
                                required
                                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
                            />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Phone
                            </label>
                            <input
                                type="tel"
                                name="phone"
                                value={formData.phone}
                                onChange={handleInputChange}
                                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
                            />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Street Address *
                            </label>
                            <input
                                type="text"
                                name="address"
                                value={formData.address}
                                onChange={handleInputChange}
                                required
                                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
                            />
                        </div>

                        <div className="grid grid-cols-2 gap-3">
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    City *
                                </label>
                                <input
                                    type="text"
                                    name="city"
                                    value={formData.city}
                                    onChange={handleInputChange}
                                    required
                                    className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Postal Code *
                                </label>
                                <input
                                    type="text"
                                    name="postalCode"
                                    value={formData.postalCode}
                                    onChange={handleInputChange}
                                    required
                                    className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
                                />
                            </div>
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Country *
                            </label>
                            <input
                                type="text"
                                name="country"
                                value={formData.country}
                                onChange={handleInputChange}
                                required
                                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
                            />
                        </div>

                        <div className="border-t pt-4 mt-4">
                            <h3 className="font-semibold mb-2">Order Summary</h3>
                            <ul className="text-sm space-y-1">
                                {items.map(item => (
                                    <li key={item.product.id} className="flex justify-between">
                                        <span>{item.quantity}x {item.product.name}</span>
                                        <span>€ {(item.product.price * item.quantity).toFixed(2)}</span>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    </div>

                    <div className="px-4 py-3 border-t">
                        <div className="flex justify-between font-semibold mb-2">
                            <span>Total:</span>
                            <span>€ {totalPrice.toFixed(2)}</span>
                        </div>
                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className="w-full bg-yellow-400 hover:bg-yellow-500 text-gray-900 font-medium py-2 rounded disabled:opacity-60"
                        >
                            {isSubmitting ? 'Processing...' : 'Place Order'}
                        </button>
                    </div>
                </form>
            ) : (
                <>
                    <div className="flex-1 overflow-y-auto px-4 py-3">
                        {items.length === 0 ? (
                            <p className="text-sm text-gray-600">Your cart is empty.</p>
                        ) : (
                            <ul className="space-y-3">
                                {items.map((item) => (
                                    <CartItemRow key={item.product.id} item={item} onRemove={onRemoveItem}/>
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
                            onClick={() => setShowCheckout(true)}
                        >
                            Checkout
                        </button>
                    </div>
                </>
            )}
        </div>
    )
}
