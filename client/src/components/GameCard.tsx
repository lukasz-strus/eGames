import React, { FormEvent } from 'react'
import { Card } from 'react-bootstrap'
import { Game } from '../contracts/Game'
import './GameCard.css'

interface GameCardProps {
	game: Game
	onGameClick: (gameId: string) => void
}

const GameCard: React.FC<GameCardProps> = ({ game, onGameClick }) => {
	const handleGameClick = (event: FormEvent) => {
		event.preventDefault()

		onGameClick(game.id)
	}

	return (
		<Card className='game-card my-4' onClick={handleGameClick}>
			<Card.Img src={game.imageUrl} alt={game.name} className='game-img' />
			<Card.Body>
				<Card.Text className='fs-5 fw-bold'>
					{game.amount} {game.currency}
				</Card.Text>
			</Card.Body>
		</Card>
	)
}

export default GameCard
