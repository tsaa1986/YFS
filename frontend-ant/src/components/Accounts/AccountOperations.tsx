import React, { useEffect, useState } from "react";
import { Collapse } from 'antd';
import AccountSelectedPeriod from "./AccountSelectedPeriod";
import { AccountGroupType } from "../../api/api";
import { DataType } from "./AccountsList";

interface IAccountOperationProps {
    selectedAccountGroupData: AccountGroupType | null
    selectedAccount: DataType | undefined
}


const AccountOperation: React.FC<IAccountOperationProps> = ({selectedAccountGroupData, selectedAccount}) => {
    const [account, setAccount] = useState(selectedAccount);

    useEffect(()=>{
        //console.log('seekcted account:', account)
        setAccount(selectedAccount)
    },[selectedAccount])

    return (
        <div>
            <AccountSelectedPeriod/>
            <div>{selectedAccountGroupData?.accountGroupId}</div>
            <div>{account?.balance}</div>
        </div>
    )
}


export default AccountOperation;