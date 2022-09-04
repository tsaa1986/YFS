import React from 'react';
import s from './SignIn.module.css';

const SignInForm = (props) => {
	return (
			<form className={s.form}>
				<div>
					<input placeholder={"Login"} />
				</div>
				<div>
					<input placeholder={"password"} />
				</div>
				<div>
					<input type={"checkbox"} /> remember me
				</div>
				<div>
					<button>Login</button>
				</div>
			</form>
	)
}

const SignIn = (props) => {
	return <div>
		<h1>Login</h1>
		<SignInForm />
		</div>
}
export default SignIn;
