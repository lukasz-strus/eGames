import React from 'react'
import { Form } from 'react-bootstrap'

interface FormFieldProps {
	label: string
	type: string
	placeholder: string
	value: string
	isInvalid?: boolean
	feedback?: string | null
	onChange: (e: React.ChangeEvent<HTMLInputElement>) => void
}

const FormField: React.FC<FormFieldProps> = ({ label, type, placeholder, value, isInvalid, feedback, onChange }) => {
	return (
		<Form.Group className='mb-3'>
			<Form.Label>{label}</Form.Label>
			<Form.Control type={type} placeholder={placeholder} value={value} onChange={onChange} isInvalid={isInvalid} />
			{feedback && <Form.Control.Feedback type='invalid'>{feedback}</Form.Control.Feedback>}
		</Form.Group>
	)
}

export default FormField
