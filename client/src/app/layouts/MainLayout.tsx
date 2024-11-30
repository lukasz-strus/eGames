import React, { useEffect } from 'react'
import { Container, Navbar } from 'react-bootstrap'
import { useState } from 'react'
import LoginModal from '../features/auth/components/LoginModal.tsx'
import Brand from './components/Brand.tsx'
import NavigationLinks from './components/NavigationLinks.tsx'
import UserMenu from './components/UserMenu.tsx'
import { AuthService } from '../features/auth/services/AuthService.ts'
import { useAuth } from '../core/context/AuthContext.tsx'

const authService = AuthService.getInstance()

const MainLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
	const [showLoginModal, setShowLoginModal] = useState(false)
	const { setIsLoggedIn, setUserName } = useAuth()

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

	return (
		<>
			<Navbar expand='lg' className='fs-4 bg-dark sticky-top'>
				<Container>
					<Brand />
					<Navbar.Toggle aria-controls='basic-navbar-nav' />
					<Navbar.Collapse id='basic-navbar-nav'>
						<NavigationLinks />
						<UserMenu />
					</Navbar.Collapse>
				</Container>
			</Navbar>
			{showLoginModal && <LoginModal show={showLoginModal} onClose={() => setShowLoginModal(false)} />}
			<Container>{children}</Container>
		</>
	)
}

export default MainLayout
