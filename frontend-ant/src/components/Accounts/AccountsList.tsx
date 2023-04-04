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

interface DataType {
  key: React.Key;
  name: string;
  balance: number;
}

export const AccountsList = (props: accountListPropsType) => {



const columns: ColumnsType<DataType> = [
  {
    title: 'action',
    dataIndex: '',
    key: 'x',
    width:120,
    render: () => {return(<button title='Add expens'>add expens</button>)},
  },
  {
    title: 'Name',
    dataIndex: 'name',
    key: 'name'
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
                  setAccountListSelectedTab(res)
                  })
            }
            else {
              account.getListByGroupId(tabId).then(
                res => { console.log(res)
                  setAccountListSelectedTab(res)
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
        </Space>
        {/*accountListDataSource?.map( item => {return item} )*/}
        <Table columns={columns} dataSource={accountListDataSource}/>
        {/*<div>{(accountListDataSource !== undefined && accountListDataSource !== null && Array.isArray(accountListDataSource)) ?  accountListDataSource.map( item => {return <div>1</div>} ) : 'hi' }</div>*/}
    </div>
    )
}