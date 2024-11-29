import { useEffect, useState } from 'react'
import { Button, Form, Modal, Alert } from 'react-bootstrap'

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
			onClose()
		} catch {
			setError('Login failed. Please check your credentials.')
		}
	}

	const handleKeyDown = (event: KeyboardEvent) => {
		if (event.key === 'Enter') {
			event.preventDefault()
			handleLogin()
		} else if (event.key === 'Escape') {
			event.preventDefault()
			onClose()
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
		<Modal show={show} onHide={onClose}>
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
					<Form.Group className='mb-3' controlId='formEmail'>
						<Form.Label>Email</Form.Label>
						<Form.Control
							type='email'
							placeholder='Enter email'
							value={email}
							onChange={e => setEmail(e.target.value)}
							isInvalid={!!emailError}
						/>
						<Form.Control.Feedback type='invalid'>{emailError}</Form.Control.Feedback>
					</Form.Group>
					<Form.Group className='mb-3' controlId='formPassword'>
						<Form.Label>Password</Form.Label>
						<Form.Control
							type='password'
							placeholder='Enter password'
							value={password}
							onChange={e => setPassword(e.target.value)}
							isInvalid={!!passwordError}
						/>
						<Form.Control.Feedback type='invalid'>{passwordError}</Form.Control.Feedback>
					</Form.Group>
				</Form>
			</Modal.Body>
			<Modal.Footer>
				<Button variant='secondary' onClick={onClose}>
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
