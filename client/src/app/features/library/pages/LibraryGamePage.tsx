import { useEffect, useState } from 'react'
import { FullLibraryGame } from '../../../core/contracts/LibraryGame'
import { useNavigate, useParams } from 'react-router-dom'
import { Alert, Button, Card, Container, Modal, Spinner } from 'react-bootstrap'
import GameImage from '../../game/store/components/GameImage'
import GameInfo from '../../game/store/components/GameInfo'
import { LibraryService } from '../services/LibraryService'

const libraryService = LibraryService.getInstance()

const LibraryGamePage: React.FC = () => {
	const { libraryGameId } = useParams<{ libraryGameId: string }>()
	const [fullLibraryGame, setFullLibraryGame] = useState<FullLibraryGame | null>(null)
	const [loading, setLoading] = useState<boolean>(true)
	const [error, setError] = useState<string | null>(null)
	const [showModal, setShowModal] = useState<boolean>(false)
	const navigate = useNavigate()

	useEffect(() => {
		const loadLibraryGame = async (token: string) => {
			try {
				if (libraryGameId) {
					const data = await libraryService.getLibraryGameById(libraryGameId, token)

					setFullLibraryGame(data)
				} else {
					setError('Invalid game type or ID.')
				}
			} catch (err) {
				setError('Failed to load the game.')
			} finally {
				setLoading(false)
			}
		}

		const token = localStorage.getItem('authToken')

		if (token) {
			loadLibraryGame(token)

			window.scrollTo(0, 0)
		} else {
			navigate('/login')
		}
	}, [libraryGameId])

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

	if (!fullLibraryGame) return null

	const handleShowModal = () => setShowModal(true)
	const handleCloseModal = () => setShowModal(false)

	return (
		<>
			<div className='bcg-image' />
			<Container className='my-5 game-container'>
				<GameImage src={fullLibraryGame.gameResponse.imageUrl} alt={fullLibraryGame.gameResponse.name} />
				<div className='game-info'>
					<GameInfo game={fullLibraryGame.gameResponse} />
					<Card>
						<div className='m-4 text-left'>
							<p>
								<strong>Purchase Date:</strong> {new Date(fullLibraryGame.purchaseDate).toLocaleDateString()}
							</p>
							<div className='d-flex gap-3'>
								<Button
									variant='primary'
									onClick={() => window.open(fullLibraryGame.gameResponse.downloadLink, '_blank')}>
									Download Game
								</Button>
								<Button variant='secondary' onClick={handleShowModal}>
									View Licence Key
								</Button>
							</div>
						</div>
					</Card>
				</div>
			</Container>

			<Modal show={showModal} onHide={handleCloseModal} centered>
				<Modal.Header closeButton>
					<Modal.Title>Licence Key</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<p className='text-center fs-4'>{fullLibraryGame.licenceKey}</p>
				</Modal.Body>
				<Modal.Footer>
					<Button variant='secondary' onClick={handleCloseModal}>
						Close
					</Button>
				</Modal.Footer>
			</Modal>
		</>
	)
}

export default LibraryGamePage
