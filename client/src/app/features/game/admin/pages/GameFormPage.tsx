import React, { useState, useEffect } from 'react'
import { Container, Row, Col, Button, Form, Table } from 'react-bootstrap'
import { Game, FullGame, DlcGame, Subscription } from '../../../../core/contracts/Game'
import { useNavigate, useParams } from 'react-router-dom'
import FormField from '../../../../core/components/FormField'
import { GameService } from '../../services/GameService'

const gameService = GameService.getInstance()

const GameFormPage: React.FC = () => {
	const { gameId } = useParams<{ gameId?: string }>()
	const [game, setGame] = useState<Partial<Game | FullGame | Subscription>>({})
	const [dlcGames, setDlcGames] = useState<DlcGame[]>([])
	const [tmpDlcGame, setTmpDlcGame] = useState<Partial<DlcGame>>({})
	const [dlcGameButtonLabel, setDlcGameButtonLabel] = useState<string>('Add DLC Game')
	const [isLoading, setIsLoading] = useState<boolean>(false)
	const [isDlcOnAddMode, setIsDlcOnAddMode] = useState<boolean>(false)
	const navigate = useNavigate()

	useEffect(() => {
		const fetchGame = async () => {
			if (!gameId) return

			setIsLoading(true)
			try {
				const fetchedGame = await gameService.fetchGameById(gameId)
				setGame(fetchedGame)

				if (fetchedGame.type === 'FullGame' && 'dlcGames' in fetchedGame) {
					//setDlcGames(fetchedGame.dlcGames || [])
				}
			} catch (error) {
				console.error('Error fetching game:', error)
			} finally {
				setIsLoading(false)
			}
		}

		fetchGame()
	}, [gameId])

	const handleInputChange = (key: keyof Game, value: string | number) => {
		setGame(prev => ({ ...prev, [key]: value }))
	}

	const handleSubscriptionInputChange = (key: keyof Subscription, value: string | number) => {
		setGame(prev => ({ ...prev, [key]: value }))
	}

	const handleDlcInputChange = (key: keyof DlcGame, value: string | number) => {
		setTmpDlcGame(prev => ({ ...prev, [key]: value }))
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

	const handleRemoveDlcGame = (index: number) => {
		setDlcGames(prev => prev.filter((_, i) => i !== index))
	}

	const handleEditDlcGame = (index: number) => {
		const dlcGame = dlcGames[index]
		setTmpDlcGame(dlcGame)
		handleRemoveDlcGame(index)
		handleAddDlcGame()
	}

	const handleSaveGame = async () => {
		try {
			setIsLoading(true)
			const token = localStorage.getItem('authToken')

			if (!token) {
				throw new Error('User is not authenticated')
			}

			if (game.type === 'FullGame') {
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

			if (game.type === 'Subscription') {
				const subscriptionPayload: Subscription = game as Subscription

				if (gameId) {
					await gameService.updateSubscription(gameId, subscriptionPayload, token)
				} else {
					await gameService.createSubscription(subscriptionPayload, token)
				}
			}

			if (game.type === 'DlcGame') {
				const dlcGamePayload: DlcGame = game as DlcGame

				if (gameId) {
					await gameService.updateDlcGame(gameId, dlcGamePayload, token)
				} else {
					throw new Error('DLC Game can not be created without base game id')
				}
			}

			navigate('/games-managment')
		} catch (error) {
			console.error('Error saving game:', error)
		} finally {
			setIsLoading(false)
		}
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
								onChange={e => handleInputChange('name', e.target.value)}
							/>
						</Col>
					</Row>
					<Row>
						{game.type === 'DlcGame' && (
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
						{game.type !== 'DlcGame' && (
							<Col md={6}>
								<FormField
									label='Game Type'
									type='select'
									floatingLabel={true}
									value={game.type || ''}
									options={[
										{ value: '', label: '' },
										{ value: 'FullGame', label: 'Full Game' },
										{ value: 'Subscription', label: 'Subscription' },
									]}
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
									{ value: 'USD', label: 'USD' },
									{ value: 'EUR', label: 'EUR' },
									{ value: 'PLN', label: 'PLN' },
								]}
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
								onChange={e => handleInputChange('publisher', e.target.value)}
							/>
						</Col>
						<Col md={3}>
							<FormField
								label='Release Date'
								type='date'
								floatingLabel={true}
								value={game.releaseDate || ''}
								onChange={e => handleInputChange('releaseDate', e.target.value)}
							/>
						</Col>
						<Col md={3}>
							<FormField
								label='File Size'
								type='number'
								floatingLabel={true}
								value={game.fileSize || ''}
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
