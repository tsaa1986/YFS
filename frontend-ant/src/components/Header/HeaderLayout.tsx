import { Avatar, Col, Dropdown, Layout, Menu, Row, Select } from 'antd';
import { Header as HeaderAntd } from 'antd/es/layout/layout';
import { NavLink } from 'react-router-dom';
import { MailOutlined, LogoutOutlined,
    UserOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined } from "@ant-design/icons";
import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { authAPI } from '../../api/api';
import { IUser } from '../types/types';
//import { ReactComponent as Img } from "./images/user.svg";
const { Option } = Select;

type HeaderTypeProps = {
    isLoggedIn: Boolean,
    languageDisplay: String,
    setLanguageDisplay: Dispatch<SetStateAction<any>>
    user: IUser | null;
    //setisLoggedIn: Dispatch<SetStateAction<any>>
}

export const HeaderLayout: React.FC<HeaderTypeProps> = ({isLoggedIn, languageDisplay, setLanguageDisplay, user}) => {
    const [collapsed, setCollapsed] = useState<Boolean>(false)
    const [drawerVisible, setDrawerVisible] = useState<Boolean>(false)

    const handleLogout = () => {
        authAPI.logOut();
        //setUser();
        //setisLoggedIn(false);
    }

    useEffect(()=>{
        console.log('user was changed', user);
    }, [user])

    let userMenu = (
        <Menu>
            <Menu.Item>         
                <a className="dropdown-item text-muted" href="/" onClick={handleLogout}>      
                    <LogoutOutlined className="mr-1" />
                    Logout
                </a>
            </Menu.Item>      
        </Menu>
    )
    let thumbnail = <Avatar size={50} icon={<UserOutlined />} />

  
    return(
        <HeaderAntd 
           style={{
                background: 'lightyellow',           
                padding: 0, 
                paddingBottom: 0,
                width: "100%" }}> {/*//background: "colorBgContainer" }} />*/}
                
            <Row
                align="middle"
            >                
                <Col flex={2} style={{ display: "flex", alignItems: "center" }}>   
                      <NavLink to={"/"} className="navbar-logo-container">
                     <img src="/logo-$.png" alt="Logo" style={{ height: 40}}/>
                     </NavLink>
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
                </Col>

                <Col>   
                   {!isLoggedIn && <NavLink style={{
                        color:"red"                        
                        }} to={"/login"} className="navbar-logo-container">
                        Login to
                     </NavLink>
                    }
                </Col>

                <Col flex={2} style={{color: "red", fontSize: "26px", fontFamily: "cursive"}}>
                 Your Financial Space
                </Col>

                {/*<Col>
                 Events <MailOutlined />
                </Col>*/}

                <Col style={{ textAlign: "right", paddingRight: 10 }}>
                    { isLoggedIn && (user !== undefined) && (user !== null) ? (<div style={{fontWeight: "bold"}}>Welcome: {user.userName}</div>) : <div></div> }
                </Col>

                {/*<Col
                span={1}
                style={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                height: 46
                }}
                 >test</Col>*/}
                {
                    
                    isLoggedIn && 
                    <div style={{marginLeft: "10"}}> {/*className="ml-auto mr-0">*/}                            
                            <Dropdown
                                trigger={['click']}
                                overlay={userMenu}>
                                {thumbnail}
                            </Dropdown>
                    </div>
                }
            </Row>        
        </HeaderAntd>
    )
}