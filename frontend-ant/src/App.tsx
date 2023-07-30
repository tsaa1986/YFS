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
import { authAPI } from './api/api';
import { ReportsLayout } from './components/Reports/ReportsLayout';
import { BudgetLayout } from './components/Budget/BudgetLayout';
import { DepositsLayout } from './components/Deposits/DepositsLayout';
import { AccountsLayout } from './components/Accounts/AccountsLayout';
import { HomeLayout } from './components/Home/HomeLayout';
import { HeaderLayout } from './components/Header/HeaderLayout';
import { WelcomeLayout } from './components/Welcome/WelcomeLayout';
import { Footer } from 'antd/es/layout/layout';
import { IUser } from './components/types/types';
import "./sass/index.scss";


//const { Content, Footer, Sider } = Layout;
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
  backgroundColor: 'lightgray',
};
  
const SideMenu:React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);

  return(
    <Sider style={siderStyle} collapsible={false} collapsed={collapsed} onCollapse={(value) => setCollapsed(value)}>
      {/*<div style={{ height: 32, margin: 16, background: 'rgba(255, 255, 255, 0.2)' }} />*/}
      <Menu defaultSelectedKeys={['1']} mode="inline" items={items} />
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
  const [languageDisplay, setLanguageDisplay] = useState("en")
  const [user, setUser] = useState<IUser | null>(null);

  const handleLogin = () => {
    authAPI.me()?.then((res) => {
        if (res !== undefined) {
          setUser(res);
        } else {
          setUser(null);
          console.error('No user data received.');
        }})
      .catch((error) => {
        console.error('No user data received.');
        setUser(null);
      })
    };   

  useEffect(() => {
    authAPI.me()?.then((res) => {
      setUser(res)
      setisLoggedIn(true)
    }).catch((error) => {
      console.error('Error fetching user data:', error);
    })
  }, [])

  useEffect(()=>{
    if (isLoggedIn === true) {
      handleLogin();
    }
  },[isLoggedIn]) 

  const handleLogout = () => {
    authAPI.logOut();
    setUser(null);
    setisLoggedIn(false);
  }
  console.log('loggedin: ',isLoggedIn)
  console.log('RENDER')
 
  const selectedKey = useLocation().pathname

  useEffect(() => {
    console.log("change language to " + languageDisplay);
  }, [languageDisplay])

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
  <div className="app-main">
    {/*{user ? (
        <button onClick={handleLogout}>Sign Out</button>
      ) : (
        <button onClick={handleLogin}>Sign In</button>
      )} */}

{/* переделать при логине устанавливать пользователя*/}
    <Routes>
    <Route path="/" element={<WelcomeLayout languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} user={user}/>} /> {/*//<Login setisLoggedIn={setisLoggedIn} />}/>*/}
      <Route path="/login" element={<Login setisLoggedIn={setisLoggedIn} loginDisplay="login" languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay}/>}/>
      <Route path="/register" element={<Register />}/>

      <Route element={
        <ProtectedRoute isAllowed={ isLoggedIn } />}>
          <Route path="/home" element={<MainLayout children={HomeLayout} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} isLoggedIn={isLoggedIn} user={user}/>} />
          <Route path="/accounts" element={<MainLayout children={AccountsLayout} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} isLoggedIn={isLoggedIn} user={user}/>} />
          <Route path="/budget" element={<MainLayout children={BudgetLayout} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} isLoggedIn={isLoggedIn} user={user}/>} />
          <Route path="/reports" element={<MainLayout children={ReportsLayout} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} isLoggedIn={isLoggedIn} user={user}/>} />
          <Route path="/deposits" element={<MainLayout children={DepositsLayout} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} isLoggedIn={isLoggedIn} user={user}/>} 
        />
      </Route>
    </Routes>
  </div>
  );
};

const MainLayout: React.FC<any> = ( {children: Component, languageDisplay, setLanguageDisplay, isLoggedIn, user}) => {

  return(
<div className="app-main"> 
<HeaderLayout isLoggedIn={isLoggedIn} languageDisplay={languageDisplay} setLanguageDisplay={setLanguageDisplay} user={user} />
    <Layout >
      <Layout className="site-layout">
      <SideMenu />
        {/*<Header style={{ padding: 0, background: "colorBgContainer" }} />*/}
        <Content style={{ margin: "20px 16px",
              padding: 20,
              background: "#fff"
              }}>
                <Component/>
        </Content>
      </Layout>
    </Layout>
    <Footer style={{background: 'lightyellow', textAlign: 'center' }}>Ant Design ©2023 Created by <span style={{color: "red"}}>Ton@</span></Footer>
    </div>
  )
}

export default App;
