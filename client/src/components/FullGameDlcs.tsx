import { useNavigate } from 'react-router-dom'
import { FullGame } from '../contracts/Game'
import GameCard from './GameCard'

interface FullGameDlcsProps {
	game: FullGame
}

const FullGameDlcs: React.FC<FullGameDlcsProps> = ({ game }) => {
	const navigate = useNavigate()
	function handleOnGameClick(gameId: string, gameType: string) {
		navigate(`/game/${gameType}/${gameId}`)
	}

	return (
		<>
			{game.dlcGames.map(dlc => (
				<GameCard key={dlc.id} game={dlc} onGameClick={handleOnGameClick} />
			))}
		</>
	)
}

export default FullGameDlcs
