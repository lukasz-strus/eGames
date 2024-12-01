import React, { useEffect } from 'react'
import { Container, Nav, Navbar, Spinner } from 'react-bootstrap'
import { useState } from 'react'
import Brand from './components/Brand.tsx'
import UserMenu from './components/UserMenu.tsx'
import { AuthService } from '../features/auth/services/AuthService.ts'
import { useAuth } from '../core/context/AuthContext.tsx'
import { useLocation } from 'react-router-dom'
import { UserRole } from '../core/contracts/User.ts'

const authService = AuthService.getInstance()

const MainLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
	const { setIsLoggedIn, setUserName } = useAuth()
	const [isLoading, setIsLoading] = useState<boolean>(true)
	const [isLibraryVisible, setLibraryVisible] = useState<boolean>(false)
	const location = useLocation()

	const getNavLinkClass = (path: string): string => {
		return location.pathname === path ? 'fw-bold' : ''
	}

	const setMenuVisibility = (roles: UserRole[]) => {
		setLibraryVisible(false)

		console.log(roles)

		if (roles.some(role => role.name === 'Customer')) setLibraryVisible(true)
	}

	useEffect(() => {
		const fetchProfile = async (token: string) => {
			try {
				const user = await authService.getProfile(token)
				if (user) {
					setIsLoggedIn(true)
					setUserName(user.userName)
					setMenuVisibility(user.userRoles.items)
				} else {
					localStorage.removeItem('authToken')
				}
			} catch (error) {
				console.error('Failed to get user profile:', error)
				localStorage.removeItem('authToken')
			} finally {
				setIsLoading(false)
			}
		}

		const token = localStorage.getItem('authToken')

		if (token) {
			fetchProfile(token)
		} else {
			setIsLoading(false)
		}
	}, [])

	if (isLoading) {
		return (
			<div className='d-flex justify-content-center align-items-center vh-100'>
				<Spinner animation='border' />
			</div>
		)
	}

	return (
		<>
			<Navbar expand='lg' className='fs-4 bg-dark sticky-top'>
				<Container>
					<Brand />
					<Navbar.Toggle aria-controls='basic-navbar-nav' />
					<Navbar.Collapse id='basic-navbar-nav'>
						<Nav className='me-auto'>
							<Nav.Link href='/' className={getNavLinkClass('/')}>
								Store
							</Nav.Link>
							{isLibraryVisible && (
								<Nav.Link href='/library' className={getNavLinkClass('/library')}>
									Library
								</Nav.Link>
							)}
						</Nav>
						<UserMenu onUserRolesChange={setMenuVisibility} />
					</Navbar.Collapse>
				</Container>
			</Navbar>
			<Container>{children}</Container>
		</>
	)
}

export default MainLayout
