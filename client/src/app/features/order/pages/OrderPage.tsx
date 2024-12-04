import React, { useEffect, useState } from 'react'
import { Table, Container, Button, Alert } from 'react-bootstrap'
import { OrderService } from '../services/OrderService'
import { Order } from '../../../core/contracts/Order'
import { GameService } from '../../../features/game/services/GameService'
import { Game } from '../../../core/contracts/Game'
import { useNavigate } from 'react-router-dom'

const orderService = OrderService.getInstance()
const gameService = GameService.getInstance()

const OrderPage: React.FC = () => {
	const [order, setOrder] = useState<Order | null>(null)
	const [gameDetails, setGameDetails] = useState<Map<string, Game>>(new Map())
	const [loading, setLoading] = useState<boolean>(false)
	const [error, setError] = useState<string | null>(null)
	const [successMessage, setSuccessMessage] = useState<string | null>(null)
	const navigate = useNavigate()

	const fetchOrder = async (token: string) => {
		setLoading(true)
		setError(null)

		try {
			const orderData = await orderService.getOrder(token)
			if (!orderData) return
			setOrder(orderData)

			const gameDetails = new Map<string, Game>()
			for (const item of orderData.orderItems) {
				const game = await gameService.fetchGameById(item.gameId)
				gameDetails.set(item.gameId, game)
			}
			setGameDetails(gameDetails)
		} catch (err) {
			setError('Failed to load order.')
		} finally {
			setLoading(false)
		}
	}

	useEffect(() => {
		const token = localStorage.getItem('authToken')

		if (token) {
			fetchOrder(token)
		} else {
			setError('You need to be logged in to view your order.')
		}
	}, [])

	const handleRemoveItem = async (itemId: string) => {
		try {
			const token = localStorage.getItem('authToken')
			if (!token) {
				navigate('/login')
				return
			}

			if (!order) {
				setError('Failed to remove item.')
				return
			}

			await orderService.removeOrderItem(order.id, itemId, token)
			setOrder(prevOrder => {
				if (!prevOrder) return null
				return {
					...prevOrder,
					orderItems: prevOrder.orderItems.filter(item => item.id !== itemId),
				}
			})
			setSuccessMessage('Item removed successfully.')
		} catch (err) {
			setError('Failed to remove item.')
		}
	}

	const handlePayment = async () => {
		try {
			const token = localStorage.getItem('authToken')
			if (!token) {
				navigate('/login')
				return
			}

			if (!order) {
				setError('Failed to pay.')
				return
			}

			await orderService.payOrder(order.id, token)
			setOrder(null)
			setSuccessMessage('Payment successful. Your order is complete!')
		} catch (err) {
			setError('Failed to process payment.')
		}
	}

	const calculateTotalPrice = () => {
		return order?.orderItems.reduce((total, item) => total + item.amount, 0).toFixed(2) || '0.00'
	}

	if (loading) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<div>Loading...</div>
			</Container>
		)
	}

	if (error) {
		return (
			<Container>
				<Alert variant='danger'>{error}</Alert>
			</Container>
		)
	}

	return (
		<Container>
			{successMessage && <Alert variant='success'>{successMessage}</Alert>}

			{order?.orderItems && order.orderItems.length > 0 ? (
				<>
					<Table bordered hover className='mt-4'>
						<thead>
							<tr>
								<th>Game</th>
								<th>Name</th>
								<th>Price</th>
								<th>Action</th>
							</tr>
						</thead>
						<tbody>
							{order.orderItems.map(item => {
								const game = gameDetails.get(item.gameId)
								return (
									<tr key={item.id}>
										<td>{game?.name || 'Loading...'}</td>
										<td>{game?.amount || 'Loading...'}</td>
										<td>
											{item.amount.toFixed(2)} {item.currency}
										</td>
										<td>
											<Button variant='danger' onClick={() => handleRemoveItem(item.id)}>
												Remove
											</Button>
										</td>
									</tr>
								)
							})}
						</tbody>
					</Table>
					<div className='d-flex justify-content-between align-items-center mt-4'>
						<h4>
							Total Price: {calculateTotalPrice()} {order?.orderItems[0]?.currency || 'PLN'}
						</h4>
						<Button variant='success' onClick={handlePayment}>
							Pay Now
						</Button>
					</div>
				</>
			) : (
				<Alert variant='info' className='text-center mt-4'>
					Your order is empty.
				</Alert>
			)}
		</Container>
	)
}

export default OrderPage
