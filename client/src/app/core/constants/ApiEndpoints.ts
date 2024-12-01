class ApiEndpoints {
	static readonly BASE_URL = 'https://localhost:7164/api'

	static readonly AUTH = {
		LOGIN: '/identity/login',
		PROFILE: '/identity/manage/info',
		REGISTER: '/identity/register',
	}

	static readonly GAMES = {
		BASE: '/games',
		FULL_GAME: '/games/full',
		DLC_GAME: '/games/dlc',
		SUBSCRIPTION: '/games/subscriptions',
	}
}

export default ApiEndpoints
