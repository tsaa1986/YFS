import React, { useEffect, useRef, useState } from 'react';
import { Button, Space,Table, Divider } from "antd";
import { account, AccountGroupType, accountListType, accountType } from '../../api/api';
import { ColumnsType } from 'antd/es/table';


type accountListPropsType = {
  accounts: accountListType | undefined
  activeTabKey: string
  //onTabSelected: (tab: AccountGroupType) => void
  //onTabSelected: (tab: string) => void
}

export const AccountsList = (props: accountListPropsType) => {
    /*const dataSource = [
        {
          key: '1',
          name: 'Mike',
          age: 32,
          address: '10 Downing Street',
        },
        {
          key: '2',
          name: 'John',
          age: 42,
          address: '10 Downing Street',
        },
      ];
      const columns = [
        {
          title: 'Name',
          dataIndex: 'name',
          key: 'name',
        },
        {
          title: 'Age',
          dataIndex: 'age',
          key: 'age',
        },
        {
          title: 'Address',
          dataIndex: 'address',
          key: 'address',
        },
      ];
*/
const columns: ColumnsType<any> = [
  {
    title: 'Name',
    dataIndex: 'name',
    key: 'name'
    //render: (text: string) => <a>{text}</a>,
  },
  {
    title: 'Id',
    dataIndex: 'id',
    key: 'id'
  },
  {
    title: 'Balance',
    dataIndex: 'balance',
    key: 'balance'
  },
];
    const [accountListGroupId, setAccountListGroupId] = useState('0')
    //const [activeTab, setActiveTab] = useState<AccountGroupType>();
    const [accountListDataSource, setAccountListSelectedTab] = useState<accountListType>(props.accounts);
    
    useEffect(()=>{

    }, )

    useEffect(()=>{
      //debugger
      console.log('accountlist effect:', props.activeTabKey)
      console.log('accountlist effect:', props.accounts)
      setAccountListGroupId(props.activeTabKey)
      //let accounts = account.getListByGroupId(accountListGroupId.toString()).then(
       // res=>{ return res}
      //);
      //setAccountListSelectedTab(accounts)
      //setAccountListSelectedTab(tempAccountList);
    },[props.activeTabKey])

    return(
    <div>
        <Space style={{ display: 'flex' }}>
        <h2>Accounts List</h2>
        </Space>
        <div>{accountListGroupId}</div>
        <Table dataSource={accountListDataSource} columns={columns}/>
    </div>
    )
}