import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { Game } from '../contracts/Game'
import { fetchGameById } from '../services/api'
import { Container, Spinner, Alert, Card, Button } from 'react-bootstrap'
import './GamePage.css'

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

	function dateToLocaleString(date: string): string {
		return new Date(date).toLocaleDateString()
	}

	function toGb(bytes: number): number {
		const gb = bytes / 1024 / 1024 / 1024
		return Math.round((gb + Number.EPSILON) * 100) / 100
	}

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
				<Card className='game-img-card'>
					<Card.Img variant='top' src={game.imageUrl} alt={game.name} className='game-img' />
				</Card>

				<div className='game-info'>
					<Card className='game-detail'>
						<Card.Body className='game-detail-description'>
							<Card.Title className='fs-3'>
								<p>
									<strong>{game.name}</strong>
								</p>
							</Card.Title>
							<Card.Text className='fs-5'>
								<p>{game.type}</p>
							</Card.Text>
							<Card.Text className='fs-5'>
								<p>{game.description}</p>
							</Card.Text>
							<Card.Footer className='fs-6 game-detail-footer'>
								<p>
									<strong>Release date:</strong> {dateToLocaleString(game.releaseDate)}
								</p>
								<p>
									<strong>Publisher:</strong> {game.publisher}
								</p>
								<p>
									<strong>Size:</strong> {toGb(game.fileSize)} GB
								</p>
							</Card.Footer>
						</Card.Body>
					</Card>

					<Card className='game-detail'>
						<Card.Body className='game-detail-price'>
							<Card.Text className='fs-4 fw-bold'>
								{game.amount} {game.currency}
							</Card.Text>
							<Button variant='success' className='game-detail-button'>
								Add to order
							</Button>
						</Card.Body>
					</Card>
				</div>
			</Container>
		</>
	)
}

export default GamePage
