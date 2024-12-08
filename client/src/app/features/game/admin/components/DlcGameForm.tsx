import React from 'react'
import { Row, Col, Button } from 'react-bootstrap'
import { DlcGame } from '../../../../core/contracts/Game'
import FormField from '../../../../core/components/FormField'

interface DlcGameFormProps {
	dlcGame: Partial<DlcGame>
	errors: Record<string, string | null>
	onChange: (key: keyof DlcGame, value: string | number) => void
	onSave: () => void
}

const DlcGameForm: React.FC<DlcGameFormProps> = ({ dlcGame, errors, onChange, onSave }) => {
	return (
		<>
			<Row>
				<Col md={6}>
					<FormField
						label='Title'
						type='text'
						floatingLabel={true}
						value={dlcGame.name || ''}
						isInvalid={!!errors.dlcName}
						feedback={errors.dlcName}
						onChange={e => onChange('name', e.target.value)}
					/>
				</Col>
				<Col md={6}>
					<FormField
						label='Publisher'
						type='text'
						floatingLabel={true}
						value={dlcGame.publisher || ''}
						isInvalid={!!errors.dlcPublisher}
						feedback={errors.dlcPublisher}
						onChange={e => onChange('publisher', e.target.value)}
					/>
				</Col>
			</Row>
			<Row>
				<Col md={3}>
					<FormField
						label='Price'
						type='number'
						floatingLabel={true}
						value={dlcGame.amount || ''}
						isInvalid={!!errors.dlcAmount}
						feedback={errors.dlcAmount}
						onChange={e => onChange('amount', parseFloat(e.target.value))}
					/>
				</Col>
				<Col md={3}>
					<FormField
						label='Currency'
						type='select'
						floatingLabel={true}
						value={dlcGame.currency || ''}
						isInvalid={!!errors.dlcCurrency}
						feedback={errors.dlcCurrency}
						options={[
							{ value: '', label: '' },
							{ value: 'USD', label: 'USD' },
							{ value: 'EUR', label: 'EUR' },
							{ value: 'PLN', label: 'PLN' },
						]}
						onChange={e => onChange('currency', e.target.value)}
					/>
				</Col>
				<Col md={3}>
					<FormField
						label='Release Date'
						type='date'
						floatingLabel={true}
						value={dlcGame.releaseDate || ''}
						isInvalid={!!errors.dlcReleaseDate}
						feedback={errors.dlcReleaseDate}
						onChange={e => onChange('releaseDate', e.target.value)}
					/>
				</Col>
				<Col md={3}>
					<FormField
						label='File Size'
						type='number'
						floatingLabel={true}
						value={dlcGame.fileSize || ''}
						isInvalid={!!errors.dlcFileSize}
						feedback={errors.dlcFileSize}
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
						value={dlcGame.description || ''}
						isInvalid={!!errors.dlcDescription}
						feedback={errors.dlcDescription}
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
						value={dlcGame.downloadLink || ''}
						isInvalid={!!errors.dlcDownloadLink}
						feedback={errors.dlcDownloadLink}
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
						value={dlcGame.imageUrl || ''}
						isInvalid={!!errors.dlcImageUrl}
						feedback={errors.dlcImageUrl}
						onChange={e => onChange('imageUrl', e.target.value)}
					/>
				</Col>
			</Row>
			<Row className='mt-3'>
				<Col>
					<Button variant='success' onClick={onSave}>
						Save DLC Game
					</Button>
				</Col>
			</Row>
		</>
	)
}

export default DlcGameForm
