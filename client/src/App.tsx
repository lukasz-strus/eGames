import React from 'react'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import MainLayout from './components/MainLayout'
import HomePage from './pages/HomePage'
import LibraryPage from './pages/LibraryPage'

//import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css'

const App: React.FC = () => {
	const htmlElement = document.querySelector('html')
	htmlElement?.setAttribute('data-bs-theme', 'dark') // TODDO: to toggle functionality
	return (
		<Router>
			<MainLayout>
				<Routes>
					<Route path='/' element={<HomePage />} />
					<Route path='/library' element={<LibraryPage />} />
				</Routes>
			</MainLayout>
		</Router>
	)
}

export default App
