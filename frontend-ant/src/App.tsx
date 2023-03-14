import React, { useEffect, useState } from 'react';
import { Navigate, Routes, Route, Link, useNavigate,useLocation } from 'react-router-dom';
import { Login } from './components/AccountManagement/Login';
import { Main } from './components/MainLayout/Main';
import {
  DesktopOutlined,
  FileOutlined,
  PieChartOutlined,
  TeamOutlined,
  UserOutlined,
} from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { Breadcrumb, Layout, Menu, theme } from 'antd';

const { Header, Content, Footer, Sider } = Layout;

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
  getItem("/", <Link to={"/"}>{'Home Page'}</Link>, '1', <PieChartOutlined />),
  getItem("/accounts", <Link to={"/accounts"}>{'Accounts'}</Link>, '2', <DesktopOutlined />),
  getItem("/budget",<Link to={"/budget"}>{'Budget'}</Link>, 'sub1', <UserOutlined />, ),
  getItem("/reports",<Link to={"/reports"}>{'Reports'}</Link>, '10', <FileOutlined />),
  getItem("/deposits",<Link to={"/deposits"}>{'Deposits'}</Link>, '9', <DesktopOutlined />),
  getItem("/settings",<Link to={"/settings"}>{'Settings'}</Link>, 'sub2', <TeamOutlined />, 
    [ getItem("/settings/accountGroup",<Link to={"/settings/accountGroup"}>{'Account Group'}</Link>, '6'), 
      getItem("/settings/accounts",<Link to={"/settings/accounts"}>{'Accounts'}</Link>, '8'), 
      getItem("/settings/currency",<Link to={"/settings/currency"}>{'Currency'}</Link>, '11')
    ]),
];

  
const SideMenu:React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);

  return(
    <Sider collapsible collapsed={collapsed} onCollapse={(value) => setCollapsed(value)}>
      <div style={{ height: 32, margin: 16, background: 'rgba(255, 255, 255, 0.2)' }} />
      <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline" items={items} />
    </Sider>
  );
}

const Budget = () => {
  return <h4> Budget </h4>
 }
const Accounts = () => {
  return <h4> Accounts</h4>
 }



const App: React.FC = () => {
  const navigate = useNavigate();
  const [collapsed, setCollapsed] = useState(false);
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  const [isLoggedIn, setisLoggedIn] = useState(false);

  const selectedKey = useLocation().pathname

  useEffect(() => {
    // Checking if user is not loggedIn
      if(selectedKey === '/') {
          if (isLoggedIn) {
            navigate("/");
          } else {
            navigate("/login");
          }
    }
  }, [navigate, isLoggedIn]);


  return (
    <Layout style={{ minHeight: '100vh' }}>
        <SideMenu />

      <Layout className="site-layout">
        <Header style={{ padding: 0, background: "colorBgContainer" }} />
        <Content style={{ margin: "24px 16px",
              padding: 24,
              background: "#fff",
              minHeight: 280 }}>
    <Routes>
      <Route path="/accounts" element={<Accounts />} />
      <Route path="/budget" element={<Budget />} />
      <Route path="/settings/accountGroup" element={<div>accountGroups configuration</div> } />
    </Routes>
        </Content>

        <Footer style={{ textAlign: 'center' }}>Ant Design ©2023 Created by Ant UED</Footer>
        </Layout>
    </Layout>

  );
};


export default App;
