import React from "react";
import { HeaderLayout } from "../Header/HeaderLayout";

type WelcomeTypeProps = {
    setLanguageDisplay: React.Dispatch<React.SetStateAction<string>>,
    languageDisplay: string,
}

export const WelcomeLayout: React.FC<WelcomeTypeProps> = ({languageDisplay, setLanguageDisplay}) => {
    return (
       <HeaderLayout isLoggedIn={false} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay}/>     
    );
}