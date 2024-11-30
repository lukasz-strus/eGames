import { Image, Navbar } from 'react-bootstrap'
import brandImg from '../../../assets/egames-logo.png'

const Brand: React.FC = () => (
	<Navbar.Brand href='/'>
		<Image src={brandImg} alt='eGames' width='90px' />
	</Navbar.Brand>
)

export default Brand
