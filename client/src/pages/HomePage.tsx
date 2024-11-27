import React, { useEffect, useState } from 'react'
import { fetchGames } from '../services/api'
import { Game } from '../contracts/Game'
import GameCard from '../components/GameCard'
import { Container, Spinner, Alert } from 'react-bootstrap'

const HomePage: React.FC = () => {
	const [games, setGames] = useState<Game[]>([])
	const [loading, setLoading] = useState<boolean>(true)
	const [error, setError] = useState<string | null>(null)

	useEffect(() => {
		const loadGames = async () => {
			try {
				const data = await fetchGames()
				setGames(data)
			} catch (err) {
				setError('Failed to load games.')
			} finally {
				setLoading(false)
			}
		}

		loadGames()
	}, [])

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
		<Container>
			<div className='d-flex justify-content-between align-content-around flex-wrap'>
				{games.map(game => (
					<GameCard game={game} />
				))}
			</div>
		</Container>
	)
}

export default HomePage
