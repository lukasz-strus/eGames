import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { DlcGame, type FullGame, type Game } from '../../../../core/contracts/Game'
import { Container, Spinner, Alert } from 'react-bootstrap'
import GameInfo from '../components/GameInfo'
import GamePrice from '../components/GamePrice'
import GameImage from '../components/GameImage'
import FullGameDlcs from '../components/FullGameDlcs'
import GameCard from '../components/GameCard'
import { GameService } from '../../services/GameService'
import { GameType } from '../../../../core/enums/GameType'
import { OrderService } from '../../../order/services/OrderService'
import SuccessModal from '../../../../core/components/SuccessModal'
import LoginModal from '../../../auth/components/LoginModal'
import { UserRole } from '../../../../core/contracts/User'

const gameService = GameService.getInstance()
const orderService = OrderService.getInstance()

const GamePage: React.FC = () => {
	const { gameId, gameType } = useParams<{ gameId: string; gameType: GameType }>()
	const [game, setGame] = useState<Game | null>(null)
	const [baseGame, setBaseGame] = useState<FullGame | null>(null)
	const [loading, setLoading] = useState<boolean>(true)
	const [error, setError] = useState<string | null>(null)
	const [showLoginModal, setShowLoginModal] = useState(false)
	const [showSuccessModal, setShowSuccessModal] = useState(false)
	const navigate = useNavigate()

	function handleOnGameClick(gameId: string, gameType: string) {
		navigate(`/game/${gameType}/${gameId}`)
	}

	const addToOrderAfterLogin = (roles: UserRole[]) => {
		if (!roles.some(role => role.name === 'Customer')) {
			setError('You must be a customer to add games to the order.')
			return
		}

		handleAddToOrder()
	}

	async function handleAddToOrder() {
		if (!game) return

		const token = localStorage.getItem('authToken')

		if (!token) {
			setShowLoginModal(true)
			return
		}
		try {
			await orderService.addOrderItem(game.id, token)

			setShowSuccessModal(true)
		} catch (err) {
			setError('Failed to add the game to the order.')
		}
	}

	function handleSuccessClose() {
		setShowSuccessModal(false)

		navigate(0)
	}

	useEffect(() => {
		const loadGame = async () => {
			try {
				if (gameId && gameType) {
					const data = await gameService.fetchGameById(gameId, gameType)

					setGame(data)

					if (data.type === GameType.DlcGame) {
						const baseGame = await gameService.fetchGameById((data as DlcGame).baseGameId, GameType.FullGame)

						setBaseGame(baseGame as FullGame)
					}
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

		window.scrollTo(0, 0)
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
					<GamePrice amount={game.amount} currency={game.currency} onAddToOrder={handleAddToOrder} />
				</div>
			</Container>

			{game.type === GameType.FullGame && (game as FullGame).dlcGames.length !== 0 && (
				<Container className='content-container'>
					<h1 className='fs-2'>DLC:</h1>
					<div className='d-flex gap-3  align-content-around flex-wrap'>
						<FullGameDlcs game={game as FullGame} />
					</div>
				</Container>
			)}

			{game.type === GameType.DlcGame && baseGame && (
				<Container className='content-container'>
					<h1 className='fs-2'>Base Game:</h1>
					<div className='d-flex gap-3  align-content-around flex-wrap'>
						<GameCard key={baseGame.id} game={baseGame} onGameClick={handleOnGameClick} />
					</div>
				</Container>
			)}

			<LoginModal
				show={showLoginModal}
				onClose={() => setShowLoginModal(false)}
				onUserRolesChange={addToOrderAfterLogin}
			/>

			{showSuccessModal && (
				<SuccessModal show={showSuccessModal} onClose={handleSuccessClose} title='Game has been added to order' />
			)}
		</>
	)
}

export default GamePage
