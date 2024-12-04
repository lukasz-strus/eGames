import { Game } from './Game'

export interface LibraryGame {
	id: string
	gameId: string
	name: string
	imageUrl: string
}

export interface LibraryGames {
	totalPages: number
	itemsFrom: number
	itemsTo: number
	totalItemsCount: number
	items: LibraryGame[]
}

export interface FullLibraryGame {
	id: string
	gameId: string
	purchaseDate: string
	licenceKey: string
	gameResponse: Game
}
