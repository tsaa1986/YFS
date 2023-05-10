import { Avatar, Col, Dropdown, Layout, Menu, Row } from 'antd';
import { Header as HeaderAntd } from 'antd/es/layout/layout';
import { NavLink } from 'react-router-dom';
import { MailOutlined, LogoutOutlined,
    UserOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined } from "@ant-design/icons";
//import { ReactComponent as Img } from "./images/user.svg";

export const HeaderLayout: React.FC = () => {

    let userMenu = (
        <Menu>
            <Menu.Item>         
                <a className="dropdown-item text-muted" href="/" /*onClick={userSignOut}*/>      
                    <LogoutOutlined className="mr-1" />
                    Logout
                </a>
            </Menu.Item>      
        </Menu>
    )
    let thumbnail = <Avatar size={50} icon={<UserOutlined />} />
    
    return(
    <Layout>
        <HeaderAntd 
            style={{ 
                color: "white", 
                padding: 0, 
                height: 50,
                paddingBottom: 1,

                width: "100%" }}> {/*//background: "colorBgContainer" }} />*/}
            <Row
                align="middle"
                style={{
                    /*height: 25,
                    border: "1px solid white"*/
                }}
            >                
                <Col flex={2}>   
                     <NavLink to={"/"} className="navbar-logo-container">
                        Logo Here
                     </NavLink>
                </Col>
                <Col flex={2} style={{color: "red", fontSize: "26px", fontFamily: "cursive"}}>
                 Your Financial Space
                </Col>
                <Col>
                 Events <MailOutlined />
                </Col>
                <Col style={{ textAlign: "right", marginLeft: 25 }}>
                    Welcome UserName
                </Col>
                <Col>
                <div
                    style={{
                    height: 40,
                    width: 40,
                    backgroundColor: "#545B64",
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    borderRadius: "50%",
                    marginLeft: 14
                    }}
                >
                    {/*<Img style={{ height: 19, width: 16 }} />*/}
                    <span
                            className="trigger"                            
                            //onClick={this.toggle}
                        >
                        {
                            //this.state.collapsed &&
                            //    <MenuUnfoldOutlined className="anticon-menu" />
                        }
                        {
                            //!this.state.collapsed &&
                                <MenuFoldOutlined className="anticon-menu" />
                        }
                        </span>
                        <div className="ml-auto mr-0">                            
                            <Dropdown
                                trigger={['click']}
                                overlay={userMenu}>
                                {thumbnail}
                            </Dropdown>
                        </div>
              </div>
            </Col>

            </Row>
        


        </HeaderAntd>
    </Layout>
    )
}