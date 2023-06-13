import React, { useEffect, useRef, useState } from 'react';
import { Button, Tabs, Form, Input, Modal, InputNumber, Divider,
  Checkbox, Select, DatePicker } from "antd";
import { account, accountGroups, accountTypesResponseType, bankType, currency, currencyType,
accountListType, 
accountType} from '../../api/api';
import './AccountsLayout.css';
import TabDetails from './TabDetails';
import AccountAddForm from './AccountAddForm';

type TargetKey = React.MouseEvent | React.KeyboardEvent | string;

type AccountGroupType = {
    accountGroupId:	number
    userId: string
    accountGroupNameRu:	string | null
    accountGroupNameEn:	string
    accountGroupNameUa:	string | null
    groupOrederBy:	number
}

type initialItemsType = {
    label: string,
    children: JSX.Element,
    key: string,
    closable: false,
}[]

interface ITabItem {
  label: string,
  children: JSX.Element,
  key: string,
  closable: false,
}
type tabItems = ITabItem[];


export const AccountsTab: React.FC = () => { 
  const [activeTabKey, setActiveTabKey] = useState('0');
  const [accountListSelectedTab, setAccountListSelectedTab] = useState<accountListType>();
  const [openAccounts, setOpenAccounts] = useState<accountType[]>([])
  const initialItemsAccountsGroup: initialItemsType = [
    { label: 'Favorites', 
      children: <TabDetails key={'0'} 
                          accountGroupData={ {accountGroupId:0,   
                                                userId: '',
                                                accountGroupNameRu:	'',
                                                accountGroupNameEn:	'',
                                                accountGroupNameUa:	'',
                                                groupOrederBy:	0} }
                          activeTabKey={"0"}
                          openAccounts={openAccounts}
                          setOpenAccounts={setOpenAccounts}
      />,
    key: '0', 
    closable: false,
    }
  ];
  const [itemsAccountsGroup, setItems] = useState<initialItemsType>(initialItemsAccountsGroup);

  useEffect(() => {
    account.getListOpenAccountByUserId().then(res => {
          if (res != undefined)
            {                
              //debugger
              setOpenAccounts(res);
            }
            //return []
     })
  }, [])

  useEffect(()=>{
      console.log('SYNC_EFFECT_TABS');      
      getAccountGroups();
  }, [openAccounts])

  useEffect(()=>{
    console.log("change acive tab", activeTabKey)
  }, [activeTabKey])


  useEffect(()=> console.log('useeffect openaccout', openAccounts), [openAccounts])

  const onChangeActiveTab = (newActiveKey: string) => {
       setActiveTabKey(newActiveKey);
    };

  const addTabAccountGroup = (accountGroupItem: AccountGroupType) => {
        const newActiveKey = accountGroupItem.accountGroupId.toString()//`newTab${newTabIndex.current++}`;
        const newPanes = [...itemsAccountsGroup];
        newPanes.push({ label: accountGroupItem.accountGroupNameEn, 
          children: <TabDetails key={accountGroupItem.accountGroupNameEn} 
                                accountGroupData={accountGroupItem} 
                                activeTabKey={activeTabKey}
                                openAccounts={openAccounts} 
                                setOpenAccounts={setOpenAccounts}
                                />, 
          key: accountGroupItem.accountGroupId.toString(),
          closable: false });
        setItems(newPanes);
        setActiveTabKey(newActiveKey);
    };
  const buildTabAccountsGroup = (accData:any) => {
        let newActiveKey = '';//`newTab${newTabIndex.current++}`;
        //if (initialItemsAccountsGroup[0].key === '0')
            
        const newPanes: any = [...initialItemsAccountsGroup]//[...itemsAccountsGroup];
        //debugger
        if (accData.data[0] !== null)
          {
            newActiveKey = accData.data[0].accountGroupId.toString();
            accData.data.map( (m: AccountGroupType) => {
              newPanes.push({ label: m.accountGroupNameEn, 
                children: <TabDetails key={m.accountGroupNameEn} 
                                      accountGroupData={m} 
                                      openAccounts={openAccounts} 
                                      activeTabKey={activeTabKey}
                                      setOpenAccounts={setOpenAccounts}
                                      />, 
                key: m.accountGroupId.toString(), closable: false });
            })
        } //);
        setItems(newPanes);
        setActiveTabKey(newActiveKey);
}
  const remove = (targetKey: TargetKey) => {
    let newActiveKey = activeTabKey;
    let lastIndex = -1;
    itemsAccountsGroup.forEach((item, i) => {
      if (item.key === targetKey) {
        lastIndex = i - 1;
      }
    });

const newPanes = itemsAccountsGroup.filter((item) => item.key !== targetKey);
    if (newPanes.length && newActiveKey === targetKey) {
      if (lastIndex >= 0) {
        newActiveKey = newPanes[lastIndex].key;
      } else {
        newActiveKey = newPanes[0].key;
      }
    }
    setItems(newPanes);
    setActiveTabKey(newActiveKey);
};

  const onEdit = (
    targetKey: React.MouseEvent | React.KeyboardEvent | string,
    action: 'add' | 'remove',
  ) => {
    if (action === 'add') {
      //add();
      showModalAddGroupForm();
    } else {
      remove(targetKey);
    }
};
  const getAccountGroups = () => {
    accountGroups.get().then(
        res => {
            if (res != null ) {
              //debugger
              buildTabAccountsGroup(res)
        }});
}

const [visibleAddGroupForm, setVisibleAddGroupForm] = useState(false);
const [visibleAddAccountForm, setVisibleAddAccountForm] = useState(false);

const showModalAddGroupForm = () => {
    setVisibleAddGroupForm(true)
  }
const showModalAddAccountForm = () => {
    setVisibleAddAccountForm(true)
  }
const handleCancelAddGroupForm = () => {
    setVisibleAddGroupForm(false)
    form.resetFields()
  }
const handleCancelAddAccountForm = () => {
    setVisibleAddAccountForm(false)
    formAddAccount.resetFields()
  }
const handleSubmitAddGroupForm = () => {
    console.log('handle');
    console.log(form.getFieldValue('nameGroupAccount'));
  
    accountGroups.addAccountGroup({
      "accountGroupId": 0,
      "accountGroupNameEn": form.getFieldValue('nameAccount'),
      "accountGroupNameRu": form.getFieldValue('nameAccount'),
      "accountGroupNameUa": form.getFieldValue('nameAccount'),
      "groupOrederBy": form.getFieldValue('groupOrderBy'),
      "userId": "" 
    }).then(response => {
        if (response.status === 200)
            {
                console.log(response.data)
                addTabAccountGroup(response.data)
            }
    });
  
    form.resetFields()
    setVisibleAddGroupForm(false)
}
const handleSubmitAddAccountForm = (accountStatus: number, selectedFavorites: Boolean) => {
  console.log('handle');
  console.log(formAddAccount.getFieldValue('nameAccount'));
  //debugger
  account.add({
    "id": 0,
    "accountStatus": accountStatus,
    "favorites": selectedFavorites == true ? 1 : 0,
    "accountGroupId": formAddAccount.getFieldValue('accountGroupId'),
    "accountTypeId": formAddAccount.getFieldValue('accountTypeId'),
    "currencyId": formAddAccount.getFieldValue('currencyId'),
    "bankId": formAddAccount.getFieldValue('bankId'),
    "name": formAddAccount.getFieldValue('nameAccount'),
    "balance": formAddAccount.getFieldValue('balance'),
    "openingDate": formAddAccount.getFieldValue('openingDate'),//.toDateString(),
    "note": formAddAccount.getFieldValue('note')
  }).then(response => {
      if (response.status === 200)
          {
              //debugger
              console.log(response.data)
              //addAccount(response.data)
              formAddAccount.resetFields()
              setVisibleAddAccountForm(false)
          }
  });  

}

const AccountTabButton: Record<'left', React.ReactNode> = {
  left: <Button className="tabs-extra-demo-button" onClick={showModalAddAccountForm}>Add Account</Button>,
};

const [form] = Form.useForm();
const [formAddAccount] = Form.useForm();

return(<div>
        <div className="accountsTab__container">
        {/*<div className="button__container">
            <Button onClick={ getAccountGroups }>getAccountsGroup</Button>
            <Button onClick={ () => { 
              setItems([{ label: 'Tab Reset', children: <TabDetails accountData={null}/>, key: '0' }]);
              } }>reset Tabs</Button>
            <Button onClick={showModalAddGroupForm}
            >Add Account Group
            </Button>

        <ModalWithFormExample></ModalWithFormExample>
        </div>*/}
        <div className="accountsTab">
        <Tabs 
            type="editable-card"
            onChange={onChangeActiveTab}
            //activeKey={activeTabKey}
            accessKey={activeTabKey}
            onEdit={onEdit}
            tabBarExtraContent={AccountTabButton}
            items={itemsAccountsGroup}
            />

        <AddAccountGroupForm 
            visible={visibleAddGroupForm}
            onCancel={handleCancelAddGroupForm}
            onCreate={handleSubmitAddGroupForm}
            form={form}>
        </AddAccountGroupForm>
        <AccountAddForm 
            visible={visibleAddAccountForm}
            onCancel={handleCancelAddAccountForm}
            onCreate={handleSubmitAddAccountForm}
            form={formAddAccount}
            itemsAccountsGroup={itemsAccountsGroup}
            activeAccountsGroupKey={activeTabKey}>
        </AccountAddForm>

        </div>
    </div>
    </div>)
}


