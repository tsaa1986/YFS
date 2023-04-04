import React, { useEffect, useRef, useState } from 'react';
import { Button, Space,Table, Divider } from "antd";
import { account, AccountGroupType, accountListType, accountType } from '../../api/api';
import { ColumnsType } from 'antd/es/table';


type accountListPropsType = {
  accountGroupData: AccountGroupType | null
  //accountListSelectedTab: accountListType
 // onChange: (showTab: boolean) => void
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
    const [accountListDataSource, setAccountListSelectedTab] = useState<any>();

    const fetchAccountList = () => {
        if ((props.accountGroupData !== null) && (props.accountGroupData !== undefined)){
            let tabId = props.accountGroupData.accountGroupId.toString();
            let tempAcc:accountListType
            if (props.accountGroupData.accountGroupId.toString() =='0') {
              account.getListByFavorites().then(
                res => { console.log(res)
                  setAccountListSelectedTab(res)//return res
                  })
            }
            else {
              account.getListByGroupId(tabId).then(
                res => { console.log(res)
                  setAccountListSelectedTab(res)//return res
                  })
            }            
        //setAccountListSelectedTab(tempAcc);
        console.log('useeffect fetch accountlist:',accountListDataSource)
      }
    }

    useEffect(()=>{
      //debugger
      fetchAccountList();
    }, [props.accountGroupData?.accountGroupId])


    console.log('render accountslist',accountListDataSource,props.accountGroupData?.accountGroupId)

   /* useEffect(
      ()=> {
        //console.log(activeTabKey);
        //debugger
        if (props.accountGroupData !== null)
        {
          let tabId = props.accountGroupData.accountGroupId.toString();
          let tempAcc:accountListType
          account.getListByGroupId(tabId).then(
            res => { console.log(res)
              setAccountListSelectedTab(res)//return res
              }
          )
          //setAccountListSelectedTab(tempAcc);
          console.log('useeffect accountlist:',accountListDataSource)
        }
      }, [])*/

    return(
    <div>
        <Space style={{ display: 'flex' }}>
        <h2>Accounts List</h2>
        </Space>
        {/*accountListDataSource?.map( item => {return item} )*/}
        <Table dataSource={accountListDataSource} columns={columns}/>
        {/*<div>{(accountListDataSource !== undefined && accountListDataSource !== null && Array.isArray(accountListDataSource)) ?  accountListDataSource.map( item => {return <div>1</div>} ) : 'hi' }</div>*/}
    </div>
    )
}