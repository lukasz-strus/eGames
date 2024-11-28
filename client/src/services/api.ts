import axios from 'axios'
import { type DlcGame, type Subscription, type FullGame, type Game } from '../contracts/Game'

const API = axios.create({
	baseURL: 'https://localhost:7164/api',
})

export const fetchGames = async (): Promise<Game[]> => {
	const { data } = await API.get('/games')
	return data.items
}

export const fetchGameById = async (gameId: string, gameType: string): Promise<Game> => {
	let gameTypeUrl: string = ''

	if (gameType === 'FullGame') gameTypeUrl = 'full/'
	else if (gameType === 'DlcGame') gameTypeUrl = 'dlc/'
	else if (gameType === 'Subscription') gameTypeUrl = 'subscriptions/'

	const { data } = await API.get(`/games/${gameTypeUrl}${gameId}`)

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

export default API
