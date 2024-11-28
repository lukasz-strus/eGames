import { FullGame } from '../contracts/Game'

interface FullGameInfoProps {
	game: FullGame
}

const FullGameInfo: React.FC<FullGameInfoProps> = ({ game }) => {
	return (
		<div>
			<strong>DLC Count:</strong> {game.dlcGames.length}
		</div>
	)
}

export default FullGameInfo
