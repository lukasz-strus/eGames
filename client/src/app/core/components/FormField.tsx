import React from 'react'
import { Form } from 'react-bootstrap'

interface FormFieldProps {
	label: string
	type: string
	placeholder?: string
	value: string | number
	isInvalid?: boolean
	feedback?: string | null
	options?: { value: string | number; label: string }[]
	onChange: (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => void
}

const FormField: React.FC<FormFieldProps> = ({
	label,
	type,
	placeholder = '',
	value,
	isInvalid = false,
	feedback = null,
	options,
	onChange,
}) => {
	return (
		<Form.Group className='mb-3'>
			<Form.Label>{label}</Form.Label>
			{type === 'select' && options ? (
				<Form.Select value={value} onChange={e => onChange(e)} isInvalid={isInvalid}>
					{options.map(option => (
						<option key={option.value} value={option.value}>
							{option.label}
						</option>
					))}
				</Form.Select>
			) : (
				<Form.Control
					type={type}
					placeholder={placeholder}
					value={value}
					onChange={e => onChange(e as React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>)}
					isInvalid={isInvalid}
				/>
			)}
			{feedback && <Form.Control.Feedback type='invalid'>{feedback}</Form.Control.Feedback>}
		</Form.Group>
	)
}

export default FormField
