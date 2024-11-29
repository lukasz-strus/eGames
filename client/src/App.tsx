import React from 'react'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import MainLayout from './app/layouts/MainLayout'
import StorePage from './app/features/game/store/StorePage'
import LibraryPage from './app/features/library/LibraryPage'
import 'bootstrap/dist/css/bootstrap.min.css'
import GamePage from './app/features/game/store/GamePage'

const App: React.FC = () => {
	const htmlElement = document.querySelector('html')
	htmlElement?.setAttribute('data-bs-theme', 'dark') // TODDO: to toggle functionality
	return (
		<Router>
			<MainLayout>
				<Routes>
					<Route path='/' element={<StorePage />} />
					<Route path='/library' element={<LibraryPage />} />
					<Route path='/game/:gameType/:gameId' element={<GamePage />} />
				</Routes>
			</MainLayout>
		</Router>
	)
}

export default App
