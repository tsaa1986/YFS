import React, { useEffect, useState } from "react";
import { Button, DatePicker, Form, Input, InputNumber, Modal, Radio, RadioChangeEvent, Select } from "antd";
import { AccountDataType } from "./AccountsList";
import { Value } from "sass";
import { accountType, account, ICategory, category, operationAccount } from "../../api/api";
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

const OperationForm: React.FC<IOperationFormProps> = ({open, setOpenOperationForm, selectedAccount, typeOperation}) => {
    const [formOperation] = Form.useForm();
    const [selectedTypeOperation, setSelectTypeOperation] = useState<TypeOperation>(typeOperation);
    const [openAccounts, setOpenAccounts] = useState<accountType[]>([]);
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
                setOpenAccounts(res);
              }
       })
    }, [])

    useEffect (()=>{
      console.log('effect:', typeOperation)
      setSelectTypeOperation(typeOperation)
      console.log('accountOperation',openAccounts)
    },[typeOperation])

    useEffect(()=>{
      formOperation.resetFields();
      formOperation.setFieldsValue({ 
        radioTypeTransacion: selectedTypeOperation, 
        withdrawFromAccountId: selectedAccount?.id,
        targetAccountId: selectedAccount?.id
      })
      console.log('selectedAccountFormOperation', selectedAccount)
    },[selectedAccount])

    useEffect (()=>{
      //console.log('effect:', typeTransaction)
      console.log('selectedType', selectedTypeOperation)
      formOperation.resetFields();
      formOperation.setFieldsValue({ 
        radioTypeTransacion: selectedTypeOperation, 
        withdrawFromAccountId: selectedAccount?.id,
        targetAccountId: selectedAccount?.id
      });
      //setSelectTypeTransaction(typeTransaction)
    },[selectedTypeOperation])

    const handleSubmitAddOperationForm = () => {
      console.log('handle add operation'
      );
      //console.log(formOperation.getFieldValue('nameAccount'));
      let accountId = 0;
      let targetAccountId = 0;
      switch (selectedTypeOperation)  {
        case TypeOperation.Income: accountId = formOperation.getFieldValue('targetAccountId');
          break;
        case TypeOperation.Expense: accountId = formOperation.getFieldValue('withdrawFromAccountId');
          break;
        case TypeOperation.Transfer: accountId = formOperation.getFieldValue('withdrawFromAccountId');
          targetAccountId = formOperation.getFieldValue('targetAccountId');
          break;
      }
      //let currencyId = openAccounts?.find(element => element.id)?.currencyId;
      //currencyId = currencyId != undefined ? currencyId : 0;
    
      operationAccount.add({
        "id": 0,
        "categoryId": formOperation.getFieldValue('categoryId'),
        "typeOperation": selectedTypeOperation,
        "accountId": accountId,
        "operationCurrencyId": 0,
        "operationAmount": formOperation.getFieldValue('amount'),
        "operationDate": formOperation.getFieldValue('operationDate'),
        "description": formOperation.getFieldValue('description'),
        "tag": formOperation.getFieldValue('tag')
      }, targetAccountId).then(response => {
          if (response.status === 200)
              {
                  //debugger
                  console.log(response.data)
                  //addAccount(response.data)
                  formOperation.resetFields()
                  setOpenOperationForm(false)
              }
      });  
    
    }

    return(
    <Modal
      open={open}
      title="Create a new transaction"
      okText="Create"
      cancelText="Cancel"
      onCancel={()=>{
        formOperation.resetFields()
        setOpenOperationForm(false)
      }}
      onOk={() => {
        formOperation
          .validateFields()
          .then(() => {
            handleSubmitAddOperationForm();
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
              radioTypeOperation: selectedTypeOperation, 
              withdrawFromAccountId: selectedAccount?.id, 
              targetAccountId: selectedAccount?.id,
              categoryId: selectedTypeOperation == 1 ? 5 : selectedTypeOperation == 2 ? 2 : -1
            }}>

            <Form.Item name="radioTypeOperation" className="collection-create-form_last-form-item">
              <Radio.Group 
                value={selectedTypeOperation} 
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
            hidden={ ((selectedTypeOperation == TypeOperation.Income)) ? true : false }
            rules={[{required: true, message: 'Please select WithdrawFromAccount'}]}>          
            <Select 
              //value={account?.key}
              onChange={(e:any)=> {
                console.log(e)
                //setSelectedAccountsType(e)}
                //selectedAccountType = e;
              }}
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
            hidden={ ((selectedTypeOperation == TypeOperation.Expense)) ? true : false }
            rules={[{required: true, message: 'Please select Target Account'}]}>
            <Select 
              onChange={(e:any)=> {
                console.log(e)
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
              } 
            }
              disabled = {selectedTypeOperation == 3 ? true : false }
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
               label="Date"
               initialValue={moment()}
               >
              <DatePicker format={dateFormat}/>
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