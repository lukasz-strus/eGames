import { useState } from 'react'
import { Button, Form, Modal, Alert } from 'react-bootstrap'
import FormField from '../../../core/components/FormField'
import { AuthService } from '../services/AuthService'
import SuccessModal from '../../../core/components/SuccessModal'
import { PasswordHelpers } from '../../../core/helpers/PasswordHelpers'
import { EmailHelpers } from '../../../core/helpers/EmailHelpers'

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

	const handleRegister = async () => {
		let hasError = false

		setEmailError(null)
		setPasswordError(null)
		setError(null)

		if (!email || !EmailHelpers.validateEmail(email)) {
			setEmailError(EmailHelpers.emailError)
			hasError = true
		} else if (email !== confirmEmail) {
			setEmailError(EmailHelpers.emailMatchError)
			hasError = true
		}

		if (!password || !PasswordHelpers.validatePassword(password)) {
			setPasswordError(PasswordHelpers.passwordError)
			hasError = true
		} else if (password !== confirmPassword) {
			setPasswordError(PasswordHelpers.passwordMatchError)
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
