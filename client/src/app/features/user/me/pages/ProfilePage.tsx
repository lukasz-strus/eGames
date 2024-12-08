import React, { useEffect, useState } from 'react'
import { Container, Spinner, Alert, Row, Col, Button } from 'react-bootstrap'
import { AuthService } from '../../../auth/services/AuthService'
import FormField from '../../../../core/components/FormField'
import { PasswordHelpers } from '../../../../core/helpers/PasswordHelpers'

const authService = AuthService.getInstance()

const UserPage: React.FC = () => {
	const [userName, setUserName] = useState<string>('')
	const [email, setEmail] = useState<string>('')
	const [loading, setLoading] = useState<boolean>(true)
	const [error, setError] = useState<string | null>(null)
	const [successMessage, setSuccessMessage] = useState<string | null>(null)

	const [oldPassword, setOldPassword] = useState<string>('')
	const [newPassword, setNewPassword] = useState<string>('')
	const [newConfirmPassword, setConfirmPassword] = useState<string>('')

	useEffect(() => {
		const fetchProfile = async (token: string) => {
			try {
				const user = await authService.getProfile(token)
				if (user) {
					setUserName(user.userName)
					setEmail(user.email)
				} else {
					localStorage.removeItem('authToken')
				}
			} catch (err) {
				setError('Failed to fetch profile information.')
			} finally {
				setLoading(false)
			}
		}

		const token = localStorage.getItem('authToken')

		if (token) {
			fetchProfile(token)
		} else {
			setLoading(false)
		}
	}, [])

	const handleUpdateProfile = async () => {
		let hasError = false
		const token = localStorage.getItem('authToken')
		if (!token) {
			setLoading(false)
			return
		}

		if (!newPassword || !PasswordHelpers.validatePassword(newPassword)) {
			setError(PasswordHelpers.passwordError)
			hasError = true
		} else if (newPassword !== newConfirmPassword) {
			setError(PasswordHelpers.passwordMatchError)
			hasError = true
		}

		if (hasError) return

		setLoading(true)
		setError(null)
		setSuccessMessage(null)

		try {
			await authService.updateProfile(token, newPassword || undefined, oldPassword || undefined)
			setSuccessMessage('Profile updated successfully.')
			setOldPassword('')
			setNewPassword('')
		} catch (err) {
			setError('Failed to update profile. Please check your input.')
		} finally {
			setLoading(false)
		}
	}

	if (loading) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Spinner animation='border' />
			</Container>
		)
	}

	return (
		<Container className='section-container'>
			<h1>User Profile</h1>
			{error && <Alert variant='danger'>{error}</Alert>}
			{successMessage && <Alert variant='success'>{successMessage}</Alert>}

			<Row>
				<Col md={6}>
					<FormField
						label='Username'
						floatingLabel={true}
						type='text'
						value={userName}
						isInvalid={false}
						onChange={() => {}}
					/>
				</Col>
				<Col md={6}>
					<FormField
						label='Email'
						type='email'
						floatingLabel={true}
						value={email}
						isInvalid={false}
						onChange={() => {}}
					/>
				</Col>
			</Row>

			<hr />

			<h3>Update Profile</h3>
			<Row>
				<Col md={6}>
					<FormField
						label='Old Password'
						type='password'
						floatingLabel={true}
						value={oldPassword}
						onChange={e => setOldPassword(e.target.value)}
					/>
				</Col>
			</Row>
			<Row>
				<Col md={6}>
					<FormField
						label='New Password'
						type='password'
						floatingLabel={true}
						value={newPassword}
						onChange={e => setNewPassword(e.target.value)}
					/>
				</Col>
				<Col md={6}>
					<FormField
						label='Confirm Password'
						type='password'
						floatingLabel={true}
						value={newConfirmPassword}
						onChange={e => setConfirmPassword(e.target.value)}
					/>
				</Col>
			</Row>
			<Button variant='primary' className='mt-3' onClick={handleUpdateProfile}>
				Update Profile
			</Button>
		</Container>
	)
}

export default UserPage
