import { useState } from 'react'
import { Button, Nav, NavDropdown } from 'react-bootstrap'
import LoginModal from '../../features/auth/components/LoginModal'
import { useAuth } from '../../core/context/AuthContext'
import InfoModal from '../../core/components/InfoModal'
import { UserRole } from '../../core/contracts/User'
import { useLocation } from 'react-router-dom'

interface UserMenuProps {
	onUserRolesChange: (roles: UserRole[]) => void
}

const UserMenu: React.FC<UserMenuProps> = ({ onUserRolesChange }) => {
	const [showLoginModal, setShowLoginModal] = useState(false)
	const { isLoggedIn, setIsLoggedIn, userName, setUserName } = useAuth()
	const [showSuccessModal, setShowSuccessModal] = useState(false)
	const location = useLocation()

	const getNavLinkClass = (path: string): string => {
		return location.pathname === path ? 'fw-bold' : ''
	}

	const handleLogout = () => {
		localStorage.removeItem('authToken')
		setIsLoggedIn(false)
		setUserName(null)

		setShowSuccessModal(true)
	}

	const handleSuccessClose = () => {
		setShowSuccessModal(false)
		window.location.reload()
	}

	return (
		<>
			<Nav>
				{isLoggedIn && (
					<Nav.Link href='/order' className={getNavLinkClass('/order')}>
						Order
					</Nav.Link>
				)}
				{isLoggedIn ? (
					<NavDropdown title={userName || 'Guest'} id='user-dropdown' className={getNavLinkClass('/profile')}>
						<NavDropdown.Item href='/profile'>Profile</NavDropdown.Item>
						<NavDropdown.Item onClick={handleLogout}>Logout</NavDropdown.Item>
					</NavDropdown>
				) : (
					<Button variant='outline-light' onClick={() => setShowLoginModal(true)}>
						Login
					</Button>
				)}
			</Nav>
			<LoginModal
				show={showLoginModal}
				onClose={() => setShowLoginModal(false)}
				onUserRolesChange={onUserRolesChange}
			/>

			{showSuccessModal && (
				<InfoModal show={showSuccessModal} onClose={handleSuccessClose} title='You have been logged out' />
			)}
		</>
	)
}

export default UserMenu
