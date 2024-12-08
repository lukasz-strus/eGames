import React from 'react'
import { Row, Col } from 'react-bootstrap'
import { Game, Subscription } from '../../../../core/contracts/Game'
import { GameType } from '../../../../core/enums/GameType'
import FormField from '../../../../core/components/FormField'

interface GameDetailsFormProps {
	game: Partial<Game | Subscription>
	errors: Record<string, string | null>
	onChange: (key: keyof Game | keyof Subscription, value: string | number) => void
}

const GameDetailsForm: React.FC<GameDetailsFormProps> = ({ game, errors, onChange }) => {
	const formatDate = (date: string): string => {
		const d = new Date(date)
		const year = d.getFullYear()
		const month = String(d.getMonth() + 1).padStart(2, '0')
		const day = String(d.getDate()).padStart(2, '0')
		return `${year}-${month}-${day}`
	}

	return (
		<>
			<Row>
				<Col>
					<FormField
						label='Game Title'
						type='text'
						floatingLabel={true}
						value={game.name || ''}
						isInvalid={!!errors.name}
						feedback={errors.name}
						onChange={e => onChange('name', e.target.value)}
					/>
				</Col>
			</Row>
			<Row>
				{game.type === GameType.DlcGame && (
					<Col md={6}>
						<FormField label='Game Type' type='text' floatingLabel={true} value={game.type || ''} onChange={() => {}} />
					</Col>
				)}
				{game.type !== GameType.DlcGame && (
					<Col md={6}>
						<FormField
							label='Game Type'
							type='select'
							floatingLabel={true}
							value={game.type || ''}
							options={[
								{ value: '', label: '' },
								{ value: GameType.FullGame, label: 'Full Game' },
								{ value: GameType.Subscription, label: 'Subscription' },
							]}
							isInvalid={!!errors.type}
							feedback={errors.type}
							onChange={e => onChange('type', e.target.value)}
						/>
					</Col>
				)}
				<Col md={3}>
					<FormField
						label='Price'
						type='number'
						floatingLabel={true}
						value={game.amount || ''}
						isInvalid={!!errors.amount}
						feedback={errors.amount}
						onChange={e => onChange('amount', parseFloat(e.target.value))}
					/>
				</Col>
				<Col md={3}>
					<FormField
						label='Currency'
						type='select'
						floatingLabel={true}
						value={game.currency || ''}
						options={[
							{ value: '', label: '' },
							{ value: 'USD', label: 'USD' },
							{ value: 'EUR', label: 'EUR' },
							{ value: 'PLN', label: 'PLN' },
						]}
						isInvalid={!!errors.currency}
						feedback={errors.currency}
						onChange={e => onChange('currency', e.target.value)}
					/>
				</Col>
			</Row>
			<Row>
				<Col md={6}>
					<FormField
						label='Publisher'
						type='text'
						floatingLabel={true}
						value={game.publisher || ''}
						isInvalid={!!errors.publisher}
						feedback={errors.publisher}
						onChange={e => onChange('publisher', e.target.value)}
					/>
				</Col>
				<Col md={3}>
					<FormField
						label='Release Date'
						type='date'
						floatingLabel={true}
						value={game.releaseDate ? formatDate(game.releaseDate) : ''}
						isInvalid={!!errors.releaseDate}
						feedback={errors.releaseDate}
						onChange={e => onChange('releaseDate', e.target.value)}
					/>
				</Col>
				<Col md={3}>
					<FormField
						label='File Size'
						type='number'
						floatingLabel={true}
						value={game.fileSize || ''}
						isInvalid={!!errors.fileSize}
						feedback={errors.fileSize}
						onChange={e => onChange('fileSize', parseFloat(e.target.value))}
					/>
				</Col>
			</Row>
			<Row>
				<Col>
					<FormField
						label='Description'
						type='text'
						as='textarea'
						floatingLabel={true}
						value={game.description || ''}
						isInvalid={!!errors.description}
						feedback={errors.description}
						onChange={e => onChange('description', e.target.value)}
					/>
				</Col>
			</Row>
			<Row>
				<Col>
					<FormField
						label='Download Link'
						type='text'
						as='textarea'
						floatingLabel={true}
						value={game.downloadLink || ''}
						isInvalid={!!errors.downloadLink}
						feedback={errors.downloadLink}
						onChange={e => onChange('downloadLink', e.target.value)}
					/>
				</Col>
			</Row>
			<Row>
				<Col>
					<FormField
						label='Image Url'
						type='text'
						as='textarea'
						floatingLabel={true}
						value={game.imageUrl || ''}
						isInvalid={!!errors.imageUrl}
						feedback={errors.imageUrl}
						onChange={e => onChange('imageUrl', e.target.value)}
					/>
				</Col>
			</Row>
			{game.type === 'Subscription' && (
				<Row>
					<Col md={3}>
						<FormField
							label='Subscription Period In Days'
							type='number'
							floatingLabel={true}
							value={(game as Subscription).subscriptionPeriodInDays || ''}
							onChange={e => onChange('subscriptionPeriodInDays', parseFloat(e.target.value))}
						/>
					</Col>
				</Row>
			)}
		</>
	)
}

export default GameDetailsForm
