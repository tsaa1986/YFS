import React, { useEffect, useRef, useState } from 'react';
import { Button, Tabs, Space,Table, Form, Input, Modal,InputNumber } from "antd";
import { accountGroups, AccountGroupsResponseType } from '../../api/api';

type TargetKey = React.MouseEvent | React.KeyboardEvent | string;

type AccountGroupType = {
    accountGroupId:	number
    userId: string
    accountGroupNameRu:	string | null
    accountGroupNameEn:	string
    accountGroupNameUa:	string | null
    groupOrederBy:	number
}

type tabDetailsPropsType = {
    accountData: AccountGroupType | null
}

export const TabDetails = (props:tabDetailsPropsType) => {
    //let [key1, setKey1] = useState(props.key);
    const dataSource = [
      {
        key: '1',
        name: 'Mike',
        age: 32,
        address: '10 Downing Street',
      },
      {
        key: '2',
        name: 'John',
        age: 42,
        address: '10 Downing Street',
      },
    ];
    
    const columns = [
      {
        title: 'Name',
        dataIndex: 'name',
        key: 'name',
      },
      {
        title: 'Age',
        dataIndex: 'age',
        key: 'age',
      },
      {
        title: 'Address',
        dataIndex: 'address',
        key: 'address',
      },
    ];
  
  
  
  return ( <div>
    Tab Details
    <br></br>
    <Button onClick={()=>{console.log(props.accountData)}}>Get Accounts</Button>
    <Space style={{ display: 'flex' }}>
      <h2>Accounts List</h2>
    </Space>
    <Table dataSource={dataSource} columns={columns}/>
    </div>
  );
  }
  

type initialItemsType = {
    label: string,
    children: JSX.Element,
    key: string,
    closable: false,
}[]

const initialItemsAccountsGroup: initialItemsType = [
    { label: 'Favorites', 
    children:  <div>dvi</div>,//<TabDetails accountData={null}/>, //() => {return(<div>dvi</div>)},//'Content of Tab 1', 
    key: '0', 
    closable: false,
    },
  ];

export const AccountsTab: React.FC = () => { 
    const [activeTabKey, setActiveTabKey] = useState('0');//useState(initialItemsAccountsGroup[0].key);
    const [itemsAccountsGroup, setItems] = useState(initialItemsAccountsGroup);
    const newTabIndex = useRef(0);

    useEffect( ()=>{ 
        console.log('SYNC_EFFECT_TABS');
        getAccountGroups();
      },[])
    
    useEffect(
        ()=> {
         console.log(activeTabKey);
        }, [activeTabKey]
     )
 
    const onChange = (newActiveKey: string) => {
       setActiveTabKey(newActiveKey);
     };
   
    const add = () => {
         const newActiveKey = `newTab${newTabIndex.current++}`;
         const newPanes = [...itemsAccountsGroup];
         //newPanes.push({ label: 'New Tab', children: <TabDetails />, key: newActiveKey
     //});
         setItems(newPanes);
         setActiveTabKey(newActiveKey);
     };

    const addTabAccountGroup = (accountGroupItem: AccountGroupType) => {
        const newActiveKey = accountGroupItem.accountGroupId.toString()//`newTab${newTabIndex.current++}`;
        const newPanes = [...itemsAccountsGroup];
        //newPanes.push({ label: 'New Tab', children: <TabDetails />, key: newActiveKey
    //});
        newPanes.push({ label: accountGroupItem.accountGroupNameEn, children: <TabDetails key={accountGroupItem.accountGroupNameEn} accountData={accountGroupItem}/>, 
        key: accountGroupItem.accountGroupId.toString(), closable: false });
        setItems(newPanes);
        setActiveTabKey(newActiveKey);
    };


    const buildTabAccountsGroup = (accData:any) => {
        let newActiveKey = '';//`newTab${newTabIndex.current++}`;
        const newPanes: any = [];//[...itemsAccountsGroup];
        //let indexTab = '';
        //newPanes.splice(0,1);
        if (accData.data[0] !== null)
          {
            newActiveKey = accData.data[0].accountGroupId.toString();
            accData.data.map( (m:AccountGroupType) => {
              newPanes.push({ label: m.accountGroupNameEn, children: <TabDetails key={m.accountGroupNameEn} accountData={m}/>, key: m.accountGroupId.toString(), closable: false });
            })
            //console.log(acc)
          //newPanes.push({ label: value.accountGroupNameUa, children: `tab`/*<AccountTabPanel/>*/ /*`Рахунки групи: ${value.accountGroupNameUa}`*/, key: `newTab${newTabIndex.current++}` });
          //  newPanes.push({ label: accData.data[0].accountGroupNameEn, children: <TabDetails key1={accData.data[0].accountGroupNameEn}/>, key: accData.data[0].accountGroupId });
          //  newPanes.push({ label: accData.data[1].accountGroupNameEn, children: <TabDetails key1={accData.data[1].accountGroupNameEn}/>, key: accData.data[1].accountGroupId });
        } //);
        //debugger
        //newPanes.push({ label: name, children: 'Content of new Tab', key: newActiveKey });
        setItems(newPanes);
        setActiveTabKey(newActiveKey);
        //setActiveKey(newActiveKey);
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
            return res
        }});
}

const [visibleAddGroupForm, setVisibleAddGroupForm] = useState(false);

const showModalAddGroupForm = () => {
    setVisibleAddGroupForm(true)
  }
const handleCancelAddGroupForm = () => {
    setVisibleAddGroupForm(false)
    form.resetFields()
  }
const handleSubmitAddGroupForm = () => {
    console.log('handle');
    console.log(form.getFieldValue('nameAccount'));
  
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
    //addTabAccountsGroup
}

const [form] = Form.useForm();

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
        <Tabs defaultActiveKey='0'
            type="editable-card"
            onChange={onChange}
            activeKey={activeTabKey}
            onEdit={onEdit}
            items={itemsAccountsGroup}
            />
        <AddAccountGroupForm 
            visible={visibleAddGroupForm}
            onCancel={handleCancelAddGroupForm}
            onCreate={handleSubmitAddGroupForm}
            form={form}>
        </AddAccountGroupForm>

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
        visible={props.visible}
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
  
/*}
export const ModalWithFormExample: React.FC = () => {
    const [visible, setVisible] = useState(false);
    const [form] = Form.useForm();
      
    const showModal = () => {
        setVisible(true)
    }
    
    const handleSubmit = (values:any) => {
        console.log(values)
    }
      
    const handleCancel = () => {
        setVisible(false)
        form.resetFields()
    };
      
    return (
        <>
          <Button onClick={showModal}>Open Modal</Button>
          <Modal visible={visible} onOk={form.submit} onCancel={handleCancel}>
            <Form form={form} onFinish={handleSubmit}>
            <Form.Item
            label="Name AccountGroup"
            rules={[{required: true, message: 'Please input accountgroup name!'}]}>
              <Input />
            </Form.Item>
              {/* Any input }*/
     /*       </Form>
          </Modal>
        </>
      )
    }
*/