import { useEffect, useState } from 'react'
import { Button, Form, Modal, Alert } from 'react-bootstrap'
import FormField from '../../../core/components/FormField'

interface LoginModelProps {
	show: boolean
	onClose: () => void
	onLogin: (email: string, password: string) => Promise<void>
}

const LoginModal: React.FC<LoginModelProps> = ({ show, onClose, onLogin }) => {
	const [email, setEmail] = useState('')
	const [password, setPassword] = useState('')
	const [error, setError] = useState<string | null>(null)
	const [emailError, setEmailError] = useState<string | null>(null)
	const [passwordError, setPasswordError] = useState<string | null>(null)

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
			await onLogin(email, password)
			handleClose()
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

	const handleKeyDown = (event: KeyboardEvent) => {
		if (event.key === 'Enter') {
			event.preventDefault()
			handleLogin()
		} else if (event.key === 'Escape') {
			event.preventDefault()
			handleClose()
		}
	}

	useEffect(() => {
		if (show) {
			window.addEventListener('keydown', handleKeyDown)
		} else {
			window.removeEventListener('keydown', handleKeyDown)
		}

		return () => {
			window.removeEventListener('keydown', handleKeyDown)
		}
	}, [show])

	return (
		<Modal show={show} onHide={handleClose}>
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
						placeholder='Enter email'
						value={email}
						onChange={e => setEmail(e.target.value)}
						isInvalid={!!emailError}
						feedback={emailError}
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
				</Form>
			</Modal.Body>
			<Modal.Footer>
				<Button variant='secondary' onClick={handleClose}>
					Close
				</Button>
				<Button variant='primary' onClick={handleLogin}>
					Login
				</Button>
			</Modal.Footer>
		</Modal>
	)
}

export default LoginModal
