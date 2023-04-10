import React, { useEffect, useState } from "react";
import { Collapse, Space } from 'antd';
import AccountSelectedPeriod from "./AccountSelectedPeriod";
import { AccountGroupType } from "../../api/api";
import { DataType, IDateOption } from "./AccountsList";

interface IAccountOperationViewProps {
    selectedAccountGroupData: AccountGroupType | null
    selectedAccount: DataType | undefined
    selectedDateOption: IDateOption
}


const AccountOperationsView: React.FC<IAccountOperationViewProps> = ({selectedAccountGroupData, selectedAccount, selectedDateOption}) => {
    const [account, setAccount] = useState(selectedAccount);

    useEffect(()=>{
        //console.log('seekcted account:', account)
        setAccount(selectedAccount)
    },[selectedAccount])

    return (
        <Space wrap>
            <div>{selectedAccountGroupData?.accountGroupId}</div>
            <div>{account?.balance}</div>
            <div>{selectedDateOption.period.startDate.toDateString()}</div>
        </Space>
    )
}


export default AccountOperationsView;