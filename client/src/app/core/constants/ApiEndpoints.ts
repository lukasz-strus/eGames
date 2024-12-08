class ApiEndpoints {
	static readonly BASE_URL = 'https://egames-api.azurewebsites.net/api'
	static readonly DEV_BASE_URL = 'https://localhost:7164/api'

	static readonly AUTH = {
		LOGIN: '/identity/login',
		PROFILE: '/identity/manage/info',
		REGISTER: '/identity/register',
	}

	static readonly GAMES = {
		BASE: '/games',
		FULL_GAME: '/games/full',
		FULL_GAME_DLCS: (gameId: string) => `/games/full/${gameId}/dlc`,
		DLC_GAME: '/games/dlc',
		SUBSCRIPTION: '/games/subscriptions',
		RESTORE: (gameId: string) => `/games/${gameId}/restore`,
	}

	static readonly ORDER = {
		OWN: 'me/orders',
		BASE: '/orders',
		ITEMS: (orderId: string) => `/orders/${orderId}/items`,
		ITEM: (orderId: string, itemId: string) => `/orders/${orderId}/items/${itemId}`,
		PAY: (orderId: string) => `/orders/${orderId}/pay`,
	}

	static readonly LIBRARY = {
		OWN: 'me/library-games',
		BASE: '/library-games',
	}
}

export default ApiEndpoints
