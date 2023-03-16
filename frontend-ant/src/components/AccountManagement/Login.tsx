import React, {useState, useEffect} from "react";
import { NavLink,redirect,useNavigate} from 'react-router-dom';
import { Layout, Form, Button, Input, Typography, Alert, Card } from "antd";
import { authAPI } from "../../api/api";

const { Title } = Typography;

export type StateTypeProps = {
    showError: Boolean | null
    errorMsg: string | null
    display: string | null
}

type handleFinishTypeProps = {
    username: "string"
    password: "string"
}

export const Login: React.FC<any> = ({setisLoggedIn}) => {
 const navigate = useNavigate();
 const [state, setState] = useState<StateTypeProps>({showError: false, errorMsg: "", display: "login"})

 const handleFinish = (values: handleFinishTypeProps) => {        
    //this.loginUser(values);
    console.log(values);
    authAPI.login(values.username, values.password).then(
        res => { 
            if (res != false) {
                console.log(res)
                setisLoggedIn(true);
                navigate("/");
            } else console.log(res);
        }
    )
    console.log('handlefinish');
}

const handlePasswordReminder = (values:any) => {
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
                        <span className="pointer" onClick={() => setState({ showError: null, errorMsg: "", display: "password"})}>Forgot Password</span>
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
                    <span className="pointer" onClick={() => setState({ showError: null, errorMsg:"", display: "login" })}>Back to Login</span>
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
