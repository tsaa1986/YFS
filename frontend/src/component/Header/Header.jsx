import React from 'react';
import s from './Header.module.css';
import { NavLink } from 'react-router-dom';

const Header = () => {
	return (
		<header className={s.header}>
			<NavLink to="/app" activeClassName={s.activeLink}>Your Financial Space</NavLink>
			<nav className={s.main_nav}>
			<div className={s.site_nav}>
				<NavLink to="/app" activeClassName={s.activeLink}>Home</NavLink>
				<NavLink to="/About" activeClassName={s.activeLink}>About</NavLink>
				<NavLink to="/Contact" activeClassName={s.activeLink}>Contact</NavLink>
			</div>
			<div className={s.user_nav}>
				<NavLink to="/SignIn" activeClassName={s.activeLink}>Sign In</NavLink>
				<NavLink to="/CreateAccount" activeClassName={s.activeLink}>Create Account</NavLink>
			</div>
		</nav>
		</header>
	);
}

export default Header;
