import React, {useState, useEffect, Dispatch, Component} from "react";
import { NavLink,redirect, useNavigate} from 'react-router-dom';
import { Layout, Form, Button, Input, Typography, Alert, Card } from "antd";
import { authAPI } from "../../api/api";
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { HeaderLayout } from "../Header/HeaderLayout";
import { IUser } from "../types/types";


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

type LoginTypeProps = {
    loginDisplay: string,
    setLoginDisplay: React.Dispatch<React.SetStateAction<string>>,
    setIsLoggedIn: React.Dispatch<React.SetStateAction<boolean>>,
    languageDisplay: string,
    setLanguageDisplay: React.Dispatch<React.SetStateAction<string>>
}

export const Login: React.FC<LoginTypeProps> = ({setIsLoggedIn, setLoginDisplay, loginDisplay, languageDisplay, setLanguageDisplay}) => {
 const navigate = useNavigate();
 const [state, setState] = useState<StateTypeProps>({showError: false, errorMsg: "", display: "hide"})

useEffect( ()=> {
      let newState: StateTypeProps;
        if (state.display !== null)
        {
            newState = {...state, display: loginDisplay}
            setState(newState)
        }
        console.log("chnge login display state", loginDisplay)
    }, [loginDisplay])

 const handleFinish = (values: handleFinishTypeProps) => {        
    //this.loginUser(values);
    console.log(values);
    authAPI.login(values.username, values.password).then(
        res => { 
            if (res != false) {
                console.log(res)
                setIsLoggedIn(true);
                navigate("/home");
            } else console.log(res);
        }
    )
    console.log('handlefinish');
}

const logInDemoUser = () => {
    authAPI.login("demo", "123$qweR").then(
        res => { 
            if (res != false) {
                setIsLoggedIn(true);
                navigate("/home");
            } 
        }
    )
}

const handlePasswordReminder = (values:any) => {
    console.log('handlePasswordReminder')
}

return( 
<div>
    {(state.display === "login" || state.display === "password") &&
    <Card className="login-container">
        {
            state.display === "login" &&
                <div>
                    <Title>Login</Title>
                    <Form onFinish={handleFinish} style={{ width: "100%" }}>

                        <Form.Item
                            name="username"
                            rules={[{ required: true, message: 'Enter a username' }]}>
                            <Input 
                                prefix={<UserOutlined className="site-form-item-icon" />}
                                placeholder="username"
                            />
                        </Form.Item> 

                        <Form.Item
                            name="password"
                            rules={[{ required: true, message: 'Enter a password' }]}>
                            <Input 
                                prefix={<LockOutlined className="site-form-item-icon" />}
                                placeholder="password" type="password" 
                            />
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
        </Card>}
</div>
);
} 