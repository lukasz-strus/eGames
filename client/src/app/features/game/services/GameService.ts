import { ApiService, FetchParams } from '../../../core/services/ApiService'
import { Game, FullGame, DlcGame, Subscription, Games } from '../../../core/contracts/Game'
import ApiEndpoints from '../../../core/constants/ApiEndpoints'
import { GameType } from '../../../core/enums/GameType'

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

	async fetchGames(params: FetchParams): Promise<Games> {
		const queryParams = this.buildQueryParams(params)

		const { data } = await this.API.get(ApiEndpoints.GAMES.BASE, { params: queryParams })

		return data
	}

	async fetchFullGames(params: FetchParams): Promise<Games> {
		const queryParams = this.buildQueryParams(params)

		const { data } = await this.API.get(ApiEndpoints.GAMES.FULL_GAME, { params: queryParams })

		return data
	}

	async fetchSubscriptionGames(params: FetchParams): Promise<Games> {
		const queryParams = this.buildQueryParams(params)

		const { data } = await this.API.get(ApiEndpoints.GAMES.SUBSCRIPTION, { params: queryParams })

		return data
	}

	async fetchGameById(gameId: string, gameType: GameType): Promise<Game> {
		const gameTypeUrl = this.getGameTypeUrl(gameType)
		const { data } = await this.API.get(`${gameTypeUrl}/${gameId}`)
		return this.mapGameDataByType(gameType, data)
	}

	private getGameTypeUrl(gameType: GameType): string {
		switch (gameType) {
			case GameType.FullGame:
				return ApiEndpoints.GAMES.FULL_GAME
			case 'DlcGame':
				return ApiEndpoints.GAMES.DLC_GAME
			case 'Subscription':
				return ApiEndpoints.GAMES.SUBSCRIPTION
			default:
				throw new Error(`Unknown game type: ${gameType}`)
		}
	}

	private mapGameDataByType(gameType: GameType, data: any): Game {
		switch (gameType) {
			case GameType.FullGame:
				return data as FullGame
			case GameType.DlcGame:
				return data as DlcGame
			case GameType.Subscription:
				return data as Subscription
			default:
				return data
		}
	}
}
