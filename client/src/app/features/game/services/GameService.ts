import { ApiService, FetchParams } from '../../../core/services/ApiService'
import { Game, FullGame, DlcGame, Subscription, Games } from '../../../core/contracts/Game'
import ApiEndpoints from '../../../core/constants/ApiEndpoints'
import { GameType } from '../../../core/enums/GameType'
import { Currency } from '../../../core/enums/Currency'

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

	async fetchGameById(gameId: string, gameType?: GameType): Promise<Game> {
		if (gameType) {
			const gameTypeUrl = this.getGameTypeUrl(gameType)
			const { data } = await this.API.get(`${gameTypeUrl}/${gameId}`)
			return this.mapGameDataByType(gameType, data)
		} else {
			const { data } = await this.API.get(`${ApiEndpoints.GAMES.BASE}/${gameId}`)
			return data
		}
	}

	async deleteGame(gameId: string, token: string): Promise<void> {
		await this.API.delete(`${ApiEndpoints.GAMES.BASE}/${gameId}`, {
			params: {
				destroy: false,
			},
			...this.setAuthorizationHeader(token),
		})
	}

	async restoreGame(gameId: string, token: string): Promise<void> {
		await this.API.post(ApiEndpoints.GAMES.RESTORE(gameId), {}, this.setAuthorizationHeader(token))
	}

	async createFullGame(game: FullGame, token: string): Promise<FullGame> {
		const request = {
			name: game.name,
			description: game.description,
			price: game.amount,
			currencyId: Currency.fromName(game.currency).id,
			releaseDate: game.releaseDate,
			publisher: game.publisher,
			downloadLink: game.downloadLink,
			fileSize: game.fileSize,
			imageUrl: game.imageUrl,
		}
		const { data } = await this.API.post(ApiEndpoints.GAMES.FULL_GAME, request, this.setAuthorizationHeader(token))

		for (const dlcGame of game.dlcGames) {
			await this.createDlcGame(dlcGame, data.id, token)
		}

		return data
	}

	async createSubscription(game: Subscription, token: string): Promise<Subscription> {
		const request = {
			name: game.name,
			description: game.description,
			price: game.amount,
			currencyId: Currency.fromName(game.currency).id,
			releaseDate: game.releaseDate,
			publisher: game.publisher,
			downloadLink: game.downloadLink,
			fileSize: game.fileSize,
			imageUrl: game.imageUrl,
			periodInDays: game.subscriptionPeriodInDays,
		}
		const { data } = await this.API.post(ApiEndpoints.GAMES.SUBSCRIPTION, request, this.setAuthorizationHeader(token))
		return data
	}

	async createDlcGame(dlcGame: Game, baseGameId: string, token: string): Promise<DlcGame> {
		const request = {
			name: dlcGame.name,
			description: dlcGame.description,
			price: dlcGame.amount,
			currencyId: Currency.fromName(dlcGame.currency).id,
			releaseDate: dlcGame.releaseDate,
			publisher: dlcGame.publisher,
			downloadLink: dlcGame.downloadLink,
			fileSize: dlcGame.fileSize,
			imageUrl: dlcGame.imageUrl,
		}

		const { data } = await this.API.post(
			ApiEndpoints.GAMES.FULL_GAME_DLCS(baseGameId),
			request,
			this.setAuthorizationHeader(token)
		)
		return data
	}

	async updateFullGame(gameId: string, game: FullGame, token: string): Promise<void> {
		const request = {
			name: game.name,
			description: game.description,
			price: game.amount,
			currencyId: Currency.fromName(game.currency).id,
			releaseDate: game.releaseDate,
			publisher: game.publisher,
			downloadLink: game.downloadLink,
			fileSize: game.fileSize,
			imageUrl: game.imageUrl,
		}

		await this.API.put(`${ApiEndpoints.GAMES.FULL_GAME}/${gameId}`, request, this.setAuthorizationHeader(token))
	}

	async updateSubscription(gameId: string, game: Subscription, token: string): Promise<void> {
		const request = {
			name: game.name,
			description: game.description,
			price: game.amount,
			currencyId: Currency.fromName(game.currency).id,
			releaseDate: game.releaseDate,
			publisher: game.publisher,
			downloadLink: game.downloadLink,
			fileSize: game.fileSize,
			imageUrl: game.imageUrl,
			periodInDays: game.subscriptionPeriodInDays,
		}
		await this.API.put(`${ApiEndpoints.GAMES.SUBSCRIPTION}/${gameId}`, request, this.setAuthorizationHeader(token))
	}

	async updateDlcGame(gameId: string, dlcGame: DlcGame, token: string): Promise<void> {
		const request = {
			name: dlcGame.name,
			description: dlcGame.description,
			price: dlcGame.amount,
			currencyId: Currency.fromName(dlcGame.currency).id,
			releaseDate: dlcGame.releaseDate,
			publisher: dlcGame.publisher,
			downloadLink: dlcGame.downloadLink,
			fileSize: dlcGame.fileSize,
			imageUrl: dlcGame.imageUrl,
		}

		await this.API.put(`${ApiEndpoints.GAMES.DLC_GAME}/${gameId}`, request, this.setAuthorizationHeader(token))
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
				return data as Game
		}
	}
}
