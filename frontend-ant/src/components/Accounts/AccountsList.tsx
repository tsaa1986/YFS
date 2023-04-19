import React, { useEffect, useState } from 'react';
import { Table, Divider } from "antd";
import { account, AccountGroupType, accountListType } from '../../api/api';
import { ColumnsType } from 'antd/es/table';
import AccountOperationsView from './AccountOperationsView';
import { Collapse } from 'antd';
import AccountSelectedPeriod from './AccountSelectedPeriod';
import OperationForm, { TypeOperation } from './AccountOperation';

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
                      setSelectedTypeOperation(TypeOperation.Expense)
                      setOpenOperationForm(true)
                    }
                  } 
                      title='Add expens'>-</button>
                  <button onClick={()=> {                      
                      setSelectedTypeOperation(TypeOperation.Income)     
                      setOpenOperationForm(true)               
                      }
                  } 
                      title='Add income'>+</button>
                  <button onClick={()=> {                                           
                      setSelectedTypeOperation(TypeOperation.Transfer)
                      setOpenOperationForm(true) 
                    }
                  }   title='Add transfer'>=</button>
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
    const [accountListDataSource, setAccountListSelectedTab] = useState<any>();
    const [selectedAccount, setSelectedAccount] = useState<AccountDataType>();
    const [selectedDateOption, setSelectedDateOption] = useState<IDateOption>({period: {startDate: new Date(), endDate: new Date()}})
    const [selectedTypeOperation, setSelectedTypeOperation] = useState<TypeOperation>(0)
    const [openOperationForm, setOpenOperationForm] = useState<boolean>(false);

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
          <OperationForm open={openOperationForm} setOpenOperationForm={setOpenOperationForm}
              selectedAccount={selectedAccount}
              typeOperation={selectedTypeOperation}/>
          {/*<div>{(accountListDataSource !== undefined && accountListDataSource !== null && Array.isArray(accountListDataSource)) ?  accountListDataSource.map( item => {return <div>1</div>} ) : 'hi' }</div>*/}
        </div>
    </div>
    )
}