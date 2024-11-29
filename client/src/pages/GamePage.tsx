import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { type FullGame, type Game } from '../contracts/Game'
import { fetchGameById } from '../services/api'
import { Container, Spinner, Alert } from 'react-bootstrap'
import './GamePage.css'
import GameInfo from '../components/GameInfo'
import GamePrice from '../components/GamePrice'
import GameImage from '../components/GameImage'
import FullGameDlcs from '../components/FullGameDlcs'

const GamePage: React.FC = () => {
	const { gameId, gameType } = useParams<{ gameId: string; gameType: string }>()
	const [game, setGame] = useState<Game | null>(null)
	const [loading, setLoading] = useState<boolean>(true)
	const [error, setError] = useState<string | null>(null)

	useEffect(() => {
		const loadGame = async () => {
			try {
				if (gameId && gameType) {
					const data = await fetchGameById(gameId, gameType)
					setGame(data)
				} else {
					setError('Invalid game type or ID.')
				}
			} catch (err) {
				setError('Failed to load the game.')
			} finally {
				setLoading(false)
			}
		}

		loadGame()
	}, [gameId])

	if (loading)
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Spinner animation='border' />
			</Container>
		)

	if (error)
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Alert variant='danger'>{error}</Alert>
			</Container>
		)

	if (!game) return null

	return (
		<>
			<div className='bcg-image' />
			<Container className='my-5 game-container'>
				<GameImage src={game.imageUrl} alt={game.name} />
				<div className='game-info'>
					<GameInfo game={game} />
					<GamePrice amount={game.amount} currency={game.currency} />
				</div>
			</Container>

			{game.type === 'FullGame' && (
				<Container className='content-container'>
					<h1 className='fs-2'>DLC:</h1>
					<div className='d-flex gap-3  align-content-around flex-wrap'>
						<FullGameDlcs game={game as FullGame} />
					</div>
				</Container>
			)}
		</>
	)
}

export default GamePage
