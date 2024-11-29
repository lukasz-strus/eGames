import React, { useEffect, useState } from 'react'
import { fetchGames } from '../../../core/services/api'
import { Game } from '../../../core/contracts/Game'
import GameCard from './components/GameCard'
import { Container, Spinner, Alert } from 'react-bootstrap'
import { useNavigate } from 'react-router-dom'

const StorePage: React.FC = () => {
	const [games, setGames] = useState<Game[]>([])
	const [loading, setLoading] = useState<boolean>(true)
	const [error, setError] = useState<string | null>(null)
	const navigate = useNavigate()

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

	function handleOnGameClick(gameId: string, gameType: string) {
		navigate(`/game/${gameType}/${gameId}`)
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
			<Container className='content-container'>
				<div className='d-flex justify-content-between align-content-around flex-wrap'>
					{games.map(game => (
						<GameCard key={game.id} game={game} onGameClick={handleOnGameClick} />
					))}
				</div>
			</Container>
		</>
	)
}

export default StorePage
