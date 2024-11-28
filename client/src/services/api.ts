import axios from 'axios'
import { type Game } from '../contracts/Game'

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
	return data
}

export default API
