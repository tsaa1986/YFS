import React from "react";
import { Layout } from 'antd';
//import { Header1 } from './components/Header/Header1';
import { Header } from "antd/es/layout/layout";
import { HeaderLayout } from "../Header/HeaderLayout";
import { Login } from "../AccountManagement/Login";

export const WelcomeLayout: React.FC = () => {
    return (
        <Layout>
            <HeaderLayout isLoggedIn={false}/>     
        </Layout>
    );
}