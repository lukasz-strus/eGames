import { useEffect, useState } from 'react'
import { Button, Nav, NavDropdown } from 'react-bootstrap'
import LoginModal from '../../features/auth/components/LoginModal'
import { useAuth } from '../../core/context/AuthContext'
import InfoModal from '../../core/components/InfoModal'
import { UserRole } from '../../core/contracts/User'
import { Link, useLocation } from 'react-router-dom'
import { OrderService } from '../../features/order/services/OrderService'

const orderService = OrderService.getInstance()

interface UserMenuProps {
	onUserRolesChange: (roles: UserRole[]) => void
}

const UserMenu: React.FC<UserMenuProps> = ({ onUserRolesChange }) => {
	const [showLoginModal, setShowLoginModal] = useState(false)
	const { isLoggedIn, setIsLoggedIn, userName, setUserName } = useAuth()
	const [showSuccessModal, setShowSuccessModal] = useState(false)
	const [orderCount, setOrderCount] = useState<string>('')
	const location = useLocation()

	function getNavLinkClass(path: string): string {
		return location.pathname === path ? 'fw-bold' : ''
	}

	function handleLogout() {
		localStorage.removeItem('authToken')
		setIsLoggedIn(false)
		setUserName(null)

		setShowSuccessModal(true)
	}

	function handleSuccessClose() {
		setShowSuccessModal(false)
		window.location.reload()
	}

	function getOrderHeader(count: string) {
		return count === '' ? 'Order' : `Order (${count})`
	}

	useEffect(() => {
		const fetchOrderCount = async (token: string) => {
			const order = await orderService.getOrder(token)

			if (!order) {
				setOrderCount('')
				return
			}

			setOrderCount(order.orderItems.length.toString())
		}

		const token = localStorage.getItem('authToken')
		if (token) {
			fetchOrderCount(token)
		} else {
			setOrderCount('')
		}
	}, [])

	return (
		<>
			<Nav>
				{isLoggedIn && (
					<Nav.Link as={Link} to='/order' className={getNavLinkClass('/order')}>
						{getOrderHeader(orderCount)}
					</Nav.Link>
				)}
				{isLoggedIn ? (
					<NavDropdown title={userName || 'Guest'} id='user-dropdown' className={getNavLinkClass('/profile')}>
						<NavDropdown.Item as={Link} to='/profile'>
							Profile
						</NavDropdown.Item>
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
