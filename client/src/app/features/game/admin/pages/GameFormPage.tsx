import React, { useState, useEffect } from 'react'
import { Container, Row, Col, Button, Form, Table } from 'react-bootstrap'
import { Game, FullGame, DlcGame, Subscription } from '../../../../core/contracts/Game'
import { useNavigate, useParams } from 'react-router-dom'
import FormField from '../../../../core/components/FormField'
import { GameService } from '../../services/GameService'
import { GameType } from '../../../../core/enums/GameType'

const gameService = GameService.getInstance()

const GameFormPage: React.FC = () => {
	const { gameType, gameId } = useParams<{ gameType: GameType; gameId?: string }>()
	const [game, setGame] = useState<Partial<Game | FullGame | Subscription>>({})
	const [dlcGames, setDlcGames] = useState<DlcGame[]>([])
	const [tmpDlcGame, setTmpDlcGame] = useState<Partial<DlcGame>>({})
	const [dlcGameButtonLabel, setDlcGameButtonLabel] = useState<string>('Add DLC Game')
	const [isLoading, setIsLoading] = useState<boolean>(false)
	const [isDlcOnAddMode, setIsDlcOnAddMode] = useState<boolean>(false)
	const [errors, setErrors] = useState<Record<string, string | null>>({})
	const navigate = useNavigate()

	useEffect(() => {
		fetchGame()
	}, [gameId])

	const fetchGame = async () => {
		if (!gameId) return

		setIsLoading(true)
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
			console.error('Error fetching game:', error)
		} finally {
			setIsLoading(false)
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
		} else if (game.description.length > 500) {
			newErrors.description = 'Description cannot exceed 500 characters.'
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

	const handleInputChange = (key: keyof Game, value: string | number) => {
		if (key === 'releaseDate') {
			setGame(prev => ({ ...prev, [key]: formatDate(value as string) }))
		} else {
			setGame(prev => ({ ...prev, [key]: value }))
		}

		setErrors(prev => ({ ...prev, [key]: null }))
	}

	const handleSubscriptionInputChange = (key: keyof Subscription, value: string | number) => {
		setGame(prev => ({ ...prev, [key]: value }))
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
		} else {
			setIsDlcOnAddMode(true)
			setDlcGameButtonLabel('Cancel')
		}
	}

	const handleSaveNewDlcGame = async () => {
		const newDlc = { ...tmpDlcGame, baseGameId: gameId || '' }
		setDlcGames(prev => [...prev, newDlc as DlcGame])

		setTmpDlcGame({})
		handleAddDlcGame()
	}

	const handleRemoveDlcGame = async (index: number) => {
		const token = localStorage.getItem('authToken')

		if (!token) {
			throw new Error('User is not authenticated')
		} else {
			await gameService.deleteGame(dlcGames[index].id, token)
			await fetchGame()
		}
	}

	const handleEditDlcGame = (index: number) => {
		const dlcGame = dlcGames[index]
		setTmpDlcGame(dlcGame)
		handleRemoveDlcGame(index)
		handleAddDlcGame()
	}

	const handleSaveGame = async () => {
		if (!validate()) {
			return
		}
		try {
			setIsLoading(true)
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

			//navigate('/games-managment')
		} catch (error) {
			console.error('Error saving game:', error)
		} finally {
			setIsLoading(false)
		}
	}

	const formatDate = (date: string): string => {
		const d = new Date(date)
		const year = d.getFullYear()
		const month = String(d.getMonth() + 1).padStart(2, '0') // Dodaj 1, bo miesiące są zero-indexed
		const day = String(d.getDate()).padStart(2, '0')
		return `${year}-${month}-${day}`
	}

	if (isLoading) return <div>Loading...</div>

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
					<Row>
						<Col>
							<FormField
								label='Game Title'
								type='text'
								floatingLabel={true}
								value={game.name || ''}
								isInvalid={!!errors.name}
								feedback={errors.name}
								onChange={e => handleInputChange('name', e.target.value)}
							/>
						</Col>
					</Row>
					<Row>
						{game.type === GameType.DlcGame && (
							<Col md={6}>
								<FormField
									label='Game Type'
									type='text'
									floatingLabel={true}
									value={game.type || ''}
									onChange={() => {}}
								/>
							</Col>
						)}
						{game.type !== GameType.DlcGame && (
							<Col md={6}>
								<FormField
									label='Game Type'
									type='select'
									floatingLabel={true}
									value={game.type || ''}
									options={[
										{ value: '', label: '' },
										{ value: GameType.FullGame, label: 'Full Game' },
										{ value: GameType.Subscription, label: 'Subscription' },
									]}
									isInvalid={!!errors.type}
									feedback={errors.type}
									onChange={e => handleInputChange('type', e.target.value)}
								/>
							</Col>
						)}

						<Col md={3}>
							<FormField
								label='Price'
								type='number'
								floatingLabel={true}
								value={game.amount || ''}
								isInvalid={!!errors.amount}
								feedback={errors.amount}
								onChange={e => handleInputChange('amount', parseFloat(e.target.value))}
							/>
						</Col>
						<Col md={3}>
							<FormField
								label='Currency'
								type='select'
								floatingLabel={true}
								value={game.currency || ''}
								options={[
									{ value: '', label: '' },
									{ value: 'USD', label: 'USD' },
									{ value: 'EUR', label: 'EUR' },
									{ value: 'PLN', label: 'PLN' },
								]}
								isInvalid={!!errors.currency}
								feedback={errors.currency}
								onChange={e => handleInputChange('currency', e.target.value)}
							/>
						</Col>
					</Row>
					<Row>
						<Col md={6}>
							<FormField
								label='Publisher'
								type='text'
								floatingLabel={true}
								value={game.publisher || ''}
								isInvalid={!!errors.publisher}
								feedback={errors.publisher}
								onChange={e => handleInputChange('publisher', e.target.value)}
							/>
						</Col>
						<Col md={3}>
							<FormField
								label='Release Date'
								type='date'
								floatingLabel={true}
								value={game.releaseDate ? formatDate(game.releaseDate) : ''}
								isInvalid={!!errors.releaseDate}
								feedback={errors.releaseDate}
								onChange={e => handleInputChange('releaseDate', e.target.value)}
							/>
						</Col>
						<Col md={3}>
							<FormField
								label='File Size'
								type='number'
								floatingLabel={true}
								value={game.fileSize || ''}
								isInvalid={!!errors.fileSize}
								feedback={errors.fileSize}
								onChange={e => handleInputChange('fileSize', parseFloat(e.target.value))}
							/>
						</Col>
					</Row>
					<Row>
						<Col>
							<FormField
								label='Description'
								type='text'
								as='textarea'
								floatingLabel={true}
								value={game.description || ''}
								isInvalid={!!errors.description}
								feedback={errors.description}
								onChange={e => handleInputChange('description', e.target.value)}
							/>
						</Col>
					</Row>
					<Row>
						<Col>
							<FormField
								label='Download Link'
								type='text'
								as='textarea'
								floatingLabel={true}
								value={game.downloadLink || ''}
								isInvalid={!!errors.downloadLink}
								feedback={errors.downloadLink}
								onChange={e => handleInputChange('downloadLink', e.target.value)}
							/>
						</Col>
					</Row>
					<Row>
						<Col>
							<FormField
								label='Image Url'
								type='text'
								as='textarea'
								floatingLabel={true}
								value={game.imageUrl || ''}
								isInvalid={!!errors.imageUrl}
								feedback={errors.imageUrl}
								onChange={e => handleInputChange('imageUrl', e.target.value)}
							/>
						</Col>
					</Row>
					{game.type === 'Subscription' && (
						<Row>
							<Col md={3}>
								<FormField
									label='Subscription Period In Days'
									type='number'
									floatingLabel={true}
									value={(game as Subscription).subscriptionPeriodInDays || ''}
									onChange={e => handleSubscriptionInputChange('subscriptionPeriodInDays', parseFloat(e.target.value))}
								/>
							</Col>
						</Row>
					)}
					{game.type === 'FullGame' && (
						<Container>
							<h4>DLC Games</h4>
							<Table bordered>
								<thead>
									<tr>
										<th>Name</th>
										<th>Price</th>
										<th>Action</th>
									</tr>
								</thead>
								<tbody>
									{dlcGames.map((dlc, index) => (
										<tr key={dlc.id}>
											<td>
												<Form.Control
													type='text'
													placeholder='DLC Name'
													value={dlc.name}
													onChange={() => {}}
													readOnly
												/>
											</td>
											<td>
												<Form.Control
													type='number'
													placeholder='DLC Price'
													value={dlc.amount}
													onChange={() => {}}
													readOnly
												/>
											</td>
											<td className='text-center align-middle'>
												<Button variant='warning' className='me-3' onClick={() => handleEditDlcGame(index)}>
													Edit
												</Button>
												<Button variant='danger' onClick={() => handleRemoveDlcGame(index)}>
													Remove
												</Button>
											</td>
										</tr>
									))}
								</tbody>
							</Table>
							<Button onClick={handleAddDlcGame} variant='primary' className='mb-3'>
								{dlcGameButtonLabel}
							</Button>
							{isDlcOnAddMode && (
								<>
									<Row>
										<Col>
											<FormField
												label='Title'
												type='text'
												floatingLabel={true}
												value={tmpDlcGame.name || ''}
												onChange={e => handleDlcInputChange('name', e.target.value)}
											/>
										</Col>
									</Row>
									<Row>
										<Col md={3}>
											<FormField
												label='Price'
												type='number'
												floatingLabel={true}
												value={tmpDlcGame.amount || ''}
												onChange={e => handleDlcInputChange('amount', parseFloat(e.target.value))}
											/>
										</Col>
										<Col md={3}>
											<FormField
												label='Currency'
												type='select'
												floatingLabel={true}
												value={tmpDlcGame.currency || ''}
												options={[
													{ value: 'USD', label: 'USD' },
													{ value: 'EUR', label: 'EUR' },
													{ value: 'PLN', label: 'PLN' },
												]}
												onChange={e => handleDlcInputChange('currency', e.target.value)}
											/>
										</Col>
										<Col md={3}>
											<FormField
												label='Release Date'
												type='date'
												floatingLabel={true}
												value={tmpDlcGame.releaseDate || ''}
												onChange={e => handleDlcInputChange('releaseDate', e.target.value)}
											/>
										</Col>
										<Col md={3}>
											<FormField
												label='File Size'
												type='number'
												floatingLabel={true}
												value={tmpDlcGame.fileSize || ''}
												onChange={e => handleDlcInputChange('fileSize', parseFloat(e.target.value))}
											/>
										</Col>
									</Row>
									<Row>
										<Col>
											<FormField
												label='Description'
												type='text'
												as='textarea'
												floatingLabel={true}
												value={tmpDlcGame.description || ''}
												onChange={e => handleDlcInputChange('description', e.target.value)}
											/>
										</Col>
									</Row>
									<Row>
										<Col>
											<FormField
												label='Download Link'
												type='text'
												as='textarea'
												floatingLabel={true}
												value={tmpDlcGame.downloadLink || ''}
												onChange={e => handleDlcInputChange('downloadLink', e.target.value)}
											/>
										</Col>
									</Row>
									<Row>
										<Col>
											<FormField
												label='Image Url'
												type='text'
												as='textarea'
												floatingLabel={true}
												value={tmpDlcGame.imageUrl || ''}
												onChange={e => handleDlcInputChange('imageUrl', e.target.value)}
											/>
										</Col>
									</Row>

									<Button onClick={handleSaveNewDlcGame} variant='success' className='mt-3'>
										Save DLC Game
									</Button>
								</>
							)}
						</Container>
					)}
				</Form>
			</Container>
		</>
	)
}

export default GameFormPage
