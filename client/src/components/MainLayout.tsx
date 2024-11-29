import React, { useEffect } from 'react'
import { Container, Nav, Navbar, Button, NavDropdown, Image, Modal, Alert } from 'react-bootstrap'
import { useState } from 'react'
import { useLocation } from 'react-router-dom'
import { Form } from 'react-bootstrap'
import brandImg from '../assets/egames-logo.png'
import { getEmail, loginUser } from '../services/api'

const MainLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
	const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false)
	const [userName, setUserName] = useState<string | null>('Guest')
	const [showLoginModal, setShowLoginModal] = useState(false)
	const [username, setUsername] = useState('')
	const [password, setPassword] = useState('')
	const [loginError, setLoginError] = useState<string | null>(null)
	const location = useLocation()

	useEffect(() => {
		const profile = async (token: string) => {
			const email = await getEmail(token)

			return email
		}

		const token = localStorage.getItem('authToken')
		if (token) {
			profile(token)
				.then(reult => {
					setIsLoggedIn(true)
					setUserName(reult)
				})
				.catch(error => {
					console.error('Failed to get user profile:', error)
					localStorage.removeItem('authToken')
				})
		}
	}, [])

	const getNavLinkClass = (path: string): string => {
		return location.pathname === path ? 'fw-bold' : ''
	}

	const handleLogin = async (email: string, password: string) => {
		try {
			const token = await loginUser(email, password)
			localStorage.setItem('authToken', token)

			setIsLoggedIn(true)
			setUserName(email)
			setLoginError(null) // Wyczyszczenie błędu po udanym logowaniu
			handleCloseLoginModal()
		} catch (error) {
			console.error('Login failed:', error)
			setLoginError('Invalid email or password. Please try again.') // Ustawienie błędu
		}
	}

	const handleLogout = () => {
		localStorage.removeItem('authToken')
		setIsLoggedIn(false)
		setUserName(null)
	}

	const handleShowLoginModal = () => setShowLoginModal(true)
	const handleCloseLoginModal = () => setShowLoginModal(false)

	return (
		<>
			<Navbar expand='lg' className='fs-3 bg-dark sticky-top'>
				<Container>
					<Navbar.Brand href='/'>
						<Image src={brandImg} alt='eGames' width='90px' />
					</Navbar.Brand>
					<Navbar.Toggle aria-controls='basic-navbar-nav' />
					<Navbar.Collapse id='basic-navbar-nav'>
						<Nav className='me-auto'>
							<Nav.Link href='/' className={getNavLinkClass('/')}>
								Store
							</Nav.Link>
							<Nav.Link href='/library' className={getNavLinkClass('/library')}>
								Library
							</Nav.Link>
						</Nav>
						<Nav>
							{isLoggedIn ? (
								<NavDropdown title={userName} id='user-dropdown'>
									<NavDropdown.Item href='/profile'>Profile</NavDropdown.Item>
									<NavDropdown.Item onClick={handleLogout}>Logout</NavDropdown.Item>
								</NavDropdown>
							) : (
								<Button variant='outline-light' onClick={handleShowLoginModal}>
									Login
								</Button>
							)}
						</Nav>
					</Navbar.Collapse>
				</Container>
			</Navbar>
			<Modal show={showLoginModal} onHide={handleCloseLoginModal}>
				<Modal.Header closeButton>
					<Modal.Title>Login</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					{loginError && (
						<Alert variant='danger' className='mb-3'>
							{loginError}
						</Alert>
					)}
					<Form>
						<Form.Group className='mb-3' controlId='formUsername'>
							<Form.Label>Email</Form.Label>
							<Form.Control
								type='email'
								placeholder='Enter email'
								value={username}
								isInvalid={!!loginError} // Dodanie walidacji Bootstrap
								onChange={e => {
									setUsername(e.target.value)
									setLoginError(null) // Czyszczenie błędu przy zmianie danych
								}}
							/>
						</Form.Group>
						<Form.Group className='mb-3' controlId='formPassword'>
							<Form.Label>Password</Form.Label>
							<Form.Control
								type='password'
								placeholder='Enter password'
								value={password}
								isInvalid={!!loginError} // Dodanie walidacji Bootstrap
								onChange={e => {
									setPassword(e.target.value)
									setLoginError(null) // Czyszczenie błędu przy zmianie danych
								}}
							/>
							<Form.Control.Feedback type='invalid'>Invalid email or password.</Form.Control.Feedback>
						</Form.Group>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					<Button variant='secondary' onClick={handleCloseLoginModal}>
						Close
					</Button>
					<Button
						variant='primary'
						onClick={async () => {
							await handleLogin(username, password)
						}}>
						Login
					</Button>
				</Modal.Footer>
			</Modal>
			<Container>{children}</Container>
		</>
	)
}

export default MainLayout
