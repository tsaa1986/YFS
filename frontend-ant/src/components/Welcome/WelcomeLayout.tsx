import React from "react";
import { HeaderLayout } from "../Header/HeaderLayout";
import { IUser } from "../types/types";

type WelcomeTypeProps = {
    setLanguageDisplay: React.Dispatch<React.SetStateAction<string>>,
    languageDisplay: string,
    user: IUser | null
}

export const WelcomeLayout: React.FC<WelcomeTypeProps> = ({languageDisplay, setLanguageDisplay, user}) => {
    
    return (
    <div className="wrapper">
        <header className="header">
            <div className="header__container _container">
                <a href="" className="header__logo">
                    YFS
                </a>
            </div>
        </header>

        <main className="page">
            <div className="page__main-block main-block">
                <div className="main-block__container _container">
                    <div className="main-block__body">
                        <h1 className="main-block__title">
                            Your Financial Space
                        </h1>
                        <div className="main-block__text">
                            This is your money manager. 
                        </div>
                        <div className="main-block__buttons">
                            <a href="/login" className="main-block__button main-block__button_border">Sign in</a>
                            <a href="" className="main-block__button main-block__button_border">Demo Account</a>
                        </div>
                    </div>
                </div>
                <div className="main-block__image _ibg">
                    <img src="/background.jpg" alt="background" />
                </div>
            </div>
        </main>

        <footer className="footer">
        I am footer
        </footer>
    </div>);
    }
    //<HeaderLayout isLoggedIn={false} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} user={user}/>     