import React, { useEffect, useState } from "react";
import { Collapse } from 'antd';
import AccountSelectedPeriod from "./AccountSelectedPeriod";
import { AccountGroupType } from "../../api/api";
import { DataType } from "./AccountsList";

interface IAccountOperationProps {
    selectedAccountGroupData: AccountGroupType | null
    //onChangeAccount: (account: DataType | undefined) => void
    selectedAccount: DataType | undefined
}


const AccountOperation: React.FC<IAccountOperationProps> = ({selectedAccountGroupData, selectedAccount}) => {
    const [selectedAccount2, setSelected2Account] = useState();
/*
    const onChangeAccount = (newAccount: DataType) => {
        setSelected2Account(newAccount);
      };*/
console.log(selectedAccount)
    useEffect(()=>{
        console.log('seekcted account:', selectedAccount2)
    },[selectedAccount2])

    return (
        <div>
            <AccountSelectedPeriod/>
            <div>{selectedAccountGroupData?.accountGroupId}</div>

        </div>
    )
}


export default AccountOperation;