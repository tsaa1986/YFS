import React, { useEffect, useState } from "react";
import { Collapse, Layout, Space, Table } from 'antd';
import AccountSelectedPeriod from "./AccountSelectedPeriod";
import { AccountGroupType, IOperation, operationAccount } from "../../api/api";
import { AccountDataType, IDateOption } from "./AccountsList";
import { StringGradients } from "antd/es/progress/progress";
import type { ColumnsType } from "antd/es/table";

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

const operationColumns: ColumnsType<IOperationDataType> = [
    {
        title: 'Date Operation',
        dataIndex: 'operationDate',
        key: 'operatioDate',
        render: (text) => <a>{text}</a>,
      },
      {
        title: 'Category',
        dataIndex: 'categoryId',
        key: 'categoryId'
      },
      {
        title: 'Amount',
        dataIndex: 'CurrencyAmount',
        key: 'CurrencyAmount',
      },
      {
        title: 'Description',
        dataIndex: 'description',
        key: 'description',
      }
    ]

const AccountOperationsView: React.FC<IAccountOperationViewProps> = ({selectedAccountGroupData, selectedAccount, selectedDateOption}) => {
    const [account, setAccount] = useState(selectedAccount);
    const [operationsList, setOperationList] = useState<any>([]);

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

    useEffect(()=>{
        //console.log('seekcted account:', account)
        setAccount(selectedAccount)
    },[selectedAccount])

    useEffect(()=> {
        fetchOperationsForAccountForPeriod();
    },[account])

    return (
        <Layout>
        <Space wrap>
            <div>{selectedAccountGroupData?.accountGroupId}</div>
            <div>{account?.balance}</div>
            <div>{selectedDateOption.period.startDate.toDateString()}</div>
        </Space>
        <Table columns={operationColumns} dataSource={operationsList} />

        </Layout>
    )
}


export default AccountOperationsView;