import React, { useState, useEffect } from 'react'
import { Container, Row, Col, Button, Form, Spinner, Alert } from 'react-bootstrap'
import { Game, FullGame, DlcGame, Subscription } from '../../../../core/contracts/Game'
import { useNavigate, useParams } from 'react-router-dom'
import { GameService } from '../../services/GameService'
import { GameType } from '../../../../core/enums/GameType'
import GameDetailsForm from '../components/GameDetailsForm'
import DlcGameList from '../components/DlcGameList'
import DlcGameForm from '../components/DlcGameForm'

const gameService = GameService.getInstance()

const GameFormPage: React.FC = () => {
	const { gameType, gameId } = useParams<{ gameType: GameType; gameId?: string }>()
	const [game, setGame] = useState<Partial<Game | FullGame | Subscription>>({})
	const [dlcGames, setDlcGames] = useState<DlcGame[]>([])
	const [tmpDlcGame, setTmpDlcGame] = useState<Partial<DlcGame>>({})
	const [dlcGameButtonLabel, setDlcGameButtonLabel] = useState<string>('Add DLC Game')
	const [loading, setLoading] = useState<boolean>(false)
	const [error, setError] = useState<string | null>(null)
	const [isDlcOnAddMode, setIsDlcOnAddMode] = useState<boolean>(false)
	const [errors, setErrors] = useState<Record<string, string | null>>({})
	const navigate = useNavigate()

	useEffect(() => {
		fetchGame()
	}, [gameId])

	const fetchGame = async () => {
		if (!gameId) return

		setLoading(true)
		try {
			switch (gameType) {
				case GameType.FullGame:
					const fetchedFullGame = await gameService.fetchGameById(gameId, GameType.FullGame)
					setGame(fetchedFullGame)
					setDlcGames((fetchedFullGame as FullGame).dlcGames || [])
					break
				case GameType.Subscription:
					const fetchedSubscriptionGame = await gameService.fetchGameById(gameId, GameType.Subscription)
					setGame(fetchedSubscriptionGame)
					break
				case GameType.DlcGame:
					const fetchedDlcGame = await gameService.fetchGameById(gameId, GameType.DlcGame)
					setGame(fetchedDlcGame)
					break
				default:
					throw new Error('Invalid game type selected.')
			}
		} catch (error) {
			setError('Failed to fetch game information.')
		} finally {
			setLoading(false)
		}
	}

	const validate = (): boolean => {
		const newErrors: Record<string, string | null> = {}

		if (!game.name || game.name.trim() === '') {
			newErrors.name = 'Game title is required.'
		} else if (game.name.length > 100) {
			newErrors.name = 'Game title cannot exceed 100 characters.'
		}

		if (!game.type || !['FullGame', 'Subscription', 'DlcGame'].includes(game.type)) {
			newErrors.type = 'Game type must be defined.'
		}

		if (!game.description || game.description.trim() === '') {
			newErrors.description = 'Description is required.'
		}

		if (!game.amount || game.amount <= 0) {
			newErrors.amount = 'Price must be greater than 0.'
		}

		if (!game.currency || !['USD', 'EUR', 'PLN'].includes(game.currency)) {
			newErrors.currency = 'Currency must be one of USD, EUR, or PLN.'
		}

		if (!game.releaseDate) {
			newErrors.releaseDate = 'Release date is required.'
		}

		if (!game.publisher || game.publisher.trim() === '') {
			newErrors.publisher = 'Publisher is required.'
		} else if (game.publisher.length > 100) {
			newErrors.publisher = 'Publisher cannot exceed 100 characters.'
		}

		if (!game.downloadLink || game.downloadLink.trim() === '') {
			newErrors.downloadLink = 'Download link is required.'
		} else if (game.downloadLink.length > 200) {
			newErrors.downloadLink = 'Download link cannot exceed 200 characters.'
		}

		if (!game.fileSize || game.fileSize <= 0) {
			newErrors.fileSize = 'File size must be greater than 0.'
		}

		if (!game.imageUrl || game.imageUrl.trim() === '') {
			newErrors.imageUrl = 'Image URL is required.'
		}

		setErrors(newErrors)

		return Object.values(newErrors).every(error => error === null)
	}

	const validateDlc = (): boolean => {
		const newErrors: Record<string, string | null> = {}

		if (!tmpDlcGame.name || tmpDlcGame.name.trim() === '') {
			newErrors.dlcName = 'Game title is required.'
		} else if (tmpDlcGame.name.length > 100) {
			newErrors.dlcName = 'Game title cannot exceed 100 characters.'
		}

		if (!tmpDlcGame.description || tmpDlcGame.description.trim() === '') {
			newErrors.dlcDescription = 'Description is required.'
		}

		if (!tmpDlcGame.amount || tmpDlcGame.amount <= 0) {
			newErrors.dlcAmount = 'Price must be greater than 0.'
		}

		if (!tmpDlcGame.currency || !['USD', 'EUR', 'PLN'].includes(tmpDlcGame.currency)) {
			newErrors.dlcCurrency = 'Currency must be one of USD, EUR, or PLN.'
		}

		if (!tmpDlcGame.releaseDate) {
			newErrors.dlcReleaseDate = 'Release date is required.'
		}

		if (!tmpDlcGame.publisher || tmpDlcGame.publisher.trim() === '') {
			newErrors.dlcPublisher = 'Publisher is required.'
		} else if (tmpDlcGame.publisher.length > 100) {
			newErrors.dlcPublisher = 'Publisher cannot exceed 100 characters.'
		}

		if (!tmpDlcGame.downloadLink || tmpDlcGame.downloadLink.trim() === '') {
			newErrors.dlcDownloadLink = 'Download link is required.'
		} else if (tmpDlcGame.downloadLink.length > 200) {
			newErrors.dlcDownloadLink = 'Download link cannot exceed 200 characters.'
		}

		if (!tmpDlcGame.fileSize || tmpDlcGame.fileSize <= 0) {
			newErrors.dlcFileSize = 'File size must be greater than 0.'
		}

		if (!tmpDlcGame.imageUrl || tmpDlcGame.imageUrl.trim() === '') {
			newErrors.dlcImageUrl = 'Image URL is required.'
		}

		setErrors(newErrors)

		return Object.values(newErrors).every(error => error === null)
	}

	const handleInputChange = (key: keyof Game | keyof Subscription, value: string | number) => {
		if (key === 'releaseDate') {
			setGame(prev => ({ ...prev, [key]: formatDate(value as string) }))
		} else {
			setGame(prev => ({ ...prev, [key]: value }))
		}

		setErrors(prev => ({ ...prev, [key]: null }))
	}

	const handleDlcInputChange = (key: keyof DlcGame, value: string | number) => {
		if (key === 'releaseDate') {
			setTmpDlcGame(prev => ({ ...prev, [key]: formatDate(value as string) }))
		} else {
			setTmpDlcGame(prev => ({ ...prev, [key]: value }))
		}
		setErrors(prev => ({ ...prev, [key]: null }))
	}

	const handleAddDlcGame = () => {
		if (isDlcOnAddMode) {
			setIsDlcOnAddMode(false)
			setDlcGameButtonLabel('Add DLC Game')
			setErrors({
				...errors,
				dlcName: null,
				dlcDescription: null,
				dlcAmount: null,
				dlcCurrency: null,
				dlcReleaseDate: null,
				dlcPublisher: null,
				dlcDownloadLink: null,
				dlcFileSize: null,
				dlcImageUrl: null,
			})
		} else {
			setTmpDlcGame({ id: Math.random().toString(36).substring(7), baseGameId: gameId || '' })
			setIsDlcOnAddMode(true)
			setDlcGameButtonLabel('Cancel')
		}
	}

	const handleEditDlcGame = (index: number) => {
		const dlcGame = dlcGames[index]
		setTmpDlcGame(dlcGame)
		setIsDlcOnAddMode(true)
		setDlcGameButtonLabel('Cancel')
	}

	const handleSaveNewDlcGame = async () => {
		if (!validateDlc()) {
			return
		}

		if (!gameId) {
			const newDlc = { ...tmpDlcGame, baseGameId: gameId || '' }
			setDlcGames(prev => [...prev, newDlc as DlcGame])
		} else {
			const token = localStorage.getItem('authToken')

			if (!token) {
				throw new Error('User is not authenticated')
			}

			if (gameId) {
				await gameService.createDlcGame(tmpDlcGame as DlcGame, gameId, token)
				await fetchGame()
			}
		}

		setTmpDlcGame({})
		handleAddDlcGame()
	}

	const handleRemoveDlcGame = async (index: number) => {
		if (!gameId) {
			const newDlcGames = dlcGames.filter((_, i) => i !== index)
			setDlcGames(newDlcGames)
			return
		}
		const token = localStorage.getItem('authToken')

		if (!token) {
			throw new Error('User is not authenticated')
		} else {
			await gameService.deleteGame(dlcGames[index].id, token)
			await fetchGame()
		}
	}

	const handleSaveGame = async () => {
		if (!validate()) {
			return
		}
		try {
			setLoading(true)
			const token = localStorage.getItem('authToken')

			if (!token) {
				throw new Error('User is not authenticated')
			}

			if (game.type === GameType.FullGame) {
				const fullGamePayload: FullGame = {
					...(game as FullGame),
					dlcGames,
				}

				if (gameId) {
					await gameService.updateFullGame(gameId, fullGamePayload, token)
				} else {
					await gameService.createFullGame(fullGamePayload, token)
				}
			}

			if (game.type === GameType.Subscription) {
				const subscriptionPayload: Subscription = game as Subscription

				if (gameId) {
					await gameService.updateSubscription(gameId, subscriptionPayload, token)
				} else {
					await gameService.createSubscription(subscriptionPayload, token)
				}
			}

			if (game.type === GameType.DlcGame) {
				const dlcGamePayload: DlcGame = game as DlcGame

				if (gameId) {
					await gameService.updateDlcGame(gameId, dlcGamePayload, token)
				} else {
					throw new Error('DLC Game can not be created without base game id')
				}
			}

			navigate('/games-managment')
		} catch (error) {
			setError('Failed to save game.')
		} finally {
			setLoading(false)
		}
	}

	const formatDate = (date: string): string => {
		const d = new Date(date)
		const year = d.getFullYear()
		const month = String(d.getMonth() + 1).padStart(2, '0')
		const day = String(d.getDate()).padStart(2, '0')
		return `${year}-${month}-${day}`
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
			<Container>
				<Alert variant='danger'>{error}</Alert>
			</Container>
		)
	}

	return (
		<>
			<Container className='section-container'>
				<Row className='d-flex justify-content-center align-items-center mb-4'>
					<Col md={10}>
						<h2>{gameId ? 'Edit Game' : 'Add New Game'}</h2>
					</Col>
					<Col md={2}>
						<Button onClick={handleSaveGame} variant='success'>
							Save Game
						</Button>
					</Col>
				</Row>

				<Form>
					<GameDetailsForm game={game} errors={errors} onChange={handleInputChange} />

					{game.type === 'FullGame' && (
						<>
							<h4>DLC Games</h4>
							<DlcGameList dlcGames={dlcGames} onEdit={handleEditDlcGame} onDelete={handleRemoveDlcGame} />
							<Button onClick={handleAddDlcGame} variant='primary' className='mb-3'>
								{dlcGameButtonLabel}
							</Button>
							{isDlcOnAddMode && (
								<DlcGameForm
									dlcGame={tmpDlcGame}
									errors={errors}
									onChange={handleDlcInputChange}
									onSave={handleSaveNewDlcGame}
								/>
							)}
						</>
					)}
				</Form>
			</Container>
		</>
	)
}

export default GameFormPage
