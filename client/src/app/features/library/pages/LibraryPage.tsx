import React, { useEffect, useState } from 'react'
import { Container, Spinner, Alert, Row, Col, Button } from 'react-bootstrap'
import { LibraryGame, LibraryGames } from '../../../core/contracts/LibraryGame'
import { useNavigate } from 'react-router-dom'
import { LibraryService } from '../services/LibraryService'
import FormField from '../../../core/components/FormField'
import LibrarySection from '../components/LibrarySection'
import { AuthService } from '../../auth/services/AuthService'

const libraryService = LibraryService.getInstance()
const authService = AuthService.getInstance()

const LibraryPage: React.FC = () => {
	const [libraryGames, setLibraryGames] = useState<LibraryGame[]>([])
	const [totalPages, setTotalPages] = useState<number>(1)
	const [loading, setLoading] = useState<boolean>(false)
	const [error, setError] = useState<string | null>(null)

	const [searchQuery, setSearchQuery] = useState<string>('')
	const [sortBy, setSortBy] = useState<string>('name')
	const [gamesPerPage, setGamesPerPage] = useState<number>(6)
	const [currentPage, setCurrentPage] = useState<number>(1)

	const [tempSearchQuery, setTempSearchQuery] = useState<string>('')
	const [tempSortBy, setTempSortBy] = useState<string>('name')
	const [tempGamesPerPage, setTempGamesPerPage] = useState<number>(6)

	const navigate = useNavigate()

	const isCustomer = async (token: string) => {
		try {
			const user = await authService.getProfile(token)
			if (user) {
				if (user.userRoles.items.some(role => role.name === 'Customer')) return true
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

	const loadLibraryGames = async (query?: { filters: string; sorts: string; page: number; pageSize: number }) => {
		setLoading(true)
		setError(null)

		try {
			const token = localStorage.getItem('authToken')

			if (!token) {
				setError('You are not authorized to view this page.')
				return
			}

			if (!isCustomer(token)) {
				setError('You are not authorized to view this page.')
				return
			}

			const libraryQuery = query || {
				filters: searchQuery || '',
				sorts: sortBy,
				page: currentPage,
				pageSize: gamesPerPage,
			}

			const data: LibraryGames = await libraryService.getOwnLibrary(libraryQuery, token)

			setLibraryGames(data.items)
			setTotalPages(data.totalPages)
		} catch (err) {
			setError('Failed to load library games.')
		} finally {
			setLoading(false)
		}
	}

	const handleSearch = async () => {
		const libraryQuery = {
			filters: tempSearchQuery || '',
			sorts: tempSortBy,
			page: 1,
			pageSize: tempGamesPerPage,
		}

		setSearchQuery(tempSearchQuery)
		setSortBy(tempSortBy)
		setGamesPerPage(tempGamesPerPage)
		setCurrentPage(1)

		await loadLibraryGames(libraryQuery)
	}

	const handlePageChange = (newPage: number) => {
		setCurrentPage(newPage)
		window.scrollTo(0, 0)
	}

	useEffect(() => {
		loadLibraryGames()
	}, [currentPage])

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
			<div className='bcg-image' />
			<Container className='section-container justify-content-center align-content-center'>
				<Row className='justify-content-center mt-3'>
					<Col md={3}>
						<FormField
							label='Search by name'
							type='text'
							floatingLabel={true}
							value={tempSearchQuery}
							onChange={e => setTempSearchQuery(e.target.value)}
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
			</Container>
			<LibrarySection
				libraryGames={libraryGames}
				currentPage={currentPage}
				totalPages={totalPages}
				onPageChange={handlePageChange}
				onGameClick={libraryGameId => navigate(`/library/${libraryGameId}`)}
			/>
		</>
	)
}

export default LibraryPage
