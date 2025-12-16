import type { Product } from './types'
import { PRODUCTS } from './products'

export async function fetchProducts(): Promise<Product[]> {
  await new Promise((resolve) => setTimeout(resolve, 800))
  return PRODUCTS
}

