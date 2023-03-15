import React, { useEffect, useState } from 'react';
import { Navigate, Routes, Route, Link, useNavigate,useLocation, NavLink } from 'react-router-dom';
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

  const Accounts = () => {
    return <h4> Accounts</h4>
   }
  

const items: MenuItem[] = [
  getItem("/", <NavLink to={"/"}>{'Home Page'}</NavLink>, '1', <PieChartOutlined />),
  getItem("/accounts", <NavLink to={"/accounts"}>{<Accounts />}</NavLink>, '2', <DesktopOutlined />),
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

 const HomePage = () => {
  return <h4> HomePage </h4>
 }
 const Reports = () => {
  return <h4> Reports </h4>
 }

 const RouteApp = ({ component: Component, rest }: any) => {
  return (
    <Route
      {...rest}
      render={(routeProps:any) => (
        <Layout>
          <Component {...routeProps} />
        </Layout>
      )}
    />
  );
};




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
  <Routes>
    <Route path="/login" element={<Login />}/>

    <Route path="/" element={<MainLayout children={HomePage} />} />
    <Route path="/accounts" element={<MainLayout children={Accounts} />} />
    <Route path="/budget" element={<MainLayout children={Budget} />} />
    <Route path="/reports" element={<MainLayout children={Reports} />} />
  </Routes>
  );
};

/*
const RestrictedRoute = ({component: Component, authUser, ...rest}:any) =>
    <Route
        {...rest}
        render={props  =>
            authUser
                ? <Component {...props} />
                : <Redirect
                    to={{
                        pathname: '/login',
                        state: {from: props.location}
                    }}
                />}
    />
*/

const MainLayout: React.FC<any> = ( {children: Component, rest}: any) => {

  return(
    <Layout style={{ minHeight: '100vh' }}>
        <SideMenu />

      <Layout className="site-layout">
        <Header style={{ padding: 0, background: "colorBgContainer" }} />
        <Content style={{ margin: "24px 16px",
              padding: 24,
              background: "#fff",
              minHeight: 280 }}>
                <Component/>

        </Content>

        <Footer style={{ textAlign: 'center' }}>Ant Design ©2023 Created by Ant UED</Footer>
        </Layout>
    </Layout>
  )
}


export default App;
