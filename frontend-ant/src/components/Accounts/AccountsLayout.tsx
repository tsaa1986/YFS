import React, { useEffect, useRef, useState } from 'react';
import { Button, Tabs, Form, Input, Modal, InputNumber, Divider,
  Checkbox, Select, DatePicker } from "antd";
import { account, accountGroups, accountTypesResponseType, bankType, currency, currencyType,
accountListType } from '../../api/api';
import './AccountsLayout.css';
import TabDetails from './TabDetails';

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
/*
interface Person {
   name: string
   age: number
}
type Workplace = Person[]*/ 

const initialItemsAccountsGroup: initialItemsType = [
    { label: 'Favorites', 
    children:  <TabDetails key={'0'} accountGroupData={ {accountGroupId:0,   
      userId: '',
      accountGroupNameRu:	'',
      accountGroupNameEn:	'',
      accountGroupNameUa:	'',
      groupOrederBy:	0}}/>,//<TabDetails accountData={null}/>, //() => {return(<div>dvi</div>)},//'Content of Tab 1', 
    key: '0', 
    closable: false,
    }
  ];


export const AccountsTab: React.FC = () => { 
  const [activeTabKey, setActiveTabKey] = useState('0');
  const [itemsAccountsGroup, setItems] = useState<initialItemsType>([]);//(initialItemsAccountsGroup);
  //const [itemsAccountsGroup2, setItems2] = useState<tabItems>([]);
  //const newTabIndex = useRef(0);
  const [accountListSelectedTab, setAccountListSelectedTab] = useState<accountListType>([]);

    useEffect( ()=>{ 
        console.log('SYNC_EFFECT_TABS');
        getAccountGroups();
      },[])

    useEffect(
        ()=> {
          //console.log(activeTabKey);
          //debugger
          let tempAccountList: any = account.getListByGroupId(activeTabKey);
          //setAccountListSelectedTab(tempAccountList);
          //console.log('useeffect accountsTab:',accountListSelectedTab)
        }, [activeTabKey]
     )

     useEffect(()=>{

     },[accountListSelectedTab])

    const onChange = (newActiveKey: string) => {
       setActiveTabKey(newActiveKey);
     };
    const addTabAccountGroup = (accountGroupItem: AccountGroupType) => {
        const newActiveKey = accountGroupItem.accountGroupId.toString()//`newTab${newTabIndex.current++}`;
        const newPanes = [...itemsAccountsGroup];
        newPanes.push({ label: accountGroupItem.accountGroupNameEn, 
          children: <TabDetails key={accountGroupItem.accountGroupNameEn} accountGroupData={accountGroupItem} />, 
          key: accountGroupItem.accountGroupId.toString(), 
          closable: false });
        setItems(newPanes);
        setActiveTabKey(newActiveKey);
    };
    const buildTabAccountsGroup = (accData:any) => {
        let newActiveKey = '';//`newTab${newTabIndex.current++}`;
        const newPanes: any = [...initialItemsAccountsGroup]//[...itemsAccountsGroup];
        //debugger
        if (accData.data[0] !== null)
          {
            newActiveKey = accData.data[0].accountGroupId.toString();
            accData.data.map( (m:AccountGroupType) => {
              newPanes.push({ label: m.accountGroupNameEn, 
                children: <TabDetails key={m.accountGroupNameEn} accountGroupData={m} />, 
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
            buildTabAccountsGroup(res)
            //return res
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
const handleSubmitAddAccountForm = () => {
  console.log('handle');
  console.log(formAddAccount.getFieldValue('nameAccount'));

  account.add({
    "id": 0,
    "favorites": formAddAccount.getFieldValue('favorites'),
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
            onChange={onChange}
            activeKey={activeTabKey}
            onEdit={onEdit}
            tabBarExtraContent={AccountTabButton}
            items={itemsAccountsGroup}/>
        <AddAccountGroupForm 
            visible={visibleAddGroupForm}
            onCancel={handleCancelAddGroupForm}
            onCreate={handleSubmitAddGroupForm}
            form={form}>
        </AddAccountGroupForm>
        <AddAccountForm 
            visible={visibleAddAccountForm}
            onCancel={handleCancelAddAccountForm}
            onCreate={handleSubmitAddAccountForm}
            form={formAddAccount}
            itemsAccountsGroup={itemsAccountsGroup}
            activeAccountsGroupKey={activeTabKey}>
        </AddAccountForm>

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

type AddAccountFormPropsType = {
    visible: any,//Boolean | undefined, 
    onCancel:any, 
    onCreate:any, 
    form:any,
    itemsAccountsGroup: initialItemsType,
    activeAccountsGroupKey: string,
    //itemsCurrency: Array<currencyType>,
    children: React.ReactNode
  } 

const AddAccountForm: React.FC<AddAccountFormPropsType> = (props) => {
    //const {visible, onCancel, onCreate, form} = props
    const [accountClosed, setAccountClosed] = useState<boolean>(false);
    const [accountName, setAccountName] = useState('');
    const [accountTypes, setAccountTypes] = useState<accountTypesResponseType>();
    const [currencies, setCurrencies] = useState<currencyType>();
    const [banks, setBanks] = useState<bankType>();
    const [selectedAccountType, setSelectedAccountsType] = useState(props.activeAccountsGroupKey);
    //let selectedAccountType = props.activeAccountsGroupKey;


    useEffect(()=> {  
      account.getAccountTypes().then(
        res => {
          if (res != undefined){
            setAccountTypes(res)
          }
        }
      );
      console.log(accountTypes)
    }, [])

    useEffect(()=> {  
      currency.getAllCurrencies().then(
        res => {
          if (res != undefined){
            setCurrencies(res)
          }
        }
      );
      console.log(currencies)
    }, [])

    const { TextArea } = Input;
    const { RangePicker } = DatePicker;
    //debugger
    return(<div>
      <Modal
        //visible={props.visible}
        open={props.visible}
        title="Create a new Account"
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
        <Checkbox
          checked={accountClosed}
          onChange={(e) => setAccountClosed(e.target.checked)}
        >
          Account is closed
        </Checkbox>
        <Form layout='vertical' 
          labelCol={{ span: 8 }}
          wrapperCol={{ span: 14 }}
          form={props.form}
          disabled={accountClosed}
          style={{ maxWidth: 600 }}
          initialValues={{ bankId: 1 }}
          >
          <Form.Item 
            name="nameAccount"
            label='Name account'
            rules={[{required: true, message: 'Please input Account name!'}]}>
            <Input 
                value={accountName} 
                onChange={(e) => {setAccountName(e.currentTarget.value)}}/>
          </Form.Item>
          <Form.Item name="favorites">
            <Checkbox
            checked={false}
            //onChange={(e) => setAccountClosed(e.target.checked)}
            >
            Account is favorite
          </Checkbox>
          </Form.Item>
          <Form.Item 
            name="accountGroupId"
            label="Account Group"
            rules={[{required: true, message: 'Please select Account Group!'}]}>
            <Select 
              onChange={(e:any)=> {
                console.log(e)
                setSelectedAccountsType(e)}
                //selectedAccountType = e;
              } 
              value={selectedAccountType} 
            >
              { (props.itemsAccountsGroup !== undefined) ? (props.itemsAccountsGroup.map( item => {
                return (item.key !== "0") ? <Select.Option value={item.key}>{item.label}</Select.Option> : ''}
                  )) : (<Select.Option value={''}>{''}</Select.Option>)
              }
            </Select>
          </Form.Item>
          <Form.Item 
            name="accountTypeId"
            label="Account Type"
            rules={[{required: true, message: 'Please select Account Type!'}]}>
            <Select onChange={(e:any)=>console.log(e)}>
              { (accountTypes !== undefined) ? (accountTypes.map( d => {return <Select.Option value={d.typeId}>{d.nameEn}</Select.Option>}
                )) : (<Select.Option value={'1'}>{'Choose types'}</Select.Option>)
              }
            </Select>
          </Form.Item>
          <Form.Item 
            name="bankId"
            label="Bank">
            <Select>
              { (banks !== undefined) ? (banks.map( item => {return <Select.Option value={item.id}>{item.name}</Select.Option>}
                )) : (<Select.Option value={'1'}>{'Test Bank'}</Select.Option>)
              }
            </Select>
          </Form.Item>
          <Form.Item 
            name="currencyId"
            label="Currency"
            rules={[{required: true, message: 'Please select Currency!'}]}>
            <Select >
              { (currencies !== undefined) ? (currencies.map( item => {return <Select.Option value={item.id}>{item.name_en}</Select.Option>}
                )) : (<Select.Option value={''}>{''}</Select.Option>)
              }
            </Select>
          </Form.Item>
          <Form.Item 
            name="balance"
            label="Openning Balance"
            rules={[{required: true, message: 'Please enter balance of account!'}]}>
            <InputNumber value={0.00}/>
          </Form.Item>
          <Form.Item 
            name="openingDate"
            label="Open Date Account">
            <DatePicker />
          </Form.Item>
          <Form.Item 
            name="note"
            label="Description">
            <TextArea rows={2} />
            </Form.Item>
          </Form>
      </Modal>
    </div>);
  }