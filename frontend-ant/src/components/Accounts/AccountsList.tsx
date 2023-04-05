import React, { Dispatch, SetStateAction, useEffect, useRef, useState } from 'react';
import { Button, Space,Table, Divider } from "antd";
import { account, AccountGroupType, accountListType, accountType } from '../../api/api';
import { ColumnsType } from 'antd/es/table';
import AccountOperation from './AccountOperations';


type accountListPropsType = {
  accountGroupData: AccountGroupType | null
  //setSelectedAccount: Dispatch<SetStateAction<DataType>>;
  //accountListSelectedTab: accountListType
  //onChange: (account: DataType) => void
  //onTabSelected: (tab: AccountGroupType) => void
  //onTabSelected: (tab: string) => void
}

export interface DataType {
  key: React.Key;
  name: string;
  balance: number;
}

const rowSelection = {
  onChange: (selectedRowKeys: React.Key[], selectedRows: DataType[]) => {
    console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
  },
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
    const [selectedAccount, setSelectedAccount] = useState<DataType>();

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

    
    const onChangeAccount = (newAccount: DataType) => {
      setSelectedAccount(newAccount);
    };

    useEffect(()=>{
      //debugger
      fetchAccountList();
    }, [props.accountGroupData?.accountGroupId])

    useEffect(()=> {
      //console.log(selectedAccount)
    },[selectedAccount])


    //console.log('render accountslist',accountListDataSource, props.accountGroupData?.accountGroupId)

    return(
    <div>
        <Space style={{ display: 'flex' }}>
        </Space>
        {/*accountListDataSource?.map( item => {return item} )*/}
        <Table 
          onRow={(record, rowIndex) => {
              return {
                onClick: (e) => { //console.log(record);
                  setSelectedAccount(record);
                } 
              } 
            }
          }
          columns={columns} 
          dataSource={accountListDataSource}/>
        <Divider />
        <AccountOperation selectedAccountGroupData={props.accountGroupData} selectedAccount={selectedAccount}/>
        {/*<div>{(accountListDataSource !== undefined && accountListDataSource !== null && Array.isArray(accountListDataSource)) ?  accountListDataSource.map( item => {return <div>1</div>} ) : 'hi' }</div>*/}
    </div>
    )
}