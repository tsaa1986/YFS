import './App.css';
import logo from './logo.svg';
import { BrowserRouter, NavLink, Route } from "react-router-dom";
import Test from './component/Test';

const App = (_props) => {
	return (
		<div className="App">
			<header className="App-header">
				<img src={logo} className="App-logo" alt="logo" />
				<p>
					Edit <code>src/App.js</code> YFS.
				</p>
				<BrowserRouter>
					<div>
						<Route path="/Test" component={Test} />
					</div>
					<nav className="nav">
						<div className="test">
							<NavLink to="/Test">Test</NavLink>
						</div>
					</nav>
				</BrowserRouter >

			</header>
		</div>

	);
}

export default App;
