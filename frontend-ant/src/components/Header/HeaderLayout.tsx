import { Avatar, Col, Dropdown, Layout, Menu, Row, Select } from 'antd';
import { Header as HeaderAntd } from 'antd/es/layout/layout';
import { NavLink } from 'react-router-dom';
import { MailOutlined, LogoutOutlined,
    UserOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined } from "@ant-design/icons";
import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { authAPI } from '../../api/api';
import { HeaderTypeProps } from '../types/types';
//import { ReactComponent as Img } from "./images/user.svg";
const { Option } = Select;

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
        <HeaderAntd className='header-main'> {/*//background: "colorBgContainer" }} />*/}                
            <Row
                align="middle"
            >                
                <Col flex={2} style={{ display: "flex", alignItems: "center"}}>   
                    <NavLink to={"/"} className="header-main-navarbar_container">
                        YFS
                     {/*<img src="/logo-$.png" alt="Logo" style={{ height: 40}}/>*/}
                    </NavLink>
                     <Select
                        defaultValue={languageDisplay}
                        //style={{ color: 'gold', padding: 5, width: 80 }}    
                        className="header-main-select"                    
                        disabled={false}
                        onChange={(value) => {
                        // Handle language change
                        console.log("Selected language:", value);
                        setLanguageDisplay(value);
                        }}
                    >
                        <Option id="en" value="en">ENG</Option>
                        <Option id="ua" value="ua">УКР</Option>
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