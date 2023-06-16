import React, { Dispatch, SetStateAction, useEffect, useState } from "react";
import { Layout, Popconfirm, Space, Table } from 'antd';
import { SelectedVariantPeriod } from "./AccountSelectedPeriod";
import { AccountGroupType, accountType, IOperation, operationAccount } from "../../api/api";
import { AccountDataType, IDateOption } from "./AccountsList";

import type { ColumnsType } from "antd/es/table";
import moment from "moment";
import { TypeOperation } from "./AccountOperation";


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
            title: 'CategoryName',
            dataIndex: 'categoryName',
            width: 300
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
            width: 300,
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
        if (_removeOperation.typeOperation === TypeOperation.Transfer) {
            operationAccount.removeTransfer(_removeOperation.id).then(
                res => { 
                    if (res.status === 200) {
  
                        removeOperation(_removeOperation);
                        
                        onChangeBalanceAccounts(res.data);
                    }
                }
            )
        }
        else {
            operationAccount.remove(_removeOperation.id).then(
                res => { 
                    if (res.status === 200) {
  
                        removeOperation(_removeOperation);
                        
                        onChangeBalanceAccounts(res.data);
                    }
                }
            )
        }

    };

    const handleAddOperation = (operation: IOperation[]) => {
        const items = [...operationsList];
        operation.forEach(element => {
            let operationDate = new Date(element.operationDate);
            if (items.length < 10 && selectedDateOption.dataOption === SelectedVariantPeriod.lastOperation10)
                {
                    if (element.accountId === selectedAccount?.id)
                        {
                            items.push(element)
                            setOperationList(items);
                        }
                    return;
                }
            if (operationDate  >= selectedDateOption.period.startDate && operationDate <= selectedDateOption.period.endDate)
               { 
                    if (element.accountId === selectedAccount?.id)
                        {
                            items.push(element)                            
                            setOperationList(items);
                        }
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