export enum GameType {
	FullGame = 'FullGame',
	DlcGame = 'DlcGame',
	Subscription = 'Subscription',
}

export class GameTypeExtensions {
	static getGameType(gameType: string): string {
		switch (gameType) {
			case 'FullGame':
				return 'Full Game'
			case 'DlcGame':
				return 'DLC Game'
			case 'Subscription':
				return 'Subscription Game'
			default:
				throw new Error(`Unknown game type: ${gameType}`)
		}
	}

	static getGameTypes(gameType: string): string {
		switch (gameType) {
			case 'FullGame':
				return 'Full Games'
			case 'DlcGame':
				return 'DLC Games'
			case 'Subscription':
				return 'Subscription Games'
			case 'All Games':
			case 'All':
				return 'All Games'
			default:
				throw new Error(`Unknown game type: ${gameType}`)
		}
	}
}
