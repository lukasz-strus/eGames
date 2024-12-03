export interface Game {
	id: string
	type: string
	name: string
	description: string
	currency: string
	amount: number
	releaseDate: string
	publisher: string
	downloadLink: string
	fileSize: number
	imageUrl: string
}

export interface FullGame extends Game {
	dlcGames: DlcGame[]
}

export interface DlcGame extends Game {
	baseGameId: string
}

export interface Subscription extends Game {
	subscriptionPeriodInDays: number
}

export interface Games {
	totalPages: number
	itemsFrom: number
	itemsTo: number
	totalItemsCount: number
	items: Game[]
}
