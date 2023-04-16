import React, { useEffect, useState } from "react";
import { Button, DatePicker, Form, Input, InputNumber, Modal, Radio, RadioChangeEvent, Select } from "antd";
import { AccountDataType } from "./AccountsList";
import { Value } from "sass";
import { accountType, account, ICategory, category } from "../../api/api";
import moment from "moment";
const dateFormat = 'YYYY/MM/DD';

export enum TypeOperation {
    Expense = 1,
    Income = 2,
    Transfer = 3,
}

interface IAccountOperationProps {
    typeOperation: TypeOperation
}


interface Values {
    title: string;
    description: string;
    modifier: string;
}

interface IOperationFormProps {
    open: boolean;
    //onCreate: (values: Values) => void;
    //onCancel: () => void;
    setOpenOperationForm: React.Dispatch<React.SetStateAction<boolean>>
    selectedAccount: AccountDataType | undefined
    typeOperation: TypeOperation
    //accountGroup: AccountGroupType
    //typeTransaction: TypeTransaction;
    //onChangeTypeTransaction: (typeTransaction: TypeTransaction) => void;
  }

const OperationForm: React.FC<IOperationFormProps> = ({open,/*, onCreate,*/ setOpenOperationForm, selectedAccount, typeOperation}) => {
    const [formOperation] = Form.useForm();
    const [selectTypeOperation, setSelectTypeOperation] = useState<TypeOperation>(typeOperation);
    const [openAccounts, setOpenAccounts2] = useState<accountType[]>([]);
    const [categoryList, setCategoryList] = useState<ICategory[] | null>([]);

    useEffect(()=>{
      category.getCategoryListByUserId().then( res => {
        if (res != undefined) {
          setCategoryList(res)
          //console.log('category operationForm', res);
        }
      })
    },[])

    useEffect(() => {
      account.getListOpenAccountByUserId().then(res => {
            if (res != undefined)
              {                
                setOpenAccounts2(res);
              }
              //return []
       })
    }, [])

    useEffect (()=>{
      console.log('effect:', typeOperation)
      setSelectTypeOperation(typeOperation)
      console.log('accountOperation',openAccounts)
    },[typeOperation])

    useEffect(()=>{
      formOperation.resetFields();
      formOperation.setFieldsValue({ radioTypeTransacion: selectTypeOperation, 
        withdrawFromAccountId: selectedAccount?.id,
        targetAccountId: selectedAccount?.id})
      console.log('selectedAccountFormOperation', selectedAccount)
    },[selectedAccount])

    useEffect (()=>{
      //console.log('effect:', typeTransaction)
      console.log('selectedType', selectTypeOperation)
      formOperation.resetFields();
      formOperation.setFieldsValue({ radioTypeTransacion: selectTypeOperation, 
        withdrawFromAccountId: selectedAccount?.id,
        targetAccountId: selectedAccount?.id
      });
      //setSelectTypeTransaction(typeTransaction)
    },[selectTypeOperation])

    return(
    <Modal
      open={open}
      title="Create a new transaction"
      okText="Create"
      cancelText="Cancel"
      onCancel={()=>{
        formOperation.resetFields()
        //setSelectTypeTransaction(0)
        setOpenOperationForm(false)
      }}
      onOk={() => {
        formOperation
          .validateFields()
          .then((values) => {
            formOperation.resetFields();
            //onCreate(values);
          })
          .catch((info) => {
            console.log('Validate Failed:', info);
          });
      }}>
        <Form
            form={formOperation}
            layout="vertical"
            name="form_in_modal"
            size="small"
            initialValues={{ 
              radioTypeOperation: selectTypeOperation, 
              withdrawFromAccountId: selectedAccount?.id, 
              targetAccountId: selectedAccount?.id,
              categoryId: 1
            }}>

            <Form.Item name="radioTypeOperation" className="collection-create-form_last-form-item">
              <Radio.Group 
                value={selectTypeOperation} 
                onChange={(e: RadioChangeEvent) => {setSelectTypeOperation(e.target.value)}}>
                  <Radio value={TypeOperation.Expense} >Expense</Radio>
                  <Radio value={TypeOperation.Income} >Income</Radio>
                  <Radio value={TypeOperation.Transfer} >Transfer</Radio>
              </Radio.Group>
            </Form.Item>

            <Form.Item 
            name="withdrawFromAccountId"
            label="WithdrawFromAccount"           
            //initialValue={selectedAccount?.id}
            hidden={ ((selectTypeOperation == TypeOperation.Income)) ? true : false }
            rules={[{required: true, message: 'Please select WithdrawFromAccount'}]}>          
            <Select 
              //value={account?.key}
              onChange={(e:any)=> {
                console.log(e)
                //setSelectedAccountsType(e)}
                //selectedAccountType = e;
              }}//value={selectedAccountType} 
            >
              {
                  (openAccounts !== undefined) ? (openAccounts.map( item => {
                    return (item.id != 0) ? <Select.Option value={item.id}>{`GroupName: ${item.accountGroupId}  ${item.name}` + ` Balance = `+`${item.balance}`}</Select.Option> : ''}
                      )) : (<Select.Option value={0}>{'Sekect Account'}</Select.Option>)
              }
            </Select>
            </Form.Item>

            <Form.Item 
            name="targetAccountId"
            label="Target Account"
            //initialValue={selectedAccount?.id}
            hidden={ ((selectTypeOperation == TypeOperation.Expense)) ? true : false }
            rules={[{required: true, message: 'Please select Target Account'}]}>
            <Select 
              onChange={(e:any)=> {
                console.log(e)
                //setSelectedAccountsType(e)}
                //selectedAccountType = e;
              } }
            >
              {
                  (openAccounts !== undefined) ? (openAccounts.map( item => {
                    return (item.id != 0) ? <Select.Option value={item.id}>{`GroupName: ${item.accountGroupId}  ${item.name}` + ` Balance = `+`${item.balance}`}</Select.Option> : ''}
                      )) : (<Select.Option value={0}>{'Select Account'}</Select.Option>)
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
              { (categoryList !== null) ? (categoryList.map( item => 
                  { return <Select.Option value={item.id}>{item.name_ENG}</Select.Option>})) : ""  
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
               name="operationDate"
               label="Date">
              <DatePicker format={dateFormat} />
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
              <Input type="textarea" value={selectedAccount?.balance}/>
            </Form.Item>
        </Form>
      </Modal>
)
}
export default OperationForm;


const AccountTransaction: React.FC<IAccountOperationProps> = ({typeOperation}) => {
    const [typeOeration, setTypeOperation] = useState<TypeOperation>(typeOperation)
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

function dayjs(arg0: string) {
  throw new Error("Function not implemented.");
}
