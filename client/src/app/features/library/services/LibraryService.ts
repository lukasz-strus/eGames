import { ApiService, FetchParams } from '../../../core/services/ApiService'
import ApiEndpoints from '../../../core/constants/ApiEndpoints'
import { FullLibraryGame, LibraryGames } from '../../../core/contracts/LibraryGame'

export class LibraryService extends ApiService {
	private static instance: LibraryService | null = null

	private constructor() {
		super(ApiEndpoints.BASE_URL)
	}

	static getInstance(): LibraryService {
		if (!LibraryService.instance) {
			LibraryService.instance = new LibraryService()
		}
		return LibraryService.instance
	}

	async getOwnLibrary(params: FetchParams, token: string): Promise<LibraryGames> {
		const queryParams = this.buildQueryParams(params)

		const { data } = await this.API.get(ApiEndpoints.LIBRARY.OWN, {
			params: queryParams,
			headers: {
				Authorization: `Bearer ${token}`,
			},
		})

		return data
	}

	async getLibraryGameById(libraryGameId: string, token: string): Promise<FullLibraryGame> {
		const { data } = await this.API.get(
			`${ApiEndpoints.LIBRARY.BASE}/${libraryGameId}`,
			this.setAuthorizationHeader(token)
		)

		return data
	}
}
