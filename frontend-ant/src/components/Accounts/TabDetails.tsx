import { Button, Divider } from "antd";
import React, { useEffect, useState } from "react";
import { AccountGroupType, accountType } from "../../api/api";
import { AccountsList } from "./AccountsList";

interface tabDetailsPropsType {
    accountGroupData: AccountGroupType | null
    activeTabKey: string
    openAccounts: accountType[] | undefined
    setOpenAccounts: React.Dispatch<React.SetStateAction<accountType[]>>
}

const TabDetails: React.FC<tabDetailsPropsType> = ({accountGroupData, activeTabKey , openAccounts, setOpenAccounts}) => {

    useEffect(()=>{
      console.log('tabdetails: ', accountGroupData)
      console.log('tabdetails: openAccounts', openAccounts)
      console.log('tabdetails: activetabkey change', activeTabKey)
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