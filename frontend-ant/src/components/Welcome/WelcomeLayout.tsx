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
       <HeaderLayout isLoggedIn={false} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} user={user}/>     
    );
}