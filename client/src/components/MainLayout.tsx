import React from 'react'
import { Container, Nav, Navbar, Button, NavDropdown, Image } from 'react-bootstrap'
import { useState } from 'react'
import { useLocation } from 'react-router-dom'
import brandImg from '../assets/egames-logo.png'

const MainLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
	const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false)
	const [userName, setUserName] = useState<string | null>('Guest')
	const location = useLocation()

	const getNavLinkClass = (path: string): string => {
		return location.pathname === path ? 'fw-bold' : ''
	}

	const handleLogin = () => {
		// Logika logowania
		setIsLoggedIn(true)
		setUserName('JohnDoe') // Tymczasowa nazwa użytkownika
	}

	const handleLogout = () => {
		// Logika wylogowania
		setIsLoggedIn(false)
		setUserName(null)
	}

	return (
		<>
			<Navbar expand='lg' className='mb-6 fs-3'>
				<Container>
					<Navbar.Brand href='/'>
						<Image src={brandImg} alt='eGames' width='200' />
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
								<Button variant='outline-light' onClick={handleLogin}>
									Login
								</Button>
							)}
						</Nav>
					</Navbar.Collapse>
				</Container>
			</Navbar>
			<Container>{children}</Container>
		</>
	)
}

export default MainLayout
