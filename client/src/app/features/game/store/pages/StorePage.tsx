import React, { useEffect, useState } from 'react'
import { Game, Games } from '../../../../core/contracts/Game'
import { Container, Spinner, Alert, Row, Col, Button } from 'react-bootstrap'
import { useNavigate } from 'react-router-dom'
import { GameService } from '../../services/GameService'
import GamesSection from '../components/GamesSection'
import { GameType } from '../../../../core/enums/GameType'
import FormField from '../../../../core/components/FormField'

const gameService = GameService.getInstance()

const StorePage: React.FC = () => {
	const [games, setGames] = useState<Game[]>([])
	const [totalPages, setTotalPages] = useState<number>(1)
	const [loading, setLoading] = useState<boolean>(false)
	const [error, setError] = useState<string | null>(null)

	const [searchQuery, setSearchQuery] = useState<string>('')
	const [selectedType, setSelectedType] = useState<string>('All')
	const [sortBy, setSortBy] = useState<string>('name')
	const [gamesPerPage, setGamesPerPage] = useState<number>(6)
	const [currentPage, setCurrentPage] = useState<number>(1)

	const [tempSearchQuery, setTempSearchQuery] = useState<string>('')
	const [tempSelectedType, setTempSelectedType] = useState<string>('All')
	const [tempSortBy, setTempSortBy] = useState<string>('name')
	const [tempGamesPerPage, setTempGamesPerPage] = useState<number>(6)

	const navigate = useNavigate()

	const loadGames = async (
		query?: { filters: string; sorts: string; page: number; pageSize: number },
		type?: string
	) => {
		setLoading(true)
		setError(null)

		try {
			const gameQuery = query || {
				filters: searchQuery || '',
				sorts: sortBy,
				page: currentPage,
				pageSize: gamesPerPage,
			}

			const gameType = type || selectedType

			let data: Games | null = null

			switch (gameType) {
				case 'All':
					data = await gameService.fetchGames(gameQuery)
					break
				case GameType.FullGame:
					data = await gameService.fetchFullGames(gameQuery)
					break
				case GameType.Subscription:
					data = await gameService.fetchSubscriptionGames(gameQuery)
					break
				default:
					throw new Error('Invalid game type selected.')
			}

			if (!data) {
				throw new Error('No data received from the server.')
			}

			setGames(data.items)
			setTotalPages(data.totalPages)
		} catch (err) {
			setError('Failed to load games.')
		} finally {
			setLoading(false)
		}
	}

	useEffect(() => {
		loadGames()
	}, [currentPage])

	const handleSearch = async () => {
		const gameQuery = {
			filters: tempSearchQuery || '',
			sorts: tempSortBy,
			page: 1,
			pageSize: tempGamesPerPage,
		}

		setSearchQuery(tempSearchQuery)
		setSelectedType(tempSelectedType)
		setSortBy(tempSortBy)
		setGamesPerPage(tempGamesPerPage)
		setCurrentPage(1)

		await loadGames(gameQuery, tempSelectedType)
	}

	const handlePageChange = (newPage: number) => {
		setCurrentPage(newPage)

		window.scrollTo(0, 0)
	}

	if (loading) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Spinner animation='border' />
			</Container>
		)
	}

	if (error) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Alert variant='danger'>{error}</Alert>
			</Container>
		)
	}

	return (
		<>
			<div className='bcg-image' />
			<Container className='section-container justify-content-center align-content-end'>
				<Row className='justify-content-center'>
					<Col md={3}>
						<FormField
							label='Search by name'
							type='text'
							placeholder='Enter game name'
							value={tempSearchQuery}
							onChange={e => setTempSearchQuery(e.target.value)}
						/>
					</Col>

					<Col md={3}>
						<FormField
							label='Filter by game type'
							type='select'
							value={tempSelectedType}
							options={[
								{ value: 'All', label: 'All Games' },
								{ value: GameType.FullGame, label: 'Full Games' },
								{ value: GameType.Subscription, label: 'Subscription Games' },
							]}
							onChange={e => setTempSelectedType(e.target.value)}
						/>
					</Col>

					<Col md={2}>
						<FormField
							label='Sort by'
							type='select'
							value={tempSortBy}
							options={[
								{ value: 'name', label: 'Name asc.' },
								{ value: '-name', label: 'Name desc.' },
							]}
							onChange={e => setTempSortBy(e.target.value)}
						/>
					</Col>

					<Col md={2}>
						<FormField
							label='Results per page'
							type='select'
							value={tempGamesPerPage}
							options={[
								{ value: 6, label: '6' },
								{ value: 9, label: '9' },
								{ value: 12, label: '12' },
								{ value: 15, label: '15' },
							]}
							onChange={e => setTempGamesPerPage(Number(e.target.value))}
						/>
					</Col>
					<Col md={1} className='text-center'>
						<Button variant='primary' className='mt-4' onClick={handleSearch}>
							<i className='fas fa-search'></i> Search
						</Button>
					</Col>
				</Row>
			</Container>
			<GamesSection
				games={games}
				type={selectedType === 'All' ? 'All Games' : selectedType}
				onGameClick={(gameId, gameType) => navigate(`/game/${gameType}/${gameId}`)}
				currentPage={currentPage}
				totalPages={totalPages}
				onPageChange={handlePageChange}
			/>
		</>
	)
}

export default StorePage
