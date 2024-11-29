import { Card } from 'react-bootstrap'
import { type Game, type FullGame, type Subscription } from '../contracts/Game'
import FullGameInfo from './FullGameInfo'
import SubscriptionInfo from './SubscriptionInfo'

interface GameInfoProps {
	game: Game
}

const GameInfo: React.FC<GameInfoProps> = ({ game }) => {
	function dateToLocaleString(date: string): string {
		return new Date(date).toLocaleDateString()
	}

	function toGb(bytes: number): number {
		const gb = bytes / 1024 / 1024 / 1024
		return Math.round((gb + Number.EPSILON) * 100) / 100
	}

	return (
		<Card className='game-detail'>
			<Card.Body className='game-detail-description'>
				<Card.Title className='fs-3'>
					<strong>{game.name}</strong>
				</Card.Title>
				<Card.Text className='fs-5'>{game.type}</Card.Text>
				<Card.Text className='fs-5'>{game.description}</Card.Text>
				<Card.Footer className='fs-6 game-detail-footer'>
					<div>
						<strong>Release date:</strong> {dateToLocaleString(game.releaseDate)}
					</div>
					<div>
						<strong>Publisher:</strong> {game.publisher}
					</div>
					<div>
						<strong>Size:</strong> {toGb(game.fileSize)} GB
					</div>
					{game.type === 'Full game' && <FullGameInfo game={game as FullGame} />}
					{game.type === 'Subscription' && <SubscriptionInfo game={game as Subscription} />}
				</Card.Footer>
			</Card.Body>
		</Card>
	)
}

export default GameInfo
