import React, { FormEvent } from 'react'
import { Card } from 'react-bootstrap'
import { Game } from '../../../../core/contracts/Game'

interface GameCardProps {
	game: Game | null
	onGameClick: (gameId: string, gameType: string) => void
}

const GameCard: React.FC<GameCardProps> = ({ game, onGameClick }) => {
	const handleGameClick = (event: FormEvent) => {
		event.preventDefault()

		if (!game) return

		onGameClick(game.id, game.type)
	}

	if (!game) return null

	return (
		<Card className='game-card my-4' onClick={handleGameClick}>
			<Card.Img src={game.imageUrl} alt={game.name} className='game-card-img' />
			<Card.Body>
				<Card.Text className='fs-5 fw-bold'>
					{game.amount} {game.currency}
				</Card.Text>
			</Card.Body>
		</Card>
	)
}

export default GameCard
