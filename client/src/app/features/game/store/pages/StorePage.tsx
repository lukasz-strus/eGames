import React, { useEffect, useState } from 'react'
import { Game, Games } from '../../../../core/contracts/Game'
import { Container, Spinner, Alert, Form, Row, Col, Button } from 'react-bootstrap'
import { useNavigate } from 'react-router-dom'
import { GameService } from '../../services/GameService'
import GamesSection from '../components/GamesSection'
import { GameType } from '../../../../core/enums/GameType'

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
						<Form.Group>
							<Form.Label>Search by name:</Form.Label>
							<Form.Control
								type='text'
								placeholder='Enter game name'
								value={tempSearchQuery}
								onChange={e => setTempSearchQuery(e.target.value)}
							/>
						</Form.Group>
					</Col>
					<Col md={3}>
						<Form.Group>
							<Form.Label>Filter by game type:</Form.Label>
							<Form.Select value={tempSelectedType} onChange={e => setTempSelectedType(e.target.value)}>
								<option value='All'>All Games</option>
								<option value={GameType.FullGame}>Full Games</option>
								<option value={GameType.Subscription}>Subscription Games</option>
							</Form.Select>
						</Form.Group>
					</Col>
					<Col md={2}>
						<Form.Group>
							<Form.Label>Sort by:</Form.Label>
							<Form.Select value={tempSortBy} onChange={e => setTempSortBy(e.target.value)}>
								<option value='name'>Name ascending</option>
								<option value='-name'>Name descending</option>
								<option value='price'>Price ascending</option>
								<option value='-price'>Price descending</option>
							</Form.Select>
						</Form.Group>
					</Col>
					<Col md={2}>
						<Form.Group>
							<Form.Label>Results per page:</Form.Label>
							<Form.Select value={tempGamesPerPage} onChange={e => setTempGamesPerPage(Number(e.target.value))}>
								<option value='6'>6</option>
								<option value='9'>9</option>
								<option value='12'>12</option>
								<option value='15'>15</option>
							</Form.Select>
						</Form.Group>
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
