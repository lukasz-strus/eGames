import React, { useEffect } from 'react'
import { Container, Navbar } from 'react-bootstrap'
import { useState } from 'react'
import LoginModal from '../features/auth/components/LoginModal.tsx'
import Brand from './components/Brand.tsx'
import NavigationLinks from './components/NavigationLinks.tsx'
import UserMenu from './components/UserMenu.tsx'
import { AuthService } from '../features/auth/services/AuthService.ts'

const authService = AuthService.getInstance()

const MainLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
	const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false)
	const [userName, setUserName] = useState<string | null>('Guest')
	const [showLoginModal, setShowLoginModal] = useState(false)

	useEffect(() => {
		const fetchProfile = async (token: string) => {
			const email = await authService.getEmail(token)
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

	const handleLogin = async (email: string, password: string) => {
		try {
			const token = await authService.loginUser(email, password)
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
			<Navbar expand='lg' className='fs-4 bg-dark sticky-top'>
				<Container>
					<Brand />
					<Navbar.Toggle aria-controls='basic-navbar-nav' />
					<Navbar.Collapse id='basic-navbar-nav'>
						<NavigationLinks />
						<UserMenu isLoggedIn={isLoggedIn} userName={userName} onLogin={handleLogin} onLogout={handleLogout} />
					</Navbar.Collapse>
				</Container>
			</Navbar>
			<LoginModal show={showLoginModal} onClose={() => setShowLoginModal(false)} onLogin={handleLogin} />
			<Container>{children}</Container>
		</>
	)
}

export default MainLayout
