import { Button, Divider } from "antd";
import React, { useEffect, useState } from "react";
import { AccountGroupType, accountType } from "../../api/api";
import AccountSelectedPeriod from "./AccountSelectedPeriod";
import { AccountsList } from "./AccountsList";

type tabDetailsPropsType = {
    accountGroupData: AccountGroupType | null
    //accountListSelectedTab: accountListType
    //onTabSelected: (tab: AccountGroupType | null) => void
    //accountSelected:
}

interface tabDetailsPropsType1 {
    accountGroupData: AccountGroupType | null
    openAccounts: accountType[] | undefined
}

const TabDetails: React.FC<tabDetailsPropsType1> = ({accountGroupData, openAccounts}) => {
    const [activeTabKey, setActiveTabKey] = useState('0')
    const [selectedAccount, setSelectedAccount] = useState();

    useEffect(()=>{
      console.log('tabdetails: ', accountGroupData)
      console.log('tabdetails: ', openAccounts)
      console.log('tabdetails: ','props.accounts')
      //setActiveTabKey('')
    },[accountGroupData])
  
    return (           
        <div>
            <AccountsList accountGroupData={accountGroupData} openAccounts={openAccounts}/>  
        </div>
    );
}

export default TabDetails;