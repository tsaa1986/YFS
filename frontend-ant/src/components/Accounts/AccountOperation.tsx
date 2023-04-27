import React, { useEffect, useState } from "react";
import { Button, DatePicker, Form, Input, InputNumber, Modal, Radio, RadioChangeEvent, Select, TreeSelect } from "antd";
import { AccountDataType } from "./AccountsList";
import { Value } from "sass";
import { accountType, account, ICategory, category, operationAccount, IOperation } from "../../api/api";
import moment from "moment";
import { TreeNode } from "antd/es/tree-select";
import { stringify } from "querystring";
import { TreeNodeNormal } from "antd/es/tree/Tree";
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
    selectedAccount: accountType | undefined
    typeOperation: TypeOperation
    setAddedOperation: React.Dispatch<React.SetStateAction<IOperation[] | undefined>>
    //accountGroup: AccountGroupType
    //typeTransaction: TypeTransaction;
    //onChangeTypeTransaction: (typeTransaction: TypeTransaction) => void;
  }

interface ICategoryTree {
  title: string,
  value: number,
  key: number,
  rootId: number,
  children?: TreeNodeNormal[];
}

const OperationForm: React.FC<IOperationFormProps> = ({open, setOpenOperationForm, selectedAccount, typeOperation, setAddedOperation}) => {
    const [formOperation] = Form.useForm();
    const [selectedTypeOperation, setSelectTypeOperation] = useState<TypeOperation>(typeOperation);
    const [openAccounts, setOpenAccounts] = useState<accountType[]>([]);
    const [categoryList, setCategoryList] = useState<ICategory[] | null>([]);
    const [selectedCatagoryId, setSelectedCategoryId] = useState<number>(0);
    const [categoryTreeData, setCategoryTreeData] = useState<[]>([]);//useState<ICategoryTree[]>([]);

    useEffect(()=>{
      category.getCategoryListByUserId().then( res => {
        if (res != undefined) {
          let list:any
          res.map(r=>list.push(r));
          buildCategoryTreeData(list)
          setCategoryList(res)        
        }
      })
    },[])



    useEffect(() => {
      //treeCategoryData = [{ title: '', value: 0,  key: 0}]
     // buildCategoryTreeData();
    }, [])

    useEffect(() => {
      account.getListOpenAccountByUserId().then(res => {
        if (res != undefined)
          {                
            setOpenAccounts(res);
          }
       })
    }, [])

    useEffect (()=>{
      setSelectTypeOperation(typeOperation)
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
      console.log('selectedType', selectedTypeOperation)
      formOperation.resetFields();
      formOperation.setFieldsValue({ 
        radioTypeTransacion: selectedTypeOperation, 
        withdrawFromAccountId: selectedAccount?.id,
        targetAccountId: selectedAccount?.id
      });
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

      operationAccount.add({
        "id": 0,
        "categoryId": formOperation.getFieldValue('categoryId'),
        "typeOperation": selectedTypeOperation,
        "accountId": accountId,
        "operationCurrencyId": 0,
        "operationAmount": formOperation.getFieldValue('amount'),
        "operationDate": formOperation.getFieldValue('operationDate'),
        "balance": 0,
        "description": formOperation.getFieldValue('description'),
        "tag": formOperation.getFieldValue('tag')
      }, targetAccountId).then(response => {
          if (response.status === 200)
              {
                  //debugger
                  console.log(response.data)
                  //addAccount(response.data)
                  formOperation.resetFields()
                  if (response.data != undefined)
                    {
                      setAddedOperation(response.data)
                    };   
                                 
                  setOpenOperationForm(false)
              }
      });  
    
    }

    const buildCategoryTreeData = (_category: []) => {
      let list:any = []
      _category.map( item => list.push(item));//{ title: item.name_ENG, value: item.id,  key: item.id, rootId: item.rootId, children: []}));

      let map: any = {}, node, roots:any = [];

      for (let i = 0; i < list.length; i+=1) {
        map[list[i].id] = i;
        list[i].children = [];
      }

      for (let i=0; i < list.length; i+= 1) {
        node = list[i];
        if (node.rootId !== 0) {
          // if you have dangling branches check that map[node.parentId] exists
          list[map[node.rootId]].children.push(node)//({ title: node.title, value: node.value,  key: node.value, rootId: node.rootId});
        } else {
          roots.push(node)//({ title: node.title, value: node.value,  key: node.value, rootId: node.rootId});
        }
      }
      //debugger
      //let list2:ICategoryTree[] = [];
      //roots.map(r => list2.map( {title: name_ENG, value: item.id,  key: item.id, rootId: item.rootId }))
      setCategoryTreeData(roots)
      /*let tempCategoryTree: ICategoryTree[] = [{ title: '', value: 0,  key: 0}];
      if (_category !== null) {
        _category.map( item => {
          { 
            if (item.rootId === 0)
                tempCategoryTree.push({title: item.name_ENG !== null ? item.name_ENG : 'empty',
                value: item.id,
                key: item.id})}
          }
        )
        setCategoryTreeData(tempCategoryTree)
      }*/
    }

    const treeData = [
      {
        title: 'Node1',
        value: '0-0',
        key: '0-0',
        children: [
          {
            title: 'Child Node1',
            value: '0-0-1',
            key: '0-0-1',
          },
          {
            title: 'Child Node2',
            value: '0-0-2',
            key: '0-0-2',
          },
        ],
      },
      {
        title: 'Node2',
        value: '0-1',
        key: '0-1',
      },
    ];

    const [value, setValue] = useState<string>();

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

            <TreeSelect 
              showSearch
              style={{width: '100%'}}
              value={selectedCatagoryId}
              treeNodeFilterProp='title'
              //key={selectedCatagoryId}
              dropdownMatchSelectWidth={false}
              dropdownStyle={{ maxHeight:400, overflow: 'auto'}}
              allowClear
              treeDefaultExpandAll
              treeData={categoryTreeData}     
              //treeData={}          
              onChange={(e:any, labelList: React.ReactNode[], ee)=> {     
                setSelectedCategoryId(e);
                //setValue(e)        
                console.log(e)
                //console.log(selectedCatagoryId)
                console.log(labelList)
                console.log(ee)
              } 
            }
              disabled = {selectedTypeOperation == 3 ? true : false }
            >
              { /*(categoryList !== null) ? (categoryList.map( item =>
                <TreeNode
                  value={item.id}
                  title={item.name_ENG}
                  key={item.id}
                  disabled
                >
                {//map(optGroup.data, (option) => (
                  //<TreeNode value={option} title={option} key={option} />
                  //))
                }
                </TreeNode>)) : ""*/
              }

              {/* (categoryList !== null) ? (categoryList.map( item => 
                  { return <Select.Option value={item.id}>{item.name_ENG}</Select.Option>})) : ""  
              */}
            </TreeSelect>
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