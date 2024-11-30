import axios, { AxiosInstance } from 'axios'

export abstract class ApiService {
	protected API: AxiosInstance

	constructor(baseURL: string) {
		this.API = axios.create({
			baseURL,
		})
	}

	protected setAuthorizationHeader(token: string) {
		return {
			headers: {
				Authorization: `Bearer ${token}`,
			},
		}
	}
}
