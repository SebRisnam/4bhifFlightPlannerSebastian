import type {FC} from 'react'

interface HeaderProps {
    totalItems: number
    onToggleCart: () => void
}

export const Header: FC<HeaderProps> = ({totalItems, onToggleCart}) => {
    return (
        <header className="bg-yellow-400 border-b-4 border-red-600">
            <div className="px-8 py-4 flex items-center justify-between">
                <div className="flex items-center gap-3">
                    <img
                        className="px-4 py-2 h-25"
                        src="https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/LEGO_logo.svg/330px-LEGO_logo.svg.png"
                        alt="LEGO logo"
                    />
                </div>

                <nav className="flex items-center gap-6 text-sm font-medium">
                    <button
                        className="text-blue-700 hover:underline"
                        onClick={() => {
                            const el = document.getElementById('products')
                            el?.scrollIntoView({behavior: 'smooth'})
                        }}
                    >
                        Produkte
                    </button>
                    <button
                        className="relative flex items-center gap-1 text-gray-900 hover:text-gray-700"
                        onClick={onToggleCart}
                    >
                        <span className="material-icons"></span>
                        <span>Cart</span>
                        <span
                            className="ml-1 inline-flex items-center justify-center rounded-full bg-black text-white text-xs w-6 h-6">
              {totalItems}
            </span>
                    </button>
                </nav>
            </div>
        </header>
    )
}
