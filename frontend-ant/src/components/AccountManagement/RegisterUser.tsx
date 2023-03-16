import React from 'react';
import { Form, Button, Input, Checkbox } from 'antd';
import { authAPI, UserRegistrationType } from '../../api/api';
import { useNavigate } from 'react-router-dom';
//import { userService } from 'services/UserService.js';
//import SystemContext from 'context/SystemContext';
//import { notify } from 'util/Notify';

export const RegisterUser:React.FC = () => {
    const navigate = useNavigate();
// submit event
const handleOk = (values: UserRegistrationType) => {
    authAPI.signUp(values).then(
        res => { 
            console.log('res ' + res)
            if (res === 201){
                
                navigate("/");
            }
        }
    )
    //setState({ loading: true });
    
    //values.appId = this.context.appId;
/*       userService.post(values)
        .then(data => {

            if(this.props.onSuccess) {
                this.props.onSuccess(values);
            }
                
            return;
        })
        .catch(err => {
            notify.error(err);
        })
        .finally(() => {
            this.setState({
                loading: false
            });
        });   */                     
}

    return(
        <Form labelCol={{ span: 10 }} wrapperCol={{ span: 14 }} onFinish={handleOk}
        >

        <Form.Item 
            label="Username"
            name="userName"
            //autoComplete="off"
            rules={[{ required: true, message: 'Please enter a username' }]}
        >
            <Input placeholder="username" autoComplete="off" />
        </Form.Item>

        <Form.Item 
            label="Password"
            name="password"
            rules={[
                    { required: true, message: 'Please enter a password' }
            ]}
        >
            <Input.Password placeholder="password" autoComplete="new-password" />
        </Form.Item>

        <Form.Item 
            label="Confirm Password"
            name="confirm"
            rules={[
                    { required: true, message: 'Please enter the password again' },
                    ({ getFieldValue }) => ({
                        validator(rule, value) {
                        if (!value || getFieldValue('password') === value) {
                            return Promise.resolve();
                        }
                        return Promise.reject('The passwords do not match!');
                        }
                    })
            ]}
        >
            <Input.Password placeholder="confirm password" />
        </Form.Item>

        <Form.Item 
            label="Email"
            name="email"
            rules={[
                    { type: 'email',  message: 'Please enter a valid email address' },
                    { required: true, message: 'Please enter an email address' }
                ]}
        >
            <Input placeholder="email" />
        </Form.Item>

        <Form.Item 
            label="Phone"
            name="phoneNumber"
        >
            <Input placeholder="phone" />
        </Form.Item>

        <Form.Item wrapperCol={{ span: 14, offset: 10 }}>
            <Button type="primary" htmlType="submit" /*loading={state.loading}*/>Submit</Button>
        </Form.Item>
    </Form>
    );
}


