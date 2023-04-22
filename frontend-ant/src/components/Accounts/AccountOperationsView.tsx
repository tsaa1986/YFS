import React, { useEffect, useState } from "react";
import { Layout, Popconfirm, Space, Table } from 'antd';
import { SelectedVariantPeriod } from "./AccountSelectedPeriod";
import { AccountGroupType, IOperation, operationAccount } from "../../api/api";
import { AccountDataType, IDateOption } from "./AccountsList";
import { StringGradients } from "antd/es/progress/progress";
import type { ColumnsType } from "antd/es/table";
import moment from "moment";

interface IAccountOperationViewProps {
    selectedAccountGroupData: AccountGroupType | null
    selectedAccount: AccountDataType | undefined
    selectedDateOption: IDateOption
}

interface IOperationDataType {
    key: string;
    dateOperation: Date;
    categoryName: string;
    currencyAmount: number;
    description: string;
    tag: string;
}

const AccountOperationsView: React.FC<IAccountOperationViewProps> = ({selectedAccountGroupData, selectedAccount, selectedDateOption}) => {
    const [account, setAccount] = useState(selectedAccount);
    const [operationsList, setOperationList] = useState<any>([]);

    const operationColumns: ColumnsType<IOperationDataType> = [
        {
            title: 'Date Operation',
            dataIndex: 'operationDate',
            key: 'operatioDate',
            //render: (text) => <a>{text}</a>,
            render: (text) => moment(text).format('DD.MM.YYYY'),
            width: 100,
            align: 'center',
          },
          {
            title: 'Category',
            dataIndex: 'categoryId',
            key: 'categoryId',
            width: 400
            
          },
          {
            title: 'Amount',
            dataIndex: 'currencyAmount',
            key: 'currencyAmount',
            width: 140,
            align: 'center',
          },
          {
            title: 'Description',
            dataIndex: 'description',
            key: 'description',
            width: 200,
          },
          {
            title: 'action',
            dataIndex: 'action',
            render: (_, record) => (
                operationsList.length >= 1 ? (
                <Space size="small">
                  <a>Edit {}</a>
                    <Popconfirm title="Sure to delete?" onConfirm={() => handleDeleteOperation(record.key)}>
                        <a>Delete</a>
                    </Popconfirm>
                </Space>
              ) : null),
          }
        ]

    const handleDeleteOperation = (key: React.Key) => {
            //const newData = dataSource.filter((item) => item.key !== key);
            //setDataSource(newData);
          };

    const fetchOperationsForAccountForPeriod = () => {
        if (account != null)
            {
                console.log(selectedDateOption)
                 operationAccount.getOperationsAccountForPeriod(account.id, selectedDateOption.period.startDate, selectedDateOption.period.endDate)
                .then(res => {
                    console.log('fetchOperations', res)
                    setOperationList(res);
                })
            }
    }

    const fetchOperationsForAccount = () => {
        if ((account != null) && (selectedDateOption.dataOption == SelectedVariantPeriod.lastOperation10))
        {
             operationAccount.getLast10OperationsAccount(account.id)
            .then(res => {
                console.log('fetch-Last10-Operations', res)
                setOperationList(res);
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

    return (
        <Layout>
        <Space wrap>
            <div>{selectedAccountGroupData?.accountGroupId}</div>
            <div>{account?.balance}</div>
            <div>{selectedDateOption.period.startDate.toDateString()}</div>
        </Space>
        <Table  size="small"  
                columns={operationColumns} dataSource={operationsList} 
                pagination={{ position: ["bottomLeft"] }}
                //scroll={{ y: 2000 }}
                />

        </Layout>
    )
}


export default AccountOperationsView;