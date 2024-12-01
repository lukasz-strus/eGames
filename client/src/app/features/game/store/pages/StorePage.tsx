import React, { useEffect, useState } from 'react'
import { Game } from '../../../../core/contracts/Game'
import { Container, Spinner, Alert } from 'react-bootstrap'
import { useNavigate } from 'react-router-dom'
import { GameService } from '../../services/GameService'
import GamesSection from '../components/GamesSection'

const gameService = GameService.getInstance()

const StorePage: React.FC = () => {
	const [games, setGames] = useState<Game[]>([])
	const [loading, setLoading] = useState<boolean>(true)
	const [error, setError] = useState<string | null>(null)
	const navigate = useNavigate()

	useEffect(() => {
		const loadGames = async () => {
			try {
				const data = await gameService.fetchGames()
				setGames(data)
			} catch (err) {
				setError('Failed to load games.')
			} finally {
				setLoading(false)
			}
		}

		loadGames()
	}, [])

	const handleOnGameClick = (gameId: string, gameType: string) => {
		navigate(`/game/${gameType}/${gameId}`)
	}

	const groupedGames = {
		FullGame: games.filter(game => game.type === 'FullGame'),
		DlcGame: games.filter(game => game.type === 'DlcGame'),
		Subscription: games.filter(game => game.type === 'Subscription'),
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
			{Object.entries(groupedGames).map(([type, games]) => (
				<GamesSection key={type} games={games} type={type} onGameClick={handleOnGameClick} />
			))}
		</>
	)
}

export default StorePage
