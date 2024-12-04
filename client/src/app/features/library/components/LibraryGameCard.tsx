import React, { FormEvent } from 'react'
import { Card } from 'react-bootstrap'
import { LibraryGame } from '../../../core/contracts/LibraryGame'

interface LibraryGameCardProps {
	libraryGame: LibraryGame | null
	onGameClick: (gameId: string) => void
}

const LibraryGameCard: React.FC<LibraryGameCardProps> = ({ libraryGame, onGameClick }) => {
	const handleGameClick = (event: FormEvent) => {
		event.preventDefault()

		if (!libraryGame) return

		onGameClick(libraryGame.id)
	}

	if (!libraryGame) return null

	return (
		<Card className='library-game-card my-4' onClick={handleGameClick}>
			<Card.Img src={libraryGame.imageUrl} alt={libraryGame.name} className='library-game-card-img' />
		</Card>
	)
}

export default LibraryGameCard
