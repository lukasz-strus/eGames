import { ApiService } from '../../../core/services/ApiService.ts'
import ApiEndpoints from '../../../core/constants/ApiEndpoints'
import { User } from '../../../core/contracts/User.ts'

export class AuthService extends ApiService {
	private static instance: AuthService | null = null

	private constructor() {
		super(ApiEndpoints.BASE_URL)
	}

	static getInstance(): AuthService {
		if (!AuthService.instance) {
			AuthService.instance = new AuthService()
		}
		return AuthService.instance
	}

	async loginUser(email: string, password: string): Promise<string> {
		const { data } = await this.API.post(ApiEndpoints.AUTH.LOGIN, { email, password })
		return data.accessToken
	}

	async getProfile(token: string): Promise<User> {
		const { data } = await this.API.get(ApiEndpoints.AUTH.PROFILE, this.setAuthorizationHeader(token))
		return data
	}

	async updateProfile(token: string, newPassword?: string, oldPassword?: string): Promise<void> {
		await this.API.post(ApiEndpoints.AUTH.PROFILE, { newPassword, oldPassword }, this.setAuthorizationHeader(token))
	}

	async registerUser(userName: string, email: string, password: string): Promise<void> {
		await this.API.post(ApiEndpoints.AUTH.REGISTER, { userName, email, password })
	}
}
