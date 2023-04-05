import React, { useState } from "react";
import { DatePicker, Select, Space } from 'antd';
import type {DatePickerProps} from 'antd';
import { SizeType } from "antd/es/config-provider/SizeContext";

interface IAccountSelectedPeriod {

}

const handleChange = (value: string) => {
    console.log(`selected ${value}`);
  };
const onChangeMonth: DatePickerProps['onChange'] = (date, dateString) => {
    console.log(date, dateString);
  };

const AccountSelectedPeriod: React.FC<IAccountSelectedPeriod> = () => {
    const [size, setSize] = useState<SizeType>('small');
    const [picker, setPicker] = useState("month");

    return (
        <Space wrap>
           <Select
                defaultValue="CMPeriod"
                style={{ width: 120 }}
                onChange={handleChange}
                options={[
                    { value: 'CMPeriod', label: 'CurrentMonth' },
                    { value: 'RangePeriod', label: 'diapazon date' },
                ]}
                size={size}
            />
            <DatePicker size={size} onChange={onChangeMonth} picker="month" />


        </Space>
    )
}

export default AccountSelectedPeriod;