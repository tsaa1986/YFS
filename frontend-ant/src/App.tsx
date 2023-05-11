import React, { useEffect, useState } from 'react';
import { Navigate, Routes, Route, 
  Link, useNavigate, useLocation, 
  NavLink, Outlet } from 'react-router-dom';
import { Login } from './components/AccountManagement/Login';
import {
  DesktopOutlined,
  FileOutlined,
  PieChartOutlined,
  TeamOutlined,
  UserOutlined,
} from '@ant-design/icons';
import type { MenuProps } from 'antd';
import { Breadcrumb, Layout, Menu, theme } from 'antd';
import { Register } from './components/AccountManagement/Register';
import { authAPI, UserAccountType } from './api/api';
import { ReportsLayout } from './components/Reports/ReportsLayout';
import { BudgetLayout } from './components/Budget/BudgetLayout';
import { DepositsLayout } from './components/Deposits/DepositsLayout';
import { AccountsLayout } from './components/Accounts/AccountsLayout';
import { HomeLayout } from './components/Home/HomeLayout';
import { HeaderLayout } from './components/Header/HeaderLayout';
import { WelcomeLayout } from './components/Welcome/WelcomeLayout';


const { Content, Footer, Sider } = Layout;

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

  
const SideMenu:React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);

  return(
    <Sider collapsible collapsed={collapsed} onCollapse={(value) => setCollapsed(value)}>
      <div style={{ height: 32, margin: 16, background: 'rgba(255, 255, 255, 0.2)' }} />
      <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline" items={items} />
    </Sider>
  );
}

const ProtectedRoute = ({ 
  isAllowed, 
  redirectPath = '/login', 
  children } : any) => {
  if (!isAllowed) {
      return <Navigate to={redirectPath} replace />;
    }

  return children ? children : <Outlet />;
};

const App: React.FC = () => {
  const navigate = useNavigate();
  const [collapsed, setCollapsed] = useState(false);
  //const { token: { colorBgContainer } } = theme.useToken();
  const [isLoggedIn, setisLoggedIn] = useState(false);
  
  const [user, setUser] = useState(() => {
    authAPI.me()?.then( res => { 
      console.log(res)
      setisLoggedIn(true)
    })
  });

  const handleLogin = () => 
    setUser( () => { 
      let res = authAPI.me()
      if (res === undefined) 
        return null
        else return res
    });

  const handleLogout = () => {
    authAPI.logOut();
    //setUser();
    setisLoggedIn(false);
  }

  console.log('loggedin: ' + isLoggedIn)
  console.log('RENDER')
 
  const selectedKey = useLocation().pathname

  useEffect(() => {
    //console.log('effect nav=' + isLoggedIn + ' key:' + selectedKey  )
      if(selectedKey === '/') {
          if (isLoggedIn) {
            navigate("/home");
          } else {
            navigate("/");
            return
          }
      } 
      if(selectedKey === '/login'){
        if (isLoggedIn){
          navigate("/home")
          return
        }
      }
  }, [navigate, isLoggedIn]);

  return (
  <div id="app-main" className="app-main">

    {/*{user ? (
        <button onClick={handleLogout}>Sign Out</button>
      ) : (
        <button onClick={handleLogin}>Sign In</button>
      )} */}

{/* переделать при логине устанавливать пользователя*/}
    <Routes>
    <Route path="/" element={<WelcomeLayout />} /> {/*//<Login setisLoggedIn={setisLoggedIn} />}/>*/}
      <Route path="/login" element={<Login setisLoggedIn={setisLoggedIn} loginDisplay="login"/>}/>
      <Route path="/register" element={<Register />}/>

      <Route element={
        <ProtectedRoute isAllowed={ isLoggedIn } />}>
          <Route path="/home" element={<MainLayout children={HomeLayout} />} />
          <Route path="/accounts" element={<MainLayout children={AccountsLayout} />} />
          <Route path="/budget" element={<MainLayout children={BudgetLayout} />} />
          <Route path="/reports" element={<MainLayout children={ReportsLayout} />} />
          <Route path="/deposits" element={<MainLayout children={DepositsLayout} />} 
        />
      </Route>
    </Routes>
  </div>
  );
};

const MainLayout: React.FC<any> = ( {children: Component, rest}: any) => {

  return(
    <Layout style={{ minHeight: '100vh' }}>
      <SideMenu />
      <Layout className="site-layout">
        <HeaderLayout isLoggedIn={true}/>
        {/*<Header style={{ padding: 0, background: "colorBgContainer" }} />*/}
        <Content style={{ margin: "24px 16px",
              padding: 24,
              background: "#fff",
              minHeight: 380 }}>
                <Component/>
        </Content>

        <Footer style={{ textAlign: 'center' }}>Ant Design ©2023 Created by <span style={{color: "red"}}>Ton@</span></Footer>
      </Layout>
    </Layout>
  )
}

export default App;
