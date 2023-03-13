import React, { useState } from 'react';
import { useLocation, useNavigate, Routes, Route, Link } from 'react-router-dom';
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

const Page1 = () => {
  return <h4> Page 1</h4>
 }
const Page2 = () => {
  return <h4> Page 2</h4>
 }

const App: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  return (
      <Layout style={{ minHeight: '100vh' }}>
        <Sider collapsible collapsed={collapsed} onCollapse={(value) => setCollapsed(value)}>
          <div style={{ height: 32, margin: 16, background: 'rgba(255, 255, 255, 0.2)' }} />
          <Menu theme="dark" defaultSelectedKeys={['1']} mode="inline" items={items} />
        </Sider>
        <Layout className="site-layout">
          <Header style={{ padding: 0, background: colorBgContainer }} />
          <Content style={{ margin: '0 16px' }}>
            {/*<Breadcrumb style={{ margin: '16px 0' }}>
              <Breadcrumb.Item>User</Breadcrumb.Item>
              <Breadcrumb.Item>Bill</Breadcrumb.Item>
              </Breadcrumb>*/}
            <div style={{ padding: 24, minHeight: 360, background: colorBgContainer }}>
            <Routes>
              <Route path="/" element={<Page1 />} />
              <Route path="/page2" element={<Page2 />} />
            </Routes>
            </div>
          </Content>
          <Footer style={{ textAlign: 'center' }}>Ant Design ©2023 Created by Ant UED</Footer>
        </Layout>
      </Layout>
  );
};


export default App;
