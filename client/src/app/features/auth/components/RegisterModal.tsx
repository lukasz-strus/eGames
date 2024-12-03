import { useState } from 'react'
import { Button, Form, Modal, Alert } from 'react-bootstrap'
import FormField from '../../../core/components/FormField'
import { AuthService } from '../services/AuthService'
import SuccessModal from '../../../core/components/SuccessModal'

const authService = AuthService.getInstance()

interface RegisterModalProps {
	show: boolean
	onClose: () => void
}

const RegisterModal: React.FC<RegisterModalProps> = ({ show, onClose }) => {
	const [email, setEmail] = useState('')
	const [confirmEmail, setConfirmEmail] = useState('')
	const [password, setPassword] = useState('')
	const [confirmPassword, setConfirmPassword] = useState('')
	const [emailError, setEmailError] = useState<string | null>(null)
	const [passwordError, setPasswordError] = useState<string | null>(null)
	const [error, setError] = useState<string | null>(null)
	const [showSuccessModal, setShowSuccessModal] = useState(false)

	const validateEmail = (email: string): boolean => {
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
		return emailRegex.test(email)
	}

	const validatePassword = (password: string): boolean => {
		const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/
		return passwordRegex.test(password)
	}

	const handleRegister = async () => {
		let hasError = false

		setEmailError(null)
		setPasswordError(null)
		setError(null)

		if (!email || !validateEmail(email)) {
			setEmailError('Invalid email format.')
			hasError = true
		} else if (email !== confirmEmail) {
			setEmailError('Emails do not match.')
			hasError = true
		}

		if (!password || !validatePassword(password)) {
			setPasswordError(
				'Password must be at least 8 characters, include 1 lowercase, 1 uppercase, 1 number, and 1 special character.'
			)
			hasError = true
		} else if (password !== confirmPassword) {
			setPasswordError('Passwords do not match.')
			hasError = true
		}

		if (hasError) return

		try {
			await authService.registerUser(email, email, password)

			setShowSuccessModal(true)
		} catch {
			setError('Registration failed. Please try again.')
		}
	}

	const handleClose = () => {
		setEmail('')
		setConfirmEmail('')
		setPassword('')
		setConfirmPassword('')
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
		<>
			<Modal show={show} onHide={handleClose} centered>
				<Modal.Header closeButton>
					<Modal.Title>Register</Modal.Title>
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
							placeholder='Enter email'
							value={email}
							onChange={e => setEmail(e.target.value)}
							isInvalid={!!emailError}
							feedback={emailError}
						/>
						<FormField
							label='Confirm Email'
							type='email'
							placeholder='Confirm email'
							value={confirmEmail}
							onChange={e => setConfirmEmail(e.target.value)}
							isInvalid={!!emailError}
						/>
						<FormField
							label='Password'
							type='password'
							placeholder='Enter password'
							value={password}
							onChange={e => setPassword(e.target.value)}
							isInvalid={!!passwordError}
							feedback={passwordError}
						/>
						<FormField
							label='Confirm Password'
							type='password'
							placeholder='Confirm password'
							value={confirmPassword}
							onChange={e => setConfirmPassword(e.target.value)}
							isInvalid={!!passwordError}
						/>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					<Button variant='secondary' onClick={handleClose}>
						Close
					</Button>
					<Button variant='primary' onClick={handleRegister}>
						Register
					</Button>
				</Modal.Footer>
			</Modal>

			{showSuccessModal && (
				<SuccessModal show={showSuccessModal} onClose={handleSuccessClose} title='Registration Successful' />
			)}
		</>
	)
}

export default RegisterModal
