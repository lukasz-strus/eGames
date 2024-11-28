import { Card } from 'react-bootstrap'

interface GameImageProps {
	src: string
	alt: string
}

const GameImage: React.FC<GameImageProps> = ({ src, alt }) => {
	return (
		<Card className='game-img-card'>
			<Card.Img variant='top' src={src} alt={alt} className='game-img' />
		</Card>
	)
}

export default GameImage
