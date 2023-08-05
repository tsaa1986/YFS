import React, { useEffect, useState } from 'react';
import { Form, Input, Modal, InputNumber, Checkbox, Select, DatePicker } from "antd";
import { account, accountTypesResponseType, bankType, currency, currencyType} from '../../api/api';

type initialItemsType = {
    label: string,
    children: JSX.Element,
    key: string,
    closable: false,
}[]

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


const AccountAddForm: React.FC<AddAccountFormPropsType> = (props) => {
    const [accountClosed, setAccountClosed] = useState<boolean>(false);
    const [accountName, setAccountName] = useState('');
    const [accountTypes, setAccountTypes] = useState<accountTypesResponseType>();
    const [currencies, setCurrencies] = useState<currencyType>();
    const [banks, setBanks] = useState<bankType>();
    const [selectedFavorites, setSelectedFavorites] = useState(false); 
    const [selectedAccountGroup, setSelectedAccountGroup] = useState(props.activeAccountsGroupKey);    
    const [selectedAccountType, setSelectedAccountType] = useState(0); 
    const [selectedBank, setSelectedBank] = useState(1); 

    useEffect(()=> {  
      account.getAccountTypes().then(
        res => {
          if (res != undefined){
            console.log(res)
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
                //debugger
                accountClosed == false ? props.onCreate(1, selectedFavorites) : props.onCreate(0, selectedFavorites); //account disable check
                //props.onCreate(values);
                setSelectedFavorites(false);
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
          initialValues={{ accountGroupId: props.activeAccountsGroupKey, bankId: 1 }}
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
                checked={selectedFavorites}
                onChange={ (e) => setSelectedFavorites(e.target.checked)}
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
                setSelectedAccountGroup(e)}
              } 
              value={selectedAccountGroup} 
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
            <Select 
                onChange={(e:any)=> {
                    console.log(e)
                    setSelectedAccountType(e)
                }}
                value={selectedAccountType}
            >
              { (accountTypes !== undefined) ? (accountTypes.map( d => {return <Select.Option value={d.accountTypeId}>{d.nameEn}</Select.Option>}
                )) : (<Select.Option value={''}>{'Choose types'}</Select.Option>)
              }
            </Select>
          </Form.Item>
          <Form.Item 
            name="bankId"
            label="Bank">
            <Select
                            onChange={(e:any)=> {
                                console.log(e)
                                setSelectedBank(e)
                            }}
                            value={selectedBank}
            >
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
              { (currencies !== undefined) ? (currencies.map( item => {return <Select.Option value={item.currencyId}>{item.name_en}</Select.Option>}
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

export default AccountAddForm;