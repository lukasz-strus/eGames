export interface User {
	id: string
	email: string
	userName: string
	userRoles: UserRoles
}

export interface UserRoles {
	items: UserRole[]
}

export interface UserRole {
	id: number
	name: string
}
