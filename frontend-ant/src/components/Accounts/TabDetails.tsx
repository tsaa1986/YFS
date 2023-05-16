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
    activeTabKey: string
    openAccounts: accountType[] | undefined
    setOpenAccounts: React.Dispatch<React.SetStateAction<accountType[]>>
}

const TabDetails: React.FC<tabDetailsPropsType1> = ({accountGroupData, activeTabKey , openAccounts, setOpenAccounts}) => {

    useEffect(()=>{
      console.log('tabdetails: ', accountGroupData)
      console.log('tabdetails: openAccounts', openAccounts)
      console.log('tabdetails: activetabkey change', activeTabKey)
      //setActiveTabKey('')
    },[activeTabKey])
  
    return (           
        <div>
            <AccountsList   accountGroupData= {accountGroupData} 
                            openAccounts= {openAccounts} 
                            setOpenAccounts={setOpenAccounts} 
                            activeTabKey= {activeTabKey} />  
        </div>
    );
}

export default TabDetails;