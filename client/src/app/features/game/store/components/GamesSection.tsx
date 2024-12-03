import React from 'react'
import { Game } from '../../../../core/contracts/Game'
import { Pagination, Container } from 'react-bootstrap'
import GameCard from './GameCard'
import { GameTypeExtensions } from '../../../../core/enums/GameType'

interface GamesSectionProps {
	games: Game[]
	type: string
	onGameClick: (gameId: string, gameType: string) => void
	currentPage: number
	totalPages: number
	onPageChange: (page: number) => void
}

const GamesSection: React.FC<GamesSectionProps> = ({
	games,
	type,
	onGameClick,
	currentPage,
	totalPages,
	onPageChange,
}) => {
	return (
		<Container className='content-container  mb-5'>
			<div className='section-container '>
				<h2 className='text-left mb-4'>{GameTypeExtensions.getGameTypes(type)}</h2>
				<div className='d-flex gap-5 justify-content-center flex-wrap'>
					{games.map(game => (
						<GameCard key={game.id} game={game} onGameClick={onGameClick} />
					))}
				</div>
				<Pagination className='mt-4 justify-content-center'>
					<Pagination.Prev disabled={currentPage === 1} onClick={() => onPageChange(currentPage - 1)} />
					{Array.from({ length: totalPages }, (_, index) => (
						<Pagination.Item key={index} active={index + 1 === currentPage} onClick={() => onPageChange(index + 1)}>
							{index + 1}
						</Pagination.Item>
					))}
					<Pagination.Next disabled={currentPage === totalPages} onClick={() => onPageChange(currentPage + 1)} />
				</Pagination>
			</div>
		</Container>
	)
}

export default GamesSection
