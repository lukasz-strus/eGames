export class EmailHelpers {
	static validateEmail = (email: string): boolean => {
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
		return emailRegex.test(email)
	}

	static emailError: string = 'Invalid email format.'

	static emailMatchError: string = 'Emails do not match.'
}
