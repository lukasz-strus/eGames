import { Container } from 'react-bootstrap'
import GameCard from './GameCard'
import { Game } from '../../../../core/contracts/Game'

interface GamesSectionProps {
	games: Game[] | null
	type: string
	onGameClick: (gameId: string, gameType: string) => void
}

const GamesSection: React.FC<GamesSectionProps> = ({ games, type, onGameClick }) => {
	if (!games) return null

	const toDisplayType = (type: string) => {
		switch (type) {
			case 'FullGame':
				return 'Full Games'
			case 'DlcGame':
				return 'DLCs'
			case 'Subscription':
				return 'Subscriptions'
			default:
				return ''
		}
	}

	return (
		<Container className='content-container'>
			<div className='section-container'>
				<h2 className='text-center'>{toDisplayType(type)}</h2>
				<div className='d-flex gap-5 align-content-around flex-wrap'>
					{games.map(game => (
						<GameCard key={game.id} game={game} onGameClick={onGameClick} />
					))}
				</div>
			</div>
		</Container>
	)
}

export default GamesSection
