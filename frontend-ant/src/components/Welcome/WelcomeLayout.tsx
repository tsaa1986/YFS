import React from "react";
import { Layout } from 'antd';
//import { Header1 } from './components/Header/Header1';
import { Header } from "antd/es/layout/layout";

export const WelcomeLayout: React.FC = () => {
    return (
        <Layout>
            <Header style={{ padding: 0, background: "colorBgContainer" }} />
            <h4 > Welcome to Your Financial Space </h4>
        </Layout>
    );
}