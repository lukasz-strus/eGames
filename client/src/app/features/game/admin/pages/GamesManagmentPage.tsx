import React, { useEffect, useState } from 'react'
import { Table, Container, Spinner, Alert, Row, Col, Button, Pagination } from 'react-bootstrap'
import { Game, Games } from '../../../../core/contracts/Game'
import { GameService } from '../../services/GameService'
import FormField from '../../../../core/components/FormField'
import { GameType } from '../../../../core/enums/GameType'
import { useNavigate } from 'react-router-dom'
import { AuthService } from '../../../auth/services/AuthService'

const gameService = GameService.getInstance()
const authService = AuthService.getInstance()

const GamesManagementPage: React.FC = () => {
	const [games, setGames] = useState<Game[]>([])
	const [deletedGames, setDeletedGames] = useState<Game[]>([])
	const [totalPages, setTotalPages] = useState<number>(1)
	const [deletedTotalPages, setDeletedTotalPages] = useState<number>(1)
	const [loading, setLoading] = useState<boolean>(false)
	const [error, setError] = useState<string | null>(null)

	const [searchQueryDeleted, setSearchQueryDeleted] = useState<string>('')
	const [selectedTypeDeleted, setSelectedTypeDeleted] = useState<string>('All')
	const [sortByDeleted, setSortByDeleted] = useState<string>('name')
	const [gamesPerPageDeleted, setGamesPerPageDeleted] = useState<number>(6)
	const [deletedPage, setDeletedPage] = useState<number>(1)

	const [tempSearchQueryDeleted, setTempSearchQueryDeleted] = useState<string>('')
	const [tempSelectedTypeDeleted, setTempSelectedTypeDeleted] = useState<string>('All')
	const [tempSortByDeleted, setTempSortByDeleted] = useState<string>('name')
	const [tempGamesPerPageDeleted, setTempGamesPerPageDeleted] = useState<number>(6)

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

	async function loadGames(query?: { filters: string; sorts: string; page: number; pageSize: number }, type?: string) {
		setLoading(true)
		setError(null)

		try {
			const gameQuery = query || {
				filters: searchQuery + ',isDeleted==false' || '',
				sorts: sortBy,
				page: currentPage,
				pageSize: gamesPerPage,
			}
			const gameType = type || selectedType

			let activeGamesResponse: Games | null = null

			switch (gameType) {
				case 'All':
					activeGamesResponse = await gameService.fetchGames(gameQuery)
					break
				case GameType.FullGame:
					activeGamesResponse = await gameService.fetchFullGames(gameQuery)
					break
				case GameType.Subscription:
					activeGamesResponse = await gameService.fetchSubscriptionGames(gameQuery)
					break
				default:
					throw new Error('Invalid game type selected.')
			}

			if (activeGamesResponse) {
				setGames(activeGamesResponse.items)
				setTotalPages(activeGamesResponse.totalPages)
			}
		} catch (err) {
			setError('Failed to load games.')
		} finally {
			setLoading(false)
		}
	}

	async function loadDeletedGames(
		query?: { filters: string; sorts: string; page: number; pageSize: number },
		type?: string
	) {
		setLoading(true)
		setError(null)

		try {
			const deletedQuery = query || {
				filters: searchQueryDeleted + ',isDeleted==true' || '',
				sorts: sortByDeleted,
				page: deletedPage,
				pageSize: gamesPerPageDeleted,
			}
			const gameType = type || selectedTypeDeleted

			let deletedGamesResponse: Games | null = null

			switch (gameType) {
				case 'All':
					deletedGamesResponse = await gameService.fetchGames(deletedQuery)
					break
				case GameType.FullGame:
					deletedGamesResponse = await gameService.fetchFullGames(deletedQuery)
					break
				case GameType.Subscription:
					deletedGamesResponse = await gameService.fetchSubscriptionGames(deletedQuery)
					break
				default:
					throw new Error('Invalid game type selected.')
			}
			if (deletedGamesResponse) {
				setDeletedGames(deletedGamesResponse.items)
				setDeletedTotalPages(deletedGamesResponse.totalPages)
			}
		} catch (err) {
			setError('Failed to load games.')
		} finally {
			setLoading(false)
		}
	}

	const handleSearch = async () => {
		const gameQuery = {
			filters: (tempSearchQuery ? `name@=*${tempSearchQuery},` : '') + 'isDeleted==false',
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

	const handleSearchDeleted = async () => {
		const deletedGameQuery = {
			filters: (tempSearchQueryDeleted ? `name@=*${tempSearchQueryDeleted},` : '') + 'isDeleted==true',
			sorts: tempSortByDeleted,
			page: 1,
			pageSize: tempGamesPerPageDeleted,
		}

		setSearchQueryDeleted(tempSearchQueryDeleted)
		setSelectedTypeDeleted(tempSelectedTypeDeleted)
		setSortByDeleted(tempSortByDeleted)
		setGamesPerPageDeleted(tempGamesPerPageDeleted)
		setDeletedPage(1)

		await loadDeletedGames(deletedGameQuery, tempSelectedTypeDeleted)
	}

	const handlePageChange = (newPage: number) => {
		setCurrentPage(newPage)
		window.scrollTo(0, 0)
	}

	const handleDeletedPageChange = (newPage: number) => {
		setDeletedPage(newPage)
		window.scrollTo(0, 0)
	}

	const handleRestoreGame = async (gameId: string) => {
		try {
			const token = localStorage.getItem('authToken')

			if (!token) {
				setError('You are not authorized to view this page.')
				return
			}

			await gameService.restoreGame(gameId, token)
			setDeletedGames(prev => prev.filter(game => game.id !== gameId))
			loadGames()
			loadDeletedGames()
		} catch (err) {
			setError('Failed to restore the game.')
		}
	}

	const handleRemoveGame = async (gameId: string) => {
		try {
			const token = localStorage.getItem('authToken')

			if (!token) {
				setError('You are not authorized to view this page.')
				return
			}

			await gameService.deleteGame(gameId, token)
			loadGames()
			loadDeletedGames()
		} catch (err) {
			setError('Failed to delete the game.')
		}
	}

	const isAdmin = async (token: string) => {
		try {
			const user = await authService.getProfile(token)
			if (user) {
				if (user.userRoles.items.some(role => role.name === 'Admin')) return true
				else return false
			} else {
				localStorage.removeItem('authToken')
				return false
			}
		} catch (error) {
			localStorage.removeItem('authToken')
		} finally {
			setLoading(false)
		}
	}

	useEffect(() => {
		const token = localStorage.getItem('authToken')

		if (!token) {
			setError('You are not authorized to view this page.')
			return
		}
		if (!isAdmin(token)) {
			setError('You are not authorized to view this page.')
			return
		}

		loadGames()
		loadDeletedGames()
	}, [currentPage, deletedPage])

	if (loading) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Spinner animation='border' />
			</Container>
		)
	}

	if (error) {
		return (
			<Container>
				<Alert variant='danger'>{error}</Alert>
			</Container>
		)
	}

	return (
		<>
			<Container className='section-container justify-content-center align-content-end'>
				<h3>Active Games</h3>
				<Row className='justify-content-left mt-4'>
					<Col md={3}>
						<FormField
							label='Search by name'
							type='text'
							floatingLabel={true}
							value={tempSearchQuery}
							onChange={e => setTempSearchQuery(e.target.value)}
						/>
					</Col>

					<Col md={3}>
						<FormField
							label='Filter by game type'
							type='select'
							floatingLabel={true}
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
							floatingLabel={true}
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
							floatingLabel={true}
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
						<Button variant='primary' className='mt-2' onClick={handleSearch}>
							<i className='fas fa-search'></i> Search
						</Button>
					</Col>
				</Row>
				<Table bordered hover className='mt-4'>
					<thead>
						<tr>
							<th className='text-center'>Name</th>
							<th className='text-center'>Type</th>
							<th className='text-center'>Price</th>
							<th className='text-center'>Action</th>
						</tr>
					</thead>
					<tbody>
						{games.map(game => (
							<tr key={game.id}>
								<td className='text-center align-middle'>{game.name}</td>
								<td className='text-center align-middle'>{game.type}</td>
								<td className='text-center align-middle'>
									{game.amount.toFixed(2)} {game.currency}
								</td>
								<td className='text-center align-middle'>
									<Button
										variant='warning'
										className='me-2'
										onClick={() => navigate(`/game-form/${game.type}/${game.id}`)}>
										Edit
									</Button>
									<Button
										variant='danger'
										onClick={() => {
											if (window.confirm('Are you sure you want to delete this game?')) {
												handleRemoveGame(game.id)
											}
										}}>
										Remove
									</Button>
								</td>
							</tr>
						))}
					</tbody>
				</Table>
				<div className='d-flex justify-content-between align-items-center mt-4'>
					<Pagination>
						<Pagination.Prev disabled={currentPage === 1} onClick={() => handlePageChange(currentPage - 1)} />
						{Array.from({ length: totalPages }, (_, index) => (
							<Pagination.Item
								key={index}
								active={index + 1 === currentPage}
								onClick={() => handlePageChange(index + 1)}>
								{index + 1}
							</Pagination.Item>
						))}
						<Pagination.Next disabled={currentPage === totalPages} onClick={() => handlePageChange(currentPage + 1)} />
					</Pagination>
					<Button variant='success' onClick={() => navigate('/game-form')}>
						Add New Game
					</Button>
				</div>
			</Container>

			<Container className='section-container '>
				<h3>Deleted Games</h3>
				<Row className='justify-content-left mt-4 mb-3'>
					<Col md={3}>
						<FormField
							label='Search by name'
							type='text'
							floatingLabel={true}
							value={tempSearchQueryDeleted}
							onChange={e => setTempSearchQueryDeleted(e.target.value)}
						/>
					</Col>
					<Col md={3}>
						<FormField
							label='Filter by game type'
							type='select'
							floatingLabel={true}
							value={tempSelectedTypeDeleted}
							options={[
								{ value: 'All', label: 'All Games' },
								{ value: GameType.FullGame, label: 'Full Games' },
								{ value: GameType.Subscription, label: 'Subscription Games' },
							]}
							onChange={e => setTempSelectedTypeDeleted(e.target.value)}
						/>
					</Col>
					<Col md={2}>
						<FormField
							label='Sort by'
							type='select'
							floatingLabel={true}
							value={tempSortByDeleted}
							options={[
								{ value: 'name', label: 'Name asc.' },
								{ value: '-name', label: 'Name desc.' },
							]}
							onChange={e => setTempSortByDeleted(e.target.value)}
						/>
					</Col>
					<Col md={2}>
						<FormField
							label='Results per page'
							type='select'
							floatingLabel={true}
							value={tempGamesPerPageDeleted}
							options={[
								{ value: 6, label: '6' },
								{ value: 9, label: '9' },
								{ value: 12, label: '12' },
							]}
							onChange={e => setTempGamesPerPageDeleted(Number(e.target.value))}
						/>
					</Col>
					<Col md={1} className='text-center'>
						<Button variant='primary' className='mt-2' onClick={handleSearchDeleted}>
							Search
						</Button>
					</Col>
				</Row>
				<Table bordered hover>
					<thead>
						<tr>
							<th className='text-center'>Name</th>
							<th className='text-center'>Type</th>
							<th className='text-center'>Price</th>
							<th className='text-center'>Action</th>
						</tr>
					</thead>
					<tbody>
						{deletedGames.map(game => (
							<tr key={game.id}>
								<td className='text-center align-middle'>{game.name}</td>
								<td className='text-center align-middle'>{game.type}</td>
								<td className='text-center align-middle'>
									{game.amount.toFixed(2)} {game.currency}
								</td>
								<td className='text-center align-middle'>
									<Button variant='primary' onClick={() => handleRestoreGame(game.id)}>
										Restore
									</Button>
								</td>
							</tr>
						))}
					</tbody>
				</Table>
				<div className='d-flex justify-content-between align-items-center mt-4'>
					<Pagination>
						<Pagination.Prev disabled={deletedPage === 1} onClick={() => handleDeletedPageChange(deletedPage - 1)} />
						{Array.from({ length: deletedTotalPages }, (_, index) => (
							<Pagination.Item
								key={index}
								active={index + 1 === deletedPage}
								onClick={() => handleDeletedPageChange(index + 1)}>
								{index + 1}
							</Pagination.Item>
						))}
						<Pagination.Next
							disabled={deletedPage === deletedTotalPages}
							onClick={() => handleDeletedPageChange(deletedPage + 1)}
						/>
					</Pagination>
				</div>
			</Container>
		</>
	)
}

export default GamesManagementPage
