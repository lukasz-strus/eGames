export interface OrderItem {
	id: string
	gameId: string
	currency: string
	amount: number
}

export interface Order {
	id: string
	userId: string
	status: string
	orderItems: OrderItem[]
}
