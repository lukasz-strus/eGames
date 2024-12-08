import { useState } from 'react'
import { Button, Form, Modal, Alert } from 'react-bootstrap'
import FormField from '../../../core/components/FormField'
import RegisterModal from './RegisterModal'
import { useAuth } from '../../../core/context/AuthContext'
import { AuthService } from '../services/AuthService'
import SuccessModal from '../../../core/components/SuccessModal'
import { UserRole } from '../../../core/contracts/User'

const authService = AuthService.getInstance()

interface LoginModalProps {
	show: boolean
	onClose: () => void
	onUserRolesChange: (roles: UserRole[]) => void
}

const LoginModal: React.FC<LoginModalProps> = ({ show, onClose, onUserRolesChange }) => {
	const [email, setEmail] = useState('')
	const [password, setPassword] = useState('')
	const [error, setError] = useState<string | null>(null)
	const [emailError, setEmailError] = useState<string | null>(null)
	const [passwordError, setPasswordError] = useState<string | null>(null)
	const [showRegisterModal, setShowRegisterModal] = useState(false)
	const { setIsLoggedIn, setUserName } = useAuth()
	const [showSuccessModal, setShowSuccessModal] = useState(false)

	const validateEmail = (email: string): boolean => {
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
		return emailRegex.test(email)
	}

	const handleLogin = async () => {
		let hasError = false

		setEmailError(null)
		setPasswordError(null)
		setError(null)

		if (!email) {
			setEmailError('Email cannot be empty.')
			hasError = true
		} else if (!validateEmail(email)) {
			setEmailError('Invalid email format.')
			hasError = true
		}

		if (!password) {
			setPasswordError('Password cannot be empty.')
			hasError = true
		}

		if (hasError) return

		try {
			const token = await authService.loginUser(email, password)
			localStorage.setItem('authToken', token)

			const user = await authService.getProfile(token)

			setIsLoggedIn(true)
			setUserName(user.userName)
			onUserRolesChange(user.userRoles.items)

			setShowSuccessModal(true)
		} catch {
			setError('Login failed. Please check your credentials.')
		}
	}

	const handleClose = () => {
		setEmail('')
		setPassword('')
		setError(null)
		setEmailError(null)
		setPasswordError(null)
		onClose()
	}

	const handleSuccessClose = () => {
		setShowSuccessModal(false)
		handleClose()
	}

	return (
		<Modal show={show} onHide={handleClose} centered>
			<Modal.Header closeButton>
				<Modal.Title>Login</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				{error && (
					<Alert variant='danger' className='mb-3'>
						{error}
					</Alert>
				)}
				<Form>
					<FormField
						label='Email'
						type='email'
						floatingLabel={true}
						value={email}
						onChange={e => setEmail(e.target.value)}
						isInvalid={!!emailError}
						feedback={emailError}
					/>
					<FormField
						label='Password'
						type='password'
						floatingLabel={true}
						value={password}
						onChange={e => setPassword(e.target.value)}
						isInvalid={!!passwordError}
						feedback={passwordError}
					/>
				</Form>
			</Modal.Body>
			<Modal.Footer>
				<p className='text-muted'>
					Don't have an account?{' '}
					<Button variant='link' onClick={() => setShowRegisterModal(true)}>
						Sign up
					</Button>
				</p>
				<Button variant='secondary' onClick={handleClose}>
					Close
				</Button>
				<Button variant='primary' onClick={handleLogin}>
					Login
				</Button>
			</Modal.Footer>

			{showRegisterModal && <RegisterModal show={showRegisterModal} onClose={() => setShowRegisterModal(false)} />}

			{showSuccessModal && (
				<SuccessModal show={showSuccessModal} onClose={handleSuccessClose} title='Login Successful' />
			)}
		</Modal>
	)
}

export default LoginModal
