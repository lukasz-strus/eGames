import React, { useEffect, useState } from 'react'
import { Game } from '../../../../core/contracts/Game'
import { Container, Spinner, Alert, Form, Row, Col } from 'react-bootstrap'
import { useNavigate } from 'react-router-dom'
import { GameService } from '../../services/GameService'
import GamesSection from '../components/GamesSection'

const gameService = GameService.getInstance()

const StorePage: React.FC = () => {
	const [games, setGames] = useState<Game[]>([])
	const [filteredGames, setFilteredGames] = useState<Game[]>([])
	const [loading, setLoading] = useState<boolean>(true)
	const [error, setError] = useState<string | null>(null)
	const [selectedType, setSelectedType] = useState<string>('All')
	const [searchQuery, setSearchQuery] = useState<string>('')
	const [gamesPerPage, setGamesPerPage] = useState<number>(6) // Default games per page
	const navigate = useNavigate()

	const [currentPage, setCurrentPage] = useState<Record<string, number>>({
		FullGame: 1,
		Subscription: 1,
	})

	useEffect(() => {
		const loadGames = async () => {
			setLoading(true)
			setError(null)

			try {
				let data: Game[] = []
				switch (selectedType) {
					case 'FullGame':
						data = await gameService.fetchFullGames()
						break
					case 'Subscription':
						data = await gameService.fetchSubscriptionGames()
						break
					default:
						data = await gameService.fetchGames()
				}
				setGames(data)
				setFilteredGames(data)
			} catch (err) {
				setError('Failed to load games.')
			} finally {
				setLoading(false)
			}
		}

		loadGames()
	}, [selectedType])

	useEffect(() => {
		const filtered = games.filter(game => game.name.toLowerCase().includes(searchQuery.toLowerCase()))
		setFilteredGames(filtered)
	}, [searchQuery, games])

	const handleOnGameClick = (gameId: string, gameType: string) => {
		navigate(`/game/${gameType}/${gameId}`)
	}

	const handlePageChange = (type: string, newPage: number) => {
		setCurrentPage(prevState => ({
			...prevState,
			[type]: newPage,
		}))
	}

	const handleGamesPerPageChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
		setGamesPerPage(Number(e.target.value))
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
			<Container>
				{/* Combobox for filtering and search */}
				<Row className='align-items-center'>
					<Col md={6}>
						<Form.Group>
							<Form.Label>Search by name:</Form.Label>
							<Form.Control
								type='text'
								placeholder='Enter game name'
								value={searchQuery}
								onChange={e => setSearchQuery(e.target.value)}
							/>
						</Form.Group>
					</Col>
					<Col md={3}>
						<Form.Group>
							<Form.Label>Filter by game type:</Form.Label>
							<Form.Select value={selectedType} onChange={e => setSelectedType(e.target.value)}>
								<option value='All'>All Games</option>
								<option value='FullGame'>Full Games</option>
								<option value='Subscription'>Subscription Games</option>
							</Form.Select>
						</Form.Group>
					</Col>

					<Col md={3}>
						<Form.Group>
							<Form.Label>Results per page:</Form.Label>
							<Form.Select value={gamesPerPage} onChange={handleGamesPerPageChange}>
								<option value='6'>6</option>
								<option value='9'>9</option>
								<option value='12'>12</option>
								<option value='15'>15</option>
							</Form.Select>
						</Form.Group>
					</Col>
				</Row>
			</Container>
			{selectedType === 'All' ? (
				Object.entries({
					FullGame: filteredGames.filter(game => game.type === 'FullGame'),
					Subscription: filteredGames.filter(game => game.type === 'Subscription'),
				}).map(([type, games]) => {
					const startIndex = (currentPage[type] - 1) * gamesPerPage
					const paginatedGames = games.slice(startIndex, startIndex + gamesPerPage)
					return (
						<GamesSection
							key={type}
							games={paginatedGames}
							type={type}
							onGameClick={handleOnGameClick}
							currentPage={currentPage[type]}
							totalPages={Math.ceil(games.length / gamesPerPage)}
							onPageChange={newPage => handlePageChange(type, newPage)}
						/>
					)
				})
			) : (
				<GamesSection
					key={selectedType}
					games={filteredGames.slice(
						(currentPage[selectedType] - 1) * gamesPerPage,
						currentPage[selectedType] * gamesPerPage
					)}
					type={selectedType}
					onGameClick={handleOnGameClick}
					currentPage={currentPage[selectedType]}
					totalPages={Math.ceil(filteredGames.length / gamesPerPage)}
					onPageChange={newPage => handlePageChange(selectedType, newPage)}
				/>
			)}
		</>
	)
}

export default StorePage
