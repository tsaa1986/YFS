import React, { useEffect, useState } from 'react';
import { Table, Divider, Tag } from "antd";
import { account, accountGroups, AccountGroupType, accountListType, accountType, IOperation } from '../../api/api';
import { ColumnsType } from 'antd/es/table';
import AccountOperationsView from './AccountOperationsView';
import { Collapse } from 'antd';
import AccountSelectedPeriod, { SelectedVariantPeriod } from './AccountSelectedPeriod';
import OperationForm, { TypeOperation } from './AccountOperation';

const { Panel } = Collapse;

type accountListPropsType = {
  accountGroupData: AccountGroupType | null
  openAccounts: accountType[] | undefined
  setOpenAccounts: React.Dispatch<React.SetStateAction<accountType[]>>
  activeTabKey: string
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
  currency: string;
  balance: number;
}

export interface ISelectedDate {
  startDate: Date,
  endDate: Date
}
export interface IDateOption {
  period: ISelectedDate,
  dataOption: SelectedVariantPeriod
}

export const AccountsList = (props: accountListPropsType) => {

const columns: ColumnsType<accountType> = [
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
    title: 'Currency',
    width:100,
    align:'center',
    dataIndex: 'currencyName',
    key: 'currencyName',
    render: (_, { currencyName } ) => {
      return (
        <Tag color={'green'} key={currencyName}>
          {currencyName.toUpperCase()}  
        </Tag>
      );
    }
  },
  {
    title: 'Balance',
    width:200,
    align:'center',
    dataIndex: 'balance',
    key: 'balance'
  },
  {
    title: 'Name',
    //width:300,
    dataIndex: 'name',
    key: 'name'
  },
  /*{
    title: 'id',
    dataIndex: 'id',
    key: 'id'
  },*/
];
    const [accountListDataSource, setAccountListSelectedTab] = useState<accountType[]>([]);
    const [selectedAccount, setSelectedAccount] = useState<accountType>();
    const [selectedDateOption, setSelectedDateOption] = useState<IDateOption>({period: {startDate: new Date(), endDate: new Date()}, dataOption: SelectedVariantPeriod.lastOperation10})
    const [selectedTypeOperation, setSelectedTypeOperation] = useState<TypeOperation>(0)
    const [openOperationForm, setOpenOperationForm] = useState<boolean>(false);
    const [addedOperation, setAddedOperation] = useState<IOperation[] | undefined>();

    const fetchAccountListSelectedTabNew = () => {
      if ((props.accountGroupData !== null) && (props.accountGroupData !== undefined)){
        let tabId = props.accountGroupData.accountGroupId;
        let accountsFiltered: accountType[] = [];
        //debugger
        if (props.openAccounts !== undefined && props.openAccounts.length > 0)  {          
          if (tabId === 0) {
            accountsFiltered = props.openAccounts.filter( acc => {
              return acc.favorites === 1})
          }
          else {
          accountsFiltered = props.openAccounts.filter( acc => {
            return acc.accountGroupId === tabId})
          }
          
          setAccountListSelectedTab(accountsFiltered);
          console.log("filtered", accountsFiltered);
        }
      
      }
    }
/*
    const onChangeBalanceAccount = (accountId: number, newBalance: number) => {
      if (props.openAccounts != undefined)
      {        
        let tempAccounts = props.openAccounts.map( account => {          
          if (account.id === accountId) {
            {
              return {...account, balance: newBalance}
            }
          }
          return account;
        })
        props.setOpenAccounts(tempAccounts)
      }
    }*/

    const onChangeBalanceAccounts = (operations: IOperation[]) => {
      if (props.openAccounts != undefined)
      {        
        let tempAccounts = props.openAccounts;
        operations.forEach( operation => {
          tempAccounts = tempAccounts.map( account => { 
            if (account.id === operation.accountId) 
              {
                return {...account, balance: operation.balance}
              }
            return account;} )
        } )

        props.setOpenAccounts(tempAccounts)
      }
    }

    useEffect( () => {
      if (addedOperation !== undefined)
      {
        onChangeBalanceAccounts(addedOperation);
      }
    }, [addedOperation])
    
    useEffect(()=>{
      fetchAccountListSelectedTabNew();
      console.log("test", props.accountGroupData)
    }, [props.accountGroupData])

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
          dataSource={accountListDataSource}
          size='small'
          
          //bordered
          
          />
          
    <div>
          <Divider />
          <Collapse defaultActiveKey={['1']} ghost>
            <Panel header={`This is panel header ${selectedAccount?.name}`} key="1">
              <AccountSelectedPeriod selectedDateOption={selectedDateOption} setSelectedDateOption={setSelectedDateOption}/>  
              <AccountOperationsView 
                selectedAccountGroupData={props.accountGroupData} 
                selectedAccount={selectedAccount} 
                selectedDateOption={selectedDateOption}
                accountListDataSource={accountListDataSource}
                setAccountListSelectedTab={setAccountListSelectedTab}
                addedOperation={addedOperation}
                onChangeBalanceAccounts={onChangeBalanceAccounts}
              />
            </Panel>
          </Collapse>
          <OperationForm open={openOperationForm} 
              setOpenOperationForm={setOpenOperationForm}
              selectedAccount={selectedAccount}
              typeOperation={selectedTypeOperation}
              setAddedOperation={setAddedOperation}    
              openAccounts={props.openAccounts}
          />
          {/*<div>{(accountListDataSource !== undefined && accountListDataSource !== null && Array.isArray(accountListDataSource)) ?  accountListDataSource.map( item => {return <div>1</div>} ) : 'hi' }</div>*/}
        </div>
    </div>
    )
}