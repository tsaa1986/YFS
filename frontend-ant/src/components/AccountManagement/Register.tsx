import React from 'react';
import { NavLink } from 'react-router-dom';
//import Layout from 'components/SimpleLayout';
import { Typography, Card, Layout } from "antd";
//import SystemContext from 'context/SystemContext';
//import RegisterUser from './components/RegisterUser';
//import { authenticationService } from 'services/AuthenticationService';
import { RegisterUser } from './RegisterUser';

const { Title } = Typography;

export const Register:React.FC = () => {

return(
    <Layout>
        <Card className="login-container">
                    
            <Title>Register</Title>
            <RegisterUser  />
               {/* //onSuccess={this.loginUser}
                //showTerms={true} />*/}

            <div className="reset-password text-right mt-3">
                <NavLink to="/login" className="ant-btn">Back to Login</NavLink>
            </div>
        </Card>                
    </Layout>
);
}
