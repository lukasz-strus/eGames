import { Button, Card } from 'react-bootstrap'

interface GamePriceProps {
	amount: number
	currency: string
	onAddToOrder: () => void
}

const GamePrice: React.FC<GamePriceProps> = ({ amount, currency, onAddToOrder }) => {
	return (
		<Card className='game-detail'>
			<Card.Body className='game-detail-price'>
				<Card.Text className='fs-4 fw-bold'>
					{amount} {currency}
				</Card.Text>
				<Button variant='success' className='game-detail-button' onClick={onAddToOrder}>
					Add to order
				</Button>
			</Card.Body>
		</Card>
	)
}

export default GamePrice
