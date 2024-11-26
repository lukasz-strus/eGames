import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css'

import { Button } from 'react-bootstrap'

function App() {
	const htmlElement = document.querySelector('html')
	htmlElement?.setAttribute('data-bs-theme', 'dark') // TODDO: to toggle functionality

	return (
		<main>
			<Button variant='secondary'>Click me</Button>
		</main>
	)
}

export default App
