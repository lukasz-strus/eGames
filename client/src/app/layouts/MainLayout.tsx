import React, { useEffect } from 'react'
import { Container, Nav, Navbar, Button, NavDropdown, Image } from 'react-bootstrap'
import { useState } from 'react'
import { useLocation } from 'react-router-dom'
import brandImg from '../../assets/egames-logo.png'
import { getEmail, loginUser } from '../core/services/api'
import LoginModal from '../features/auth/components/LoginModal'

const MainLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
	const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false)
	const [userName, setUserName] = useState<string | null>('Guest')
	const [showLoginModal, setShowLoginModal] = useState(false)
	const location = useLocation()

	useEffect(() => {
		const fetchProfile = async (token: string) => {
			const email = await getEmail(token)
			return email
		}

		const token = localStorage.getItem('authToken')
		if (token) {
			fetchProfile(token)
				.then(result => {
					setIsLoggedIn(true)
					setUserName(result)
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
		} catch (error) {
			throw new Error('Invalid credentials')
		}
	}

	const handleLogout = () => {
		localStorage.removeItem('authToken')
		setIsLoggedIn(false)
		setUserName(null)
	}

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
								<Button variant='outline-light' onClick={() => setShowLoginModal(true)}>
									Login
								</Button>
							)}
						</Nav>
					</Navbar.Collapse>
				</Container>
			</Navbar>
			<LoginModal show={showLoginModal} onClose={() => setShowLoginModal(false)} onLogin={handleLogin} />
			<Container>{children}</Container>
		</>
	)
}

export default MainLayout
