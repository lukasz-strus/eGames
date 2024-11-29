import React from 'react'
import { Alert, Container } from 'react-bootstrap'

const LibraryPage: React.FC = () => {
	return (
		<Container>
			<Alert variant='info' className='text-center'>
				This feature is available only for logged-in users.
			</Alert>
		</Container>
	)
}

export default LibraryPage