export const AccountsLayout: React.FC = () => {
    return (<div>
            <AccountsTab />
        </div>
    );
}


type AddAccountGroupFormPropsType = {
    visible: any,//Boolean | undefined, 
    onCancel:any, 
    onCreate:any, 
    form:any,
    children: React.ReactNode
  } 
  
const AddAccountGroupForm: React.FC<AddAccountGroupFormPropsType> = (props) => {
    //const {visible, onCancel, onCreate, form} = props
    const [accountGroupName, setAccountGroupName] = useState('');

    //debugger
    return(<div>
      <Modal
        open={props.visible}
        title="Create a new Account Group"
        okText="Create"
        onCancel={props.onCancel}
        onOk={() => {
            props.form
              .validateFields()
              .then((values: any) => {
                props.onCreate(values);
              })
              .catch((info: any) => {
                console.log("Validate Failed:", info);
              });
            }
          }
        //onOk={onCreate}
      >
        <Form layout='vertical' 
          form={props.form}
          >
          <Form.Item name="nameAccount"
            label='Name account group'
            rules={[{required: true, message: 'Please input account group name!'}]}>
            <Input 
                value={accountGroupName} 
                onChange={(e) => {setAccountGroupName(e.currentTarget.value)}}/>
          </Form.Item>

          <Form.Item name="groupOrderBy"
            label='Group Order By'>
            <InputNumber min={1} max={10} defaultValue={3} 
            />
          </Form.Item>

          <Form.Item 
            label='Description'>
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </div>);
  }

