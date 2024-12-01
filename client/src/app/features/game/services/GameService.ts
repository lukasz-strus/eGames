import { ApiService } from '../../../core/services/ApiService'
import { Game, FullGame, DlcGame, Subscription } from '../../../core/contracts/Game'
import ApiEndpoints from '../../../core/constants/ApiEndpoints'

export class GameService extends ApiService {
	private static instance: GameService | null = null

	private constructor() {
		super(ApiEndpoints.BASE_URL)
	}

	static getInstance(): GameService {
		if (!GameService.instance) {
			GameService.instance = new GameService()
		}
		return GameService.instance
	}

	async fetchGames(): Promise<Game[]> {
		const { data } = await this.API.get(ApiEndpoints.GAMES.BASE)
		return data.items
	}

	async fetchFullGames(): Promise<Game[]> {
		const { data } = await this.API.get(ApiEndpoints.GAMES.FULL_GAME)
		return data.items
	}

	async fetchDlcGames(): Promise<Game[]> {
		const { data } = await this.API.get(ApiEndpoints.GAMES.DLC_GAME)
		return data.items
	}

	async fetchSubscriptionGames(): Promise<Game[]> {
		const { data } = await this.API.get(ApiEndpoints.GAMES.SUBSCRIPTION)
		return data.items
	}

	async fetchGameById(gameId: string, gameType: string): Promise<Game> {
		const gameTypeUrl = this.getGameTypeUrl(gameType)
		const { data } = await this.API.get(`${gameTypeUrl}/${gameId}`)
		return this.mapGameDataByType(gameType, data)
	}

	private getGameTypeUrl(gameType: string): string {
		switch (gameType) {
			case 'FullGame':
				return ApiEndpoints.GAMES.FULL_GAME
			case 'DlcGame':
				return ApiEndpoints.GAMES.DLC_GAME
			case 'Subscription':
				return ApiEndpoints.GAMES.SUBSCRIPTION
			default:
				throw new Error(`Unknown game type: ${gameType}`)
		}
	}

	private mapGameDataByType(gameType: string, data: any): Game {
		switch (gameType) {
			case 'FullGame':
				return data as FullGame
			case 'DlcGame':
				return data as DlcGame
			case 'Subscription':
				return data as Subscription
			default:
				return data
		}
	}
}
