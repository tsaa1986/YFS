import React, { Dispatch, SetStateAction, useEffect, useState } from "react";
import { DatePicker, Select, Space } from 'antd';
import type {DatePickerProps} from 'antd';
import { SizeType } from "antd/es/config-provider/SizeContext";
import moment from 'moment';
import dayjs from 'dayjs';
import { ISelectedDate } from "./AccountsList";

const { RangePicker } = DatePicker;
const dateFormat = 'YYYY-MM-DD';

interface IAccountSelectedPeriod {
    setSelectedDate: Dispatch<SetStateAction<ISelectedDate>>//(selectedDate: ISelectedDate) => {}
}

enum SelectedVariant {
    lastOperation10 = 1,
    currentMonth = 2,
    previousMonth = 3,
    last3Month = 4,
    rangeDates = 5
}

const selectOptions = [
    {value: SelectedVariant.lastOperation10, label: "Last 10 operation"},
    {value: SelectedVariant.currentMonth, label: "Current Month"},
    {value: SelectedVariant.previousMonth, label: "Previous Month"},
    {value: SelectedVariant.last3Month, label: "Last 3 month"},
    {value: SelectedVariant.rangeDates, label: "Range dates"},
]

const handleChange = (value: string) => {
    console.log(`selected ${value}`);
  };
const onChangeMonth: DatePickerProps['onChange'] = (date, dateString) => {
    console.log(date, dateString);
  };

const AccountSelectedPeriod: React.FC<IAccountSelectedPeriod> = ({setSelectedDate}) => {
    const [size, setSize] = useState<SizeType>('small');
    const [selectOption, setSelectOption] = useState<SelectedVariant>(1);
    const [picker, setPicker] = useState("month");
    const [today, setToday] = useState(new Date());
    
    const today1 = dayjs(`${today.getFullYear()}-${today.getMonth()}-${today.getDay()}`)//new Date();

    useEffect( ()=> {
        console.log(SelectedVariant[selectOption])
        console.log(`${today.getFullYear()}-${today.getMonth()}-${today.getDay()}`)
        //console.log(selectedDate)
        setSelectedDate({startDate: new Date(), endDate: new Date()});
    }, [selectOption]
    )

    const GetDataPicker = () => { switch(SelectedVariant[selectOption]) {
        case SelectedVariant[2]: return <DatePicker size={size} value={dayjs(`${today.getFullYear()}-${today.getMonth()}-${today.getDay()}`)} onChange={onChangeMonth} picker="month" disabled/>
        case SelectedVariant[3]: return <DatePicker size={size} value={dayjs(`${today.getFullYear()}-${today.getMonth()-1}-${today.getDay()}`)} onChange={onChangeMonth} picker="month" disabled/>  
       // case SelectedVariant[4]: return <RangePicker size={size} value={[dayjs(`${today.getFullYear()}-${today.getMonth()}-${today.getDay()}`, dateFormat), dayjs('2015-06-06', dateFormat)]} disabled/>
        }   
    }

    return (
        <Space wrap>
           <Select
                defaultValue={1}
                style={{ width: 150 }}
                onChange={ (value) => {setSelectOption(value)} }
                options={ selectOptions }
                size={size}
            />
       { GetDataPicker()}

        </Space>
    )
}

export default AccountSelectedPeriod;

