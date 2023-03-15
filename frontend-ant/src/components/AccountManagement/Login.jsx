import React, {useState, useEffect} from "react";
import { NavLink } from 'react-router-dom';
import { Layout, Form, Button, Input, Typography, Alert, Card } from "antd";

const { Title } = Typography;

export const Login = () => {
 const [state, setState] = useState({showError: false, errorMsg: "", display: "login"})

 const handleFinish = (values) => {        
    //this.loginUser(values);
    console.log('handlefinish')
}

const handlePasswordReminder = (values) => {
    console.log('handlePasswordReminder')
}

return( 
    <Layout>
<Card className="login-container">
    {
        state.display === "login" &&
            <div>
                <Title>Login</Title>
                <Form onFinish={handleFinish} style={{ width: "100%" }}>

                    <Form.Item
                        name="username"
                        rules={[{ required: true, message: 'Enter a username' }]}>
                        <Input placeholder="username" />
                    </Form.Item> 

                    <Form.Item
                        name="password"
                        rules={[{ required: true, message: 'Enter a password' }]}>
                        <Input placeholder="password" type="password" />
                    </Form.Item>

                    <div className="reset-password text-right mt-3 mb-3">
                        <span className="pointer" onClick={() => setState({ display: "password"})}>Forgot Password</span>
                    </div>

                    <div className="button-row mt-2 mb-3 text-center">
                        <Button type="primary" htmlType="submit" className="btn-login" /*loading={this.context.loading}*/>
                            Login
                        </Button>              
                    </div>
                </Form>

                <div className="text-center">
                    <Typography.Text type="secondary">Don't have an account yet? &nbsp;</Typography.Text>
                    <NavLink to="/register" className="ant-btn">Sign Up</NavLink>
                </div>

            </div>
    }

    {
        state.display === "password" &&
        <div>
            <Title>Password Reset</Title>
            <Form onFinish={handlePasswordReminder} style={{ width: "100%" }}>
                <Form.Item
                    name="email"
                    rules={[{ type: 'email', message: 'Enter a Valid Email'},{ required: true, message: 'Enter an email' }]}
                    >
                    <Input placeholder="email" />
                </Form.Item>

                <div className="button-row mt-2">
                    <Button type="primary" htmlType="submit" block>
                        Reset Password
                    </Button>
                </div>

                <div className="reset-password text-right mt-3">
                    <span className="pointer" onClick={() => setState({ display: "login" })}>Back to Login</span>
                </div>
            </Form>          
        </div>

    }


    </Card>
</Layout>
);
        }



{/*
<Layout>
    <Card className="login-container">
    {   
        state.display === "login" &&
            <div>
                            <Title>Login</Title>
                            <Form onFinish={this.handleFinish} style={{ width: "100%" }}>

                                <Form.Item
                                    name="username"
                                    rules={[{ required: true, message: 'Enter a username' }]}
                                >
                                    <Input placeholder="username" />
                                </Form.Item>

                                <Form.Item
                                    name="password"
                                    rules={[{ required: true, message: 'Enter a password' }]}
                                >
                                    <Input placeholder="password" type="password" />
                                </Form.Item>
                                
                                <div className="reset-password text-right mt-3 mb-3">
                                    <span className="pointer" onClick={() => this.setState({ display: "password"})}>Forgot Password</span>
                                </div>
                                
                                <div className="button-row mt-2 mb-3 text-center">
                                    <Button type="primary" htmlType="submit" className="btn-login" loading={this.context.loading}>
                                        Login
                                    </Button>
                                </div>
                            </Form>

                            <div className="text-center">
                                <Typography.Text type="secondary">Don't have an account yet? &nbsp;</Typography.Text>
                                <NavLink to="/register" className="ant-btn">Sign Up</NavLink>
                            </div>
            </div>
    }

    {   
        state.display === "password" &&
            <div>
                        <Title>Password Reset</Title>
                        <Form onFinish={this.handlePasswordReminder} style={{ width: "100%" }}>

                            <Form.Item
                                name="email"
                                rules={[{ type: 'email', message: 'Enter a Valid Email'},{ required: true, message: 'Enter an email' }]}
                            >
                                <Input placeholder="email" />
                            </Form.Item>

                            <div className="button-row mt-2">
                                <Button type="primary" htmlType="submit" block>
                                    Reset Password
                                </Button>
                            </div>
                            
                            <div className="reset-password text-right mt-3">
                                <span className="pointer" onClick={() => this.setState({ display: "login" })}>Back to Login</span>
                            </div>
                        </Form>
        </div>
    }
        </Card>
    </Layout>*/
}
