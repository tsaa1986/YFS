import React, { useEffect, useState } from 'react';
import { Navigate, Routes, Route, 
  Link } from 'react-router-dom';
import {
  DesktopOutlined,
  FileOutlined,
  PieChartOutlined,
  TeamOutlined,
  UserOutlined,
} from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { Breadcrumb, Layout, Menu, theme } from 'antd';
import { Footer } from 'antd/es/layout/layout';
import { HeaderLayout } from './Header/HeaderLayout';

const { Content, Sider } = Layout;

type MenuItem = Required<MenuProps>['items'][number];


function getItem(
    path: string,
    label: React.ReactNode,
    key: React.Key,
    icon?: React.ReactNode,
    children?: MenuItem[],
  ): MenuItem {
    return {
      path,
      key,
      icon,
      children,
      label,
    } as MenuItem;
  }

const items: MenuItem[] = [
  getItem("/", <Link to={"/home"}>{'Home Page'}</Link>, '1', <PieChartOutlined />),
  getItem("/accounts", <Link to={"/accounts"}>{'Accounts'}</Link>, '2', <DesktopOutlined />),
  getItem("/deposits",<Link to={"/deposits"}>{'Deposits'}</Link>, '3', <DesktopOutlined />),
  getItem("/budget",<Link to={"/budget"}>{'Budget'}</Link>, '4', <UserOutlined />, ),
  getItem("/reports",<Link to={"/reports"}>{'Reports'}</Link>, '5', <FileOutlined />),
  getItem("/settings",<Link to={"/settings"}>{'Settings'}</Link>, '10', <TeamOutlined />, 
    [ getItem("/settings/accountGroup",<Link to={"/settings/accountGroup"}>{'Account Group'}</Link>, 'sub1'), 
      getItem("/settings/accounts",<Link to={"/settings/accounts"}>{'Accounts'}</Link>, 'sub2'), 
      getItem("/settings/currency",<Link to={"/settings/currency"}>{'Currency'}</Link>, 'sub3')
    ]),
];

const siderStyle: React.CSSProperties = {
    textAlign: 'center',
    lineHeight: '120px',
    color: '#fff',
    backgroundColor: '#001529'
  };
const menuStyle = {
    backgroundColor: '#001529', 
    color: '#fff', 

  };

const SideMenu:React.FC = () => {
    const [collapsed, setCollapsed] = useState(false);
  
    return(
      <Sider style={siderStyle} collapsible={false} collapsed={collapsed} onCollapse={(value) => setCollapsed(value)}>
        {/*<div style={{ height: 32, margin: 16, background: 'rgba(255, 255, 255, 0.2)' }} />*/}
        <Menu style={menuStyle} defaultSelectedKeys={['1']} mode="inline" items={items} />
      </Sider>
    );
  }

const MainLayout: React.FC<any> = ( {children: Component, languageDisplay, setLanguageDisplay, isLoggedIn, user}) => {

return(
  <Layout>
  <HeaderLayout isLoggedIn={isLoggedIn} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} user={user} />
        <Layout style={{overflowY: 'auto'}}>
        <SideMenu />
          <Content style={{ margin: "20px 16px",
                padding: 20,
                background: "#fff",
                overflowY: 'auto'
                }}>
                  <Component/>
          </Content>          
        </Layout>
        <Footer className='footer'>Ant Design ©2023 Created by <span style={{color: "red"}}>Ton@</span></Footer>
</Layout>
    )
  }

export default MainLayout;