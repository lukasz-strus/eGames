export class Currency {
	static USD = {
		id: 1,
		name: 'USD',
	}
	static EUR = {
		id: 2,
		name: 'EUR',
	}
	static PLN = {
		id: 3,
		name: 'PLN',
	}

	static fromName(name: string): any {
		switch (name) {
			case 'USD':
				return Currency.USD
			case 'EUR':
				return Currency.EUR
			case 'PLN':
				return Currency.PLN
			default:
				throw new Error(`Unknown currency: ${name}`)
		}
	}
}
