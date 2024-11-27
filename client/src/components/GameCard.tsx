import React from 'react'
import { Card } from 'react-bootstrap'
import { Game } from '../contracts/Game'
import './GameCard.css'

interface GameCardProps {
	game: Game
}

const GameCard: React.FC<GameCardProps> = ({ game }) => {
	return (
		<Card key={game.id} className='game-card my-3'>
			<Card.Img variant='top' src={game.imageUrl} alt={game.name} className='game-img' />
			<Card.Body className='d-flex flex-column justify-content-between'>
				<Card.Title>{game.name}</Card.Title>
				<Card.Text>{game.description}</Card.Text>
				<Card.Text className='game-price'>
					<strong>Price:</strong> {game.amount} {game.currency}
				</Card.Text>
				<a className='stretched-link' href='#' />
			</Card.Body>
		</Card>
	)
}

export default GameCard
