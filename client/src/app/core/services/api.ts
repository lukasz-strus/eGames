import axios from 'axios'
import { type DlcGame, type Subscription, type FullGame, type Game } from '../contracts/Game'

const API = axios.create({
	baseURL: 'https://localhost:7164/api',
})

export const loginUser = async (email: string, password: string): Promise<string> => {
	const { data } = await API.post('/identity/login', { email, password })

	return data.accessToken
}

export const getEmail = async (token: string): Promise<string> => {
	const { data } = await API.get('/identity/manage/info', {
		headers: {
			Authorization: `Bearer ${token}`,
		},
	})

	return data.email
}

export const fetchGames = async (): Promise<Game[]> => {
	const { data } = await API.get('/games')

	return data.items
}

export const fetchGameById = async (gameId: string, gameType: string): Promise<Game> => {
	let gameTypeUrl: string = ''

	gameTypeUrl = getGameType(gameType, gameTypeUrl)

	const { data } = await API.get(`/games/${gameTypeUrl}${gameId}`)

	return mapGameDataByType(gameType, data)
}

export default API

function mapGameDataByType(gameType: string, data: any) {
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

function getGameType(gameType: string, gameTypeUrl: string) {
	switch (gameType) {
		case 'FullGame':
			gameTypeUrl = 'full/'
			break
		case 'DlcGame':
			gameTypeUrl = 'dlc/'
			break
		case 'Subscription':
			gameTypeUrl = 'subscriptions/'
			break
		default:
			throw new Error(`Unknown game type: ${gameType}`)
	}
	return gameTypeUrl
}
