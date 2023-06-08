import React, { Dispatch, SetStateAction, useEffect, useState } from "react";
import { Layout, Popconfirm, Space, Table } from 'antd';
import { SelectedVariantPeriod } from "./AccountSelectedPeriod";
import { AccountGroupType, accountType, IOperation, operationAccount } from "../../api/api";
import { AccountDataType, IDateOption } from "./AccountsList";

import type { ColumnsType } from "antd/es/table";
import moment from "moment";


interface IAccountOperationViewProps {
    selectedAccountGroupData: AccountGroupType | null
    selectedAccount: accountType | undefined
    selectedDateOption: IDateOption
    accountListDataSource: accountType[]
    setAccountListSelectedTab: Dispatch<SetStateAction<any>>
    addedOperation: IOperation[] | undefined
    //onChangeBalanceAccount: (accountId: number, operationAmount: number) => void
    onChangeBalanceAccounts: (operations: IOperation[]) => void
}

interface IOperationDataType {
    id: number;
    dateOperation: Date;
    categoryName: string;
    currencyAmount: number;
    description: string;
    tag: string;
}

const AccountOperationsView: React.FC<IAccountOperationViewProps> = ({selectedAccountGroupData, selectedAccount, selectedDateOption, 
    accountListDataSource, 
    setAccountListSelectedTab,
    addedOperation,
    onChangeBalanceAccounts}) => {
    const [account, setAccount] = useState(selectedAccount);
    const [operationsList, setOperationList] = useState<Array<IOperation>>([]);

    const operationColumns: ColumnsType<IOperation> = [
        {
            title: 'Date Operation',
            dataIndex: 'operationDate',
            render: (text) => moment(text).format('DD.MM.YYYY'),
            width: 100,
            align: 'center',
        },
        {
            title: 'Category',
            dataIndex: 'categoryId',
            width: 400
        },
        {
            title: 'Amount',
            dataIndex: 'currencyAmount',
            width: 140,
            align: 'center',
            render: (text) => { return (
                Intl.NumberFormat('en-US').format(text)
            )
            }
        },
        {
            title: 'Description',
            dataIndex: 'description',
            width: 200,
        },
        {
            title: 'Action',
            dataIndex: 'action',
            key: 'x',
            //render: (_, record) => (<div>{record.id}</div>),
            render: (_: any, record) => (                
                operationsList.length >= 1 ? (
                <Space size="small">
                  <a>Edit</a>
                    <Popconfirm title="Sure to delete?" onConfirm={() => handleDeleteOperation(record)}>
                        <a>Delete</a>
                    </Popconfirm>
                </Space>
              ) : null),
        },
        ]

    const handleDeleteOperation = (_removeOperation: IOperation) => {
            //const newData = operationsList.filter((item: any) => item.key !== key);
            //setDataSource(newData);
            console.log("remove operation: ", _removeOperation);
            //debugger;
            operationAccount.remove(_removeOperation.id).then(
                res => { debugger
                    if (res.status === 200) {
  
                        removeOperation(_removeOperation);
                        
                        onChangeBalanceAccounts(res.data);
                        //res.data.forEach(element => {
                        //    onChangeBalanceAccount(element.id, element.balance);                     
                        //});                        
                        //refresh table account and operation(before check record included range)
                    }
                }
            )
    };

    const handleAddOperation = (operation: IOperation[]) => {
        const items = [...operationsList];
        operation.forEach(element => {
            if (element.categoryId === -1 && element.operationAmount > 0 && element.accountId === selectedAccount?.id)
                {
                    //добавить условие проверки входит ли операция в вібраній диапазон дат
                    items.push(element)
                    setOperationList(items);
                    //onChangeBalanceAccount(element.accountId, element.balance);
                }
            if (element.categoryId === -1 && element.operationAmount < 0)
                {
                    //onChangeBalanceAccount(element.accountId, element.balance);
                }
            if (element.categoryId !== -1 && element.accountId === selectedAccount?.id) {
                items.push(element)
                setOperationList(items);
                //onChangeBalanceAccount(element.accountId, element.balance);
            }
        });    

    }

    const removeOperation = (removeOperation: IOperation) => {
        const items = [...operationsList];

        if (operationsList.length > 0)
        {   
            if (selectedAccount?.id == removeOperation.accountId)     
                setOperationList(items.filter(o => o.id !== removeOperation.id));
        }
    }


    const fetchOperationsForAccountForPeriod = () => {
        if (account != null)
            {
                console.log(selectedDateOption)
                 operationAccount.getOperationsAccountForPeriod(account.id, selectedDateOption.period.startDate, selectedDateOption.period.endDate)
                .then(res => {
                    console.log('fetchOperations', res)
                    if (res != undefined)
                        setOperationList(res);
                })
            }
    }

    const fetchOperationsForAccount = () => {
        if ((account != null) && (selectedDateOption.dataOption == SelectedVariantPeriod.lastOperation10))
        {
             operationAccount.getLast10OperationsAccount(account.id)
            .then(res => {
                if (res != undefined) {
                console.log('fetch-Last10-Operations', res)
                setOperationList(res);
                }
            })
        }
    }

    useEffect(()=>{
        setAccount(selectedAccount)
    },[selectedAccount])

    useEffect(()=> {
        if (selectedDateOption.dataOption == SelectedVariantPeriod.lastOperation10)
            fetchOperationsForAccount()
            else fetchOperationsForAccountForPeriod();
    },[account])

    useEffect(() => {
        if (selectedDateOption.dataOption == SelectedVariantPeriod.lastOperation10)
            fetchOperationsForAccount()
            else fetchOperationsForAccountForPeriod();
    }, [selectedDateOption])

    useEffect(()=>{
        if (addedOperation != undefined && addedOperation != null)
            handleAddOperation(addedOperation)
    },[addedOperation])

    return (
        <Layout>
        <Space wrap>
            <div>{selectedAccountGroupData?.accountGroupId}</div>
            <div>{account?.balance}</div>
            <div>{selectedDateOption.period.startDate.toDateString()}</div>
        </Space>
        <Table  size="small"  
                columns={operationColumns} dataSource={operationsList} 
                rowKey={record => record.id}
                pagination={{ position: ["bottomLeft"] }}                
                scroll={{ x: 250, y: 300 }}
                />
        </Layout>
    )
}


export default AccountOperationsView;