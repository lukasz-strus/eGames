import axios from 'axios'
import { type Game } from '../contracts/Game'

const API = axios.create({
	baseURL: 'https://localhost:7164/api',
})

export const fetchGames = async (): Promise<Game[]> => {
	const { data } = await API.get('/games')
	return data.items
}

export default API
