import React from 'react'
import { Card } from 'react-bootstrap'
import { Game } from '../contracts/Game'

interface GameCardProps {
	game: Game
}

const GameCard: React.FC<GameCardProps> = ({ game }) => {
	return (
		<Card key={game.id} style={{ minWidth: '250px' }}>
			<Card.Body>
				<Card.Title>{game.name}</Card.Title>
				<Card.Text>{game.description}</Card.Text>
				<Card.Text className='game-price'>
					<strong>Price:</strong> {game.amount} {game.currency}
				</Card.Text>
			</Card.Body>
		</Card>
	)
}

export default GameCard
