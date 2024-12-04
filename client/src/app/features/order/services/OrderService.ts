import { ApiService } from '../../../core/services/ApiService'
import ApiEndpoints from '../../../core/constants/ApiEndpoints'
import { Order } from '../../../core/contracts/Order'

export class OrderService extends ApiService {
	private static instance: OrderService | null = null

	private constructor() {
		super(ApiEndpoints.BASE_URL)
	}

	static getInstance(): OrderService {
		if (!OrderService.instance) {
			OrderService.instance = new OrderService()
		}
		return OrderService.instance
	}

	async getOrder(token: string): Promise<Order | null> {
		const { data } = await this.API.get(ApiEndpoints.ORDER.OWN, this.setAuthorizationHeader(token))
		if (!data) return null
		if (data.orders.length === 0) return null

		return data.orders.find((order: { status: string }) => order.status === 'Pending')
	}

	async addOrderItem(gameId: string, token: string): Promise<void> {
		let orderId = ''
		let order = await this.getOrder(token)
		if (!order) {
			const response = await this.API.post(ApiEndpoints.ORDER.BASE, {}, this.setAuthorizationHeader(token))

			orderId = response.data.id
		} else {
			orderId = order.id
		}

		await this.API.post(ApiEndpoints.ORDER.ITEMS(orderId), { gameId }, this.setAuthorizationHeader(token))
	}

	async removeOrderItem(orderId: string, itemId: string, token: string): Promise<void> {
		await this.API.delete(ApiEndpoints.ORDER.ITEM(orderId, itemId), this.setAuthorizationHeader(token))

		const order = await this.getOrder(token)
		if (order?.orderItems.length === 0) {
			await this.API.delete(`${ApiEndpoints.ORDER.BASE}/${orderId}`, this.setAuthorizationHeader(token))
		}
	}

	async payOrder(orderId: string, token: string): Promise<void> {
		await this.API.post(ApiEndpoints.ORDER.PAY(orderId), {}, this.setAuthorizationHeader(token))
	}
}
