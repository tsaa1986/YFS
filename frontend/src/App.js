import React from 'react';
import { BrowserRouter, Route } from 'react-router-dom';
import './App.css';
import Header from './component/Header/Header';
import Footer from './component/Footer/Footer';
import SignIn from './component/Navbar/SignIn/SignIn';

const App = (props) => {
	return (
		<BrowserRouter>
			<div className='app_wrapper'>
				<div className='ground'>
				<Header />
				<Route path='/Home' render={ ()=> <App />} />
				{/* <Route path='/About' render={ ()=> <About />} />
				<Route path='/Contact' render={ ()=> <Contact />} /> */}
				<Route path='/SignIn' render={ ()=> <SignIn />} />
				{/* <Route path='/CreateAccount' render={ ()=> <CreateAccount />} /> */}
				<main className="main">
					<h1>Financial Fitness Journey</h1>
				</main>
				</div>
				<Footer />
			</div>

		</BrowserRouter>
	);
}

export default App;
