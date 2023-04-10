import React, { useState } from "react";
import { Button, Form, Input, Modal, Radio } from "antd";

enum TypeTransaction {
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
    onCreate: (values: Values) => void;
    onCancel: () => void;
  }

const TransactionForm: React.FC<TransactionFormProps> = ({open, onCreate, onCancel}) => {
    const [formTransaction] = Form.useForm();
    return(
    <Modal
      open={open}
      title="Create a new collection"
      okText="Create"
      cancelText="Cancel"
      onCancel={onCancel}
      onOk={() => {
        formTransaction
          .validateFields()
          .then((values) => {
            formTransaction.resetFields();
            onCreate(values);
          })
          .catch((info) => {
            console.log('Validate Failed:', info);
          });
      }}>
        <Form
            form={formTransaction}
            layout="vertical"
            name="form_in_modal"
            initialValues={{ modifier: 'public' }}>
            <Form.Item
            name="title"
            label="Title"
            rules={[{ required: true, message: 'Please input the title of collection!' }]}
            >
            <Input />
            </Form.Item>
            <Form.Item name="description" label="Description">
            <Input type="textarea" />
            </Form.Item>
            <Form.Item name="modifier" className="collection-create-form_last-form-item">
            <Radio.Group>
                <Radio value="public">Public</Radio>
                <Radio value="private">Private</Radio>
            </Radio.Group>
            </Form.Item>
        </Form>
      </Modal>
)
}


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
      <Button
        type="primary"
        onClick={() => {
          setOpen(true);
        }}
      >
        New Collection
      </Button>
      <TransactionForm
        open={open}
        onCreate={onCreate}
        onCancel={() => {
          setOpen(false);
        }}
      />
    </div>
    )
}