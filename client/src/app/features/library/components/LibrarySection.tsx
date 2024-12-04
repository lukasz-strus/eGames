import React from 'react'
import { LibraryGame } from '../../../core/contracts/LibraryGame'
import { Pagination, Container } from 'react-bootstrap'
import LibraryGameCard from './LibraryGameCard'

interface LibrarySectionProps {
	libraryGames: LibraryGame[]
	currentPage: number
	totalPages: number
	onPageChange: (page: number) => void
	onGameClick: (gameId: string) => void
}

const LibrarySection: React.FC<LibrarySectionProps> = ({
	libraryGames,
	currentPage,
	totalPages,
	onPageChange,
	onGameClick,
}) => {
	return (
		<Container className='content-container mb-5'>
			<div className='section-container'>
				<div className='d-flex gap-5 justify-content-center flex-wrap'>
					{libraryGames.map(libraryGame => (
						<LibraryGameCard key={libraryGame.id} libraryGame={libraryGame} onGameClick={onGameClick} />
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

export default LibrarySection
