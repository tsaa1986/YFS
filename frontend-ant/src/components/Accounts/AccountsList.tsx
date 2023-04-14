import React, { Dispatch, SetStateAction, useEffect, useRef, useState } from 'react';
import { Button, Space,Table, Divider, FormInstance } from "antd";
import { account, AccountGroupType, accountListType, accountType } from '../../api/api';
import { ColumnsType } from 'antd/es/table';
import AccountOperationsView from './AccountOperationsView';
import { Collapse } from 'antd';
import AccountSelectedPeriod from './AccountSelectedPeriod';
import TransactionForm, { TypeTransaction } from './AccountTransaction';

const { Panel } = Collapse;

type accountListPropsType = {
  accountGroupData: AccountGroupType | null
  //setSelectedAccount: Dispatch<SetStateAction<DataType>>;
  //accountListSelectedTab: accountListType
  //onChange: (account: DataType) => void
  //onTabSelected: (tab: AccountGroupType) => void
  //onTabSelected: (tab: string) => void
}

export interface AccountDataType {
  key: React.Key;
  id: number;
  name: string;
  balance: number;
}

export interface ISelectedDate {
  startDate: Date,
  endDate: Date
}
export interface IDateOption {
  period: ISelectedDate
}

export const AccountsList = (props: accountListPropsType) => {

const columns: ColumnsType<AccountDataType> = [
  {
    title: 'action',
    dataIndex: '',
    key: 'x',
    width:100,
    render: () => {return(<div>
                  <button onClick={()=> {                      
                      setSelectedTypeTransaction(TypeTransaction.Expense)
                      setOpenTransactionForm(true)
                    }
                  } 
                      title='Add expens'>-</button>
                  <button onClick={()=> {                      
                      setSelectedTypeTransaction(TypeTransaction.Income)     
                      setOpenTransactionForm(true)               
                      }
                  } 
                      title='Add income'>+</button>
                  <button onClick={()=> {                                           
                      setSelectedTypeTransaction(TypeTransaction.Transfer)
                      setOpenTransactionForm(true) 
                    }
                  }   title='Add transfer'>-+</button>
                  </div>)
    },
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
  {
    title: 'id',
    dataIndex: 'id',
    key: 'id'
  },
];
    const [accountListGroupId, setAccountListGroupId] = useState('0')
    //const [activeTab, setActiveTab] = useState<AccountGroupType>();
    const [accountListDataSource, setAccountListSelectedTab] = useState<any>();
    const [selectedAccount, setSelectedAccount] = useState<AccountDataType>();
    const [selectedDateOption, setSelectedDateOption] = useState<IDateOption>({period: {startDate: new Date(), endDate: new Date()}})
    const [selectedTypeTransaction, setSelectedTypeTransaction] = useState<TypeTransaction>(0)
    const [openTransactionForm, setOpenTransactionForm] = useState<boolean>(false);

    const fetchAccountListSelectedTab = () => {
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
        //console.log('useeffect fetch accountlist:',accountListDataSource)
      }
    }  
    
    useEffect(()=>{
      //debugger
      fetchAccountListSelectedTab();
    }, [props.accountGroupData?.accountGroupId])

    useEffect(()=> {
      console.log(selectedDateOption)
    },[selectedDateOption])

    return(
    <div>
        <Table 
          onRow={(record, rowIndex) => {
              return {
                onClick: (e) => { 
                  setSelectedAccount(record);
                } 
              } 
            }
          }
          columns={columns} 
          dataSource={accountListDataSource}/>
          
    <div>
          <Divider />
          <Collapse defaultActiveKey={['1']} ghost>
            <Panel header={`This is panel header ${selectedAccount?.name}`} key="1">
              <AccountSelectedPeriod selectedDateOption={selectedDateOption} setSelectedDateOption={setSelectedDateOption}/>  
              <AccountOperationsView selectedAccountGroupData={props.accountGroupData} selectedAccount={selectedAccount} selectedDateOption={selectedDateOption}/>
            </Panel>
          </Collapse>
          <TransactionForm open={openTransactionForm} setOpenTransactionForm={setOpenTransactionForm}
              selectedAccount={selectedAccount}
              typeTransaction={selectedTypeTransaction}/>
          {/*<div>{(accountListDataSource !== undefined && accountListDataSource !== null && Array.isArray(accountListDataSource)) ?  accountListDataSource.map( item => {return <div>1</div>} ) : 'hi' }</div>*/}
        </div>
    </div>
    )
}