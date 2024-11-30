import { Nav } from 'react-bootstrap'
import { useLocation } from 'react-router-dom'

const NavigationLinks: React.FC = () => {
	const location = useLocation()

	const getNavLinkClass = (path: string): string => {
		return location.pathname === path ? 'fw-bold' : ''
	}

	return (
		<Nav className='me-auto'>
			<Nav.Link href='/' className={getNavLinkClass('/')}>
				Store
			</Nav.Link>
			<Nav.Link href='/library' className={getNavLinkClass('/library')}>
				Library
			</Nav.Link>
		</Nav>
	)
}

export default NavigationLinks
