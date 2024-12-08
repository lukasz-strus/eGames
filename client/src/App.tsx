import React from 'react'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import MainLayout from './app/layouts/MainLayout'
import StorePage from './app/features/game/store/pages/StorePage'
import LibraryPage from './app/features/library/pages/LibraryPage'
import 'bootstrap/dist/css/bootstrap.min.css'
import GamePage from './app/features/game/store/pages/GamePage'
import UserPage from './app/features/user/me/pages/ProfilePage'
import OrderPage from './app/features/order/pages/OrderPage'
import LibraryGamePage from './app/features/library/pages/LibraryGamePage'
import GamesManagmentPage from './app/features/game/admin/pages/GamesManagmentPage'
import GameFormPage from './app/features/game/admin/pages/GameFormPage'

const App: React.FC = () => {
	const htmlElement = document.querySelector('html')
	htmlElement?.setAttribute('data-bs-theme', 'dark')
	return (
		<Router>
			<MainLayout>
				<Routes>
					<Route path='/' element={<StorePage />} />
					<Route path='/library' element={<LibraryPage />} />
					<Route path='/library/:libraryGameId' element={<LibraryGamePage />} />
					<Route path='/game/:gameType/:gameId' element={<GamePage />} />
					<Route path='/profile' element={<UserPage />} />
					<Route path='/order' element={<OrderPage />} />
					<Route path='/games-managment' element={<GamesManagmentPage />} />
					<Route path='/game-form' element={<GameFormPage />} />
					<Route path='/game-form/:gameId' element={<GameFormPage />} />
				</Routes>
			</MainLayout>
		</Router>
	)
}

export default App
