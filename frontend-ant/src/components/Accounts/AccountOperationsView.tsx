import React, { Dispatch, SetStateAction, useDebugValue, useEffect, useState } from "react";
import { Layout, Popconfirm, Space, Table } from 'antd';
import { SelectedVariantPeriod } from "./AccountSelectedPeriod";
import { AccountGroupType, accountType, IOperation, operationAccount } from "../../api/api";
import { AccountDataType, IDateOption } from "./AccountsList";
import { StringGradients } from "antd/es/progress/progress";
import type { ColumnsType } from "antd/es/table";
import moment from "moment";
import { appendFile } from "fs";
import { collapseTextChangeRangesAcrossMultipleVersions } from "typescript";
import { Item } from "rc-menu";

interface IAccountOperationViewProps {
    selectedAccountGroupData: AccountGroupType | null
    selectedAccount: accountType | undefined
    selectedDateOption: IDateOption
    accountListDataSource: accountType[]
    setAccountListSelectedTab: Dispatch<SetStateAction<any>>
    addedOperation: IOperation[] | undefined
    onChangeBalanceAccount: (accountId: number, operationAmount: number) => void
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
    onChangeBalanceAccount}) => {
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
            title: 'Balance',
            dataIndex: 'balance',
            width: 100,
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
                    <Popconfirm title="Sure to delete?" onConfirm={() => handleDeleteOperation(record.id)}>
                        <a>Delete</a>
                    </Popconfirm>
                </Space>
              ) : null),
        },
        ]

    const handleDeleteOperation = (id: number) => {
            //const newData = operationsList.filter((item: any) => item.key !== key);
            //setDataSource(newData);
            console.log(id);
            operationAccount.remove(id).then(
                res => {
                    if (res.status === 200) {
  
                        removeOperation(id);
                        
                        res.data.forEach(element => {
                            //changeAccountBalance(element.id, element.balance);   

                            onChangeBalanceAccount(element.id, 0);                     
                        });                        
                        //refresh table account and operation(before check record included range)
                    }
                }
            )
    };

    const handleAddOperation = (operation: IOperation[]) => {
        const items = [...operationsList];
        operation.forEach(element => {
            if (element.categoryId === -1 && element.operationAmount > 0)
                {
                    items.push(element)
                    setOperationList(items);
                    onChangeBalanceAccount(element.accountId, element.balance);
                }
            if (element.categoryId === -1 && element.operationAmount < 0)
                {
                    onChangeBalanceAccount(element.accountId, element.balance);
                }
            if (element.categoryId !== -1) {
                items.push(element)
                setOperationList(items);
                //changeAccountBalance(element.accountId, element.balance);
                onChangeBalanceAccount(element.accountId, element.balance);
            }
        });    

    }

    const removeOperation = (id: number) => {
        const items = operationsList;

        if (operationsList.length > 0)
        {        
            setOperationList(items.filter(o => o.id !== id));
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