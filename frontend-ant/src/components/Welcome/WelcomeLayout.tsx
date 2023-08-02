import { Button, Typography } from "antd";
import { Content, Footer } from "antd/es/layout/layout";
import React, { useState} from "react";
import { useNavigate } from "react-router-dom";
import { authAPI } from "../../api/api";
import { Login } from "../AccountManagement/Login";
import { HeaderWelcomePage } from "../Header/HeaderWelcomePage";
import { IUser } from "../types/types";

export type StateTypeProps = {
    showError: Boolean | null
    errorMsg: string | null
    display: string | null
}

type handleFinishTypeProps = {
    username: "string"
    password: "string"
}

type WelcomeTypeProps = {
    setLanguageDisplay: React.Dispatch<React.SetStateAction<string>>,
    setIsLoggedIn: React.Dispatch<React.SetStateAction<boolean>>;
    languageDisplay: string,
    user: IUser | null
}

export const WelcomeLayout: React.FC<WelcomeTypeProps> = ({setIsLoggedIn, languageDisplay, setLanguageDisplay, user}) => {
   const[loginDisplay, setLoginDisplay] = useState("hide");
   const navigate = useNavigate();

   const logInDemoUser = () => {
    authAPI.login("demo", "123$qweR").then(
        res => { 
            if (res != false) {
                setIsLoggedIn(true);
                navigate("/home");
            } 
        }
    )
    }

    return (
    <div className="wrapper">
        <HeaderWelcomePage isLoggedIn={false} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} user={user}/>

        <main className="page">
            <div className="page__main-block main-block">
                <div className="main-block__container _container">
                    <div className="main-block__body">
                        <h1 className="main-block__title">
                            Your Financial Space
                        </h1>
                        <div className="main-block__text">
                            This is your financial manager. 
                        </div>
                        <div className="main-block__buttons">
                            <Button className="main-block__button main-block__button_border" onClick={()=>{setLoginDisplay("login"); console.log(loginDisplay)}}>Sign in </Button>
                            <Button className="main-block__button main-block__button_border" onClick={()=>{ logInDemoUser(); }}>Demo Account </Button>
                        </div>
                    </div>
                    <div className="main-block__login">
                        {
                            <Login  setIsLoggedIn={setIsLoggedIn} 
                                    loginDisplay={loginDisplay}
                                    setLoginDisplay={setLoginDisplay}
                                    languageDisplay={languageDisplay}
                                    setLanguageDisplay={setLanguageDisplay}
                                    />
                        //Component && <Component />
                        }
                    </div>
                </div>
                <div className="main-block__image _ibg">
                    <img src="/background.jpg" alt="background" />
                </div>
            </div>
        </main>
        <Footer className="footer">Ant Design ©2023 Created by <span style={{color: "red"}}>Ton@</span></Footer>
    </div>);
    }