export class PasswordHelpers {
	static validatePassword = (password: string): boolean => {
		const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/

		return passwordRegex.test(password)
	}

	static passwordError: string =
		'Password must be at least 8 characters, include 1 lowercase, 1 uppercase, 1 number, and 1 special character.'

	static passwordMatchError: string = 'Passwords do not match.'
}
