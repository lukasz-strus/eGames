import { useState } from 'react'
import { Button, Nav, NavDropdown } from 'react-bootstrap'
import LoginModal from '../../features/auth/components/LoginModal'

interface UserMenuProps {
	isLoggedIn: boolean
	userName: string | null
	onLogin: (email: string, password: string) => Promise<void>
	onLogout: () => void
}

const UserMenu: React.FC<UserMenuProps> = ({ isLoggedIn, userName, onLogin, onLogout }) => {
	const [showLoginModal, setShowLoginModal] = useState(false)

	return (
		<>
			<Nav>
				{isLoggedIn ? (
					<NavDropdown title={userName || 'Guest'} id='user-dropdown'>
						<NavDropdown.Item href='/profile'>Profile</NavDropdown.Item>
						<NavDropdown.Item onClick={onLogout}>Logout</NavDropdown.Item>
					</NavDropdown>
				) : (
					<Button variant='outline-light' onClick={() => setShowLoginModal(true)}>
						Login
					</Button>
				)}
			</Nav>
			<LoginModal show={showLoginModal} onClose={() => setShowLoginModal(false)} onLogin={onLogin} />
		</>
	)
}

export default UserMenu
