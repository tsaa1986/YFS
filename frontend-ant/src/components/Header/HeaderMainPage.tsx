import { Avatar, Col, Dropdown, Layout, Menu, Row, Select } from 'antd';
import { MailOutlined, LogoutOutlined,
    UserOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined } from "@ant-design/icons";
import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { HeaderTypeProps, IUser } from '../types/types';

//import { ReactComponent as Img } from "./images/user.svg";
const { Option } = Select;


export const HeaderMainPage: React.FC<HeaderTypeProps> = ({isLoggedIn, languageDisplay, setLanguageDisplay, user}) => {
    useEffect(()=>{
        console.log('user was changed', user);
    }, [user])

    return(
        <header className="header">
            <div className="header__container _container">
                <a href="" className="header__logo">
                    YFS
                </a>
                <div className="header__language">
                <Select
                        defaultValue={languageDisplay} // Set the default language value here
                        style={{ width: 80 }}    
                        className="custom-select"                    
                        disabled={true}
                        onChange={(value) => {
                        // Handle language change
                        console.log("Selected language:", value);
                        setLanguageDisplay(value);
                        // Implement your logic to change the language here
                        }}
                    >
                        <Option id="en" value="en">ENG</Option>
                        <Option id="ua" value="ua">УКР</Option>
                        {/* Add more language options as needed */}
                    </Select>
                </div>

            </div>
            <div className="header__container _language">

            </div>
        </header>
    )
}