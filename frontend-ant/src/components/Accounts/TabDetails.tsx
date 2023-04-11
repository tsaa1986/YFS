import { Button, Divider } from "antd";
import React, { useEffect, useState } from "react";
import { AccountGroupType } from "../../api/api";
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
}

const TabDetails: React.FC<tabDetailsPropsType1> = ({accountGroupData}) => {
    const [activeTabKey, setActiveTabKey] = useState('0')
    const [selectedAccount, setSelectedAccount] = useState();

    useEffect(()=>{
      console.log('tabdetails: ', accountGroupData)
      console.log('tabdetails: ','props.accounts')
      //setActiveTabKey('')
    },[accountGroupData])
  
    return (           
        <div>
            <AccountsList accountGroupData={accountGroupData}/>  
        </div>
    );
}

export default TabDetails;