import React, { useEffect, useState } from "react";
import { Button, DatePicker, Form, Input, InputNumber, Modal, Radio, RadioChangeEvent, Select } from "antd";
import { AccountDataType } from "./AccountsList";
import { Value } from "sass";

export enum TypeTransaction {
    Expense = 1,
    Income = 2,
    Transfer = 3,
}

interface IAccountTransactionProps {
    typeTransaction: TypeTransaction
}


interface Values {
    title: string;
    description: string;
    modifier: string;
}

interface TransactionFormProps {
    open: boolean;
    //onCreate: (values: Values) => void;
    onCancel: () => void;
    account: AccountDataType | undefined
    //accountGroup: AccountGroupType
    //typeTransaction: TypeTransaction;
    //onChangeTypeTransaction: (typeTransaction: TypeTransaction) => void;
  }

const TransactionForm: React.FC<TransactionFormProps> = ({open,/*, onCreate,*/ onCancel, account}) => {
    const [formTransaction] = Form.useForm();
    const [typeTransaction, setTypeTransaction] = useState<TypeTransaction>(1);

    useEffect (()=>{
      console.log('effect:',typeTransaction)
    },[typeTransaction])

    return(
    <Modal
      open={open}
      title="Create a new transaction"
      okText="Create"
      cancelText="Cancel"
      onCancel={onCancel}
      onOk={() => {
        formTransaction
          .validateFields()
          .then((values) => {
            formTransaction.resetFields();
            //onCreate(values);
          })
          .catch((info) => {
            console.log('Validate Failed:', info);
          });
      }}>
        <Form
            form={formTransaction}
            layout="vertical"
            name="form_in_modal"
            size="small"
            initialValues={{ modifier: 'public' }}>
            <Form.Item name="modifier" className="collection-create-form_last-form-item">
              <Radio.Group onChange={(e: RadioChangeEvent) => {setTypeTransaction(e.target.value) }}>
                  <Radio value={TypeTransaction.Expense} >Expense</Radio>
                  <Radio value={TypeTransaction.Income} >Income</Radio>
                  <Radio value={TypeTransaction.Transfer} >Transfer</Radio>
              </Radio.Group>
            </Form.Item>

            <Form.Item 
            name="withdrawFromAccountId"
            label="WithdrawFromAccount"            
            hidden={ ((typeTransaction == TypeTransaction.Income)) ? true : false }
            rules={[{required: true, message: 'Please select WithdrawFromAccount'}]}>          
            <Select 
              onChange={(e:any)=> {
                console.log(e)
                //setSelectedAccountsType(e)}
                //selectedAccountType = e;
              }}//value={selectedAccountType} 
            >
              {/* (props.itemsAccountsGroup !== undefined) ? (props.itemsAccountsGroup.map( item => {
                return (item.key !== "0") ? <Select.Option value={item.key}>{item.label}</Select.Option> : ''}
                  )) : (<Select.Option value={''}>{''}</Select.Option>)*/
              }
            </Select>
            </Form.Item>

            <Form.Item 
            name="targetAccountId"
            label="Target Account"
            hidden={ ((typeTransaction == TypeTransaction.Expense)) ? true : false }
            rules={[{required: true, message: 'Please select Target Account'}]}>
            <Select 
              onChange={(e:any)=> {
                console.log(e)
                //setSelectedAccountsType(e)}
                //selectedAccountType = e;
              } }
              //value={selectedAccountType} 
            >
              {/* (props.itemsAccountsGroup !== undefined) ? (props.itemsAccountsGroup.map( item => {
                return (item.key !== "0") ? <Select.Option value={item.key}>{item.label}</Select.Option> : ''}
                  )) : (<Select.Option value={''}>{''}</Select.Option>)*/
              }
            </Select>
            </Form.Item>
            <Form.Item 
            name="categoryId"
            label="Category"
            rules={[{required: true, message: 'Please select Category'}]}>
            <Select 
              onChange={(e:any)=> {
                console.log(e)
                //setSelectedAccountsType(e)}
                //selectedAccountType = e;
              } }
              //value={selectedAccountType} 
            >
              {/* (props.itemsAccountsGroup !== undefined) ? (props.itemsAccountsGroup.map( item => {
                return (item.key !== "0") ? <Select.Option value={item.key}>{item.label}</Select.Option> : ''}
                  )) : (<Select.Option value={''}>{''}</Select.Option>)*/
              }
            </Select>
            </Form.Item>
            <Form.Item 
            name="amount"
            label="Amount"
            rules={[{required: true, message: 'Please enter amount!'}]}>
              <InputNumber value={0.00}/>
            </Form.Item>
            <Form.Item  
               name="transactionDate"
               label="Date">
              <DatePicker />
            </Form.Item>

            {/*<Form.Item>
              {/*}
            <Button onClick={() => {return <div>{account?.key}</div>}}>{account?.key}</Button>
            </Form.Item>*/}
            
            <Form.Item
            name="tag"
            label="Tag"
            >
            <Input />
            </Form.Item>

            <Form.Item name="description" label="Description">
            <Input type="textarea" value={account?.balance}/>
            </Form.Item>
        </Form>
      </Modal>
)
}
export default TransactionForm;


const AccountTransaction: React.FC<IAccountTransactionProps> = ({typeTransaction}) => {
    const [typeOeration, setTypeOperation] = useState<TypeTransaction>(typeTransaction)
    const [open, setOpen] = useState(false);

    const onCreate = (values: any) => {
        console.log('Received values of form: ', values);
        setOpen(false);
      };


    const [formAccountTransaction] = Form.useForm();
    return(
    <div>
    { /* <Button
        type="primary"
        onClick={() => {
          setOpen(true);
        }}
      >
        New Collection
      </Button>
      <TransactionForm
        open={open}
        //onCreate={onCreate}
        onCancel={() => {
         setOpen(false);
        }}
      />*/}
    </div>
    )
}