import React from 'react';
import { NavLink } from 'react-router-dom';
//import Layout from 'components/SimpleLayout';
import { Typography, Card, Layout } from "antd";
//import RegisterUser from './components/RegisterUser';
//import { authenticationService } from 'services/AuthenticationService';
import { RegisterUser } from './RegisterUser';
import { authAPI, UserRegistrationType } from '../../api/api';

const { Title } = Typography;

export const Register:React.FC = () => {

const loginUser = (values:  UserRegistrationType) => {
        //this.context.setLoading(true);

        authAPI.login(values.userName, values.password)
            .then(data => {
                debugger
                console.log('loginuser: ' + data);
                handleLogin(data);
            })
            .catch(err => {
                 console.log(err);
                //setState({
                    //showError: true,
                    //errorMsg: err
                //});
                //this.context.setLoading(false);
            }); 
    }

const handleLogin = (data:any) => {
    //this.context.setLoading(true);
    //this.context.setUser(data);

    let to: any = "/";
    if(localStorage.getItem("redirect_url")) {
        to = localStorage.getItem("redirect_url");
        localStorage.removeItem("redirect_url");
    }
        
        window.location.href = to; // go home or wherever directed to
    }

return(
    <Layout>
        <Card className="login-container">
                    
            <Title>Register</Title>
            <RegisterUser  
                onSuccess={loginUser}
                showTerms={true}
            />

            <div className="reset-password text-right mt-3">
                <NavLink to="/login" className="ant-btn">Back to Login</NavLink>
            </div>
        </Card>                
    </Layout>
);
}
