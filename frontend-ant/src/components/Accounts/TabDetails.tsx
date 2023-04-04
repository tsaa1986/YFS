import { Button, Divider } from "antd";
import React, { useEffect, useState } from "react";
import { AccountGroupType } from "../../api/api";
import { AccountsList } from "./AccountsList";

type tabDetailsPropsType = {
    accountGroupData: AccountGroupType | null
    //accountListSelectedTab: accountListType
    //onTabSelected: (tab: AccountGroupType | null) => void
}

interface tabDetailsPropsType1 {
    accountGroupData: AccountGroupType | null
}

const TabDetails: React.FC<tabDetailsPropsType1> = ({accountGroupData}) => {
    const [activeTabKey, setActiveTabKey] = useState('0')

    useEffect(()=>{
      console.log('tabdetails: ', accountGroupData)
      console.log('tabdetails: ','props.accounts')
      //setActiveTabKey('')
    },[accountGroupData])
  
    return ( <div>
        {/*<Divider />
        /*<Button onClick={()=>{console.log(accountGroupData)}}>Get Account</Button>
        <Button onClick={()=>{console.log()}}>Get Accounts</Button>*/ }      
        <AccountsList accountGroupData={accountGroupData}/>
      </div>
    );
}

export default TabDetails;