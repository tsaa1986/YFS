import { Avatar, Col, Dropdown, Layout, Menu, Row } from 'antd';
import { Header as HeaderAntd } from 'antd/es/layout/layout';
import { NavLink } from 'react-router-dom';
import { MailOutlined, LogoutOutlined,
    UserOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined } from "@ant-design/icons";
import { Dispatch, SetStateAction, useState } from 'react';
import { authAPI } from '../../api/api';
//import { ReactComponent as Img } from "./images/user.svg";

type HeaderTypeProps = {
    isLoggedIn: Boolean
    //setisLoggedIn: Dispatch<SetStateAction<any>>
}

export const HeaderLayout: React.FC<HeaderTypeProps> = ({isLoggedIn}) => {
    const [collapsed, setCollapsed] = useState<Boolean>(false)
    const [drawerVisible, setDrawerVisible] = useState<Boolean>(false)

    const handleLogout = () => {
        authAPI.logOut();
        //setUser();
        //setisLoggedIn(false);
    }

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
                padding: 0, 
                height: 50,
                paddingBottom: 0,
                width: "100%" }}> {/*//background: "colorBgContainer" }} />*/}
                
            <Row
                align="middle"
            >                
                <Col flex={2}>   
                     <NavLink to={"/"} className="navbar-logo-container">
                        Logo Here
                     </NavLink>
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
                    { isLoggedIn && <div>Welcome UserName</div> }
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