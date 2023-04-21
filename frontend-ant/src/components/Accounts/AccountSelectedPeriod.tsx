import React, { Dispatch, SetStateAction, useEffect, useState } from "react";
import { DatePicker, Select, Space } from 'antd';
import type {DatePickerProps} from 'antd';
import { SizeType } from "antd/es/config-provider/SizeContext";
import moment from 'moment';
import dayjs from 'dayjs';
import { IDateOption, ISelectedDate } from "./AccountsList";

const { RangePicker } = DatePicker;
const dateFormat = 'YYYY-MM-DD';

interface IAccountSelectedPeriod {
    selectedDateOption: IDateOption
    setSelectedDateOption: Dispatch<SetStateAction<IDateOption>>//(selectedDate: ISelectedDate) => {}
}

export enum SelectedVariantPeriod {
    lastOperation10 = 1,
    currentMonth = 2,
    previousMonth = 3,
    last3Month = 4,
    rangeDates = 5
}

const selectOptions = [
    {value: SelectedVariantPeriod.lastOperation10, label: "Last 10 operation"},
    {value: SelectedVariantPeriod.currentMonth, label: "Current Month"},
    {value: SelectedVariantPeriod.previousMonth, label: "Previous Month"},
    {value: SelectedVariantPeriod.last3Month, label: "Last 3 month"},
    {value: SelectedVariantPeriod.rangeDates, label: "Range dates"},
]

const handleChange = (value: string) => {
    console.log(`selected ${value}`);
  };
const onChangeMonth: DatePickerProps['onChange'] = (date, dateString) => {
    console.log(date, dateString);
  };

const AccountSelectedPeriod: React.FC<IAccountSelectedPeriod> = ({selectedDateOption, setSelectedDateOption}) => {
    const [size, setSize] = useState<SizeType>('small');
    const [selectOption, setSelectOption] = useState<SelectedVariantPeriod>(1);
    const [picker, setPicker] = useState("month");
    const [today, setToday] = useState(new Date());
    
    useEffect( ()=> {
        let firstDate, lastDate: Date;
        switch(selectOption) {
            case 1: setSelectedDateOption( {period: { startDate: new Date(), endDate: new Date()}, dataOption: SelectedVariantPeriod.lastOperation10})
                break;
            case 2: 
                firstDate = new Date(today.getFullYear(), today.getMonth(), 1);
                lastDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
                setSelectedDateOption( {period: {startDate: firstDate, endDate: lastDate}, dataOption: SelectedVariantPeriod.currentMonth})
                break
            case 3: 
                firstDate = new Date(today.getFullYear(), today.getMonth() - 1, 1);
                lastDate = new Date(today.getFullYear(), today.getMonth(), 0);
                setSelectedDateOption( {period: { startDate: firstDate, endDate: lastDate}, dataOption: SelectedVariantPeriod.previousMonth})
                break
            case 4: 
                firstDate = new Date(today.getFullYear(), today.getMonth() - 2, 1);
                lastDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
                setSelectedDateOption( {period: { startDate: firstDate, endDate: lastDate}, dataOption: SelectedVariantPeriod.last3Month})
                break
            case 5:
                setSelectedDateOption( {period: { startDate: new Date(), endDate: new Date()}, dataOption: SelectedVariantPeriod.rangeDates})
                break
            default:
                firstDate = new Date()
                lastDate = new Date()
                break
        }
        //console.log(`date: ${selectedDate.startDate.getFullYear()}-${selectedDate.startDate.getMonth()}-${selectedDate.startDate.getDay()}`)
    }, [selectOption]
    )

    const RenderDataPicker = () => { switch(SelectedVariantPeriod[selectOption]) {
        case SelectedVariantPeriod[1]: return ``
        /*case SelectedVariant[2]: return <DatePicker 
                size={size} 
                value={dayjs(`${selectedDate.startDate.getFullYear()}-${selectedDate.startDate.getMonth()}-${selectedDate.startDate.getDay()}`)} 
                onChange={onChangeMonth} 
                picker="month" 
                disabled/>*/
        case SelectedVariantPeriod[5]: return <RangePicker />  
            default:
                return `${selectedDateOption.period.startDate.toDateString()} - ${selectedDateOption.period.endDate.toDateString()}`
                break
        //case SelectedVariant[3]: return <DatePicker size={size} value={dayjs(`${selectedDate.startDate.getFullYear()}-${selectedDate.startDate.getMonth()}-${selectedDate.startDate.getDay()}`)} onChange={onChangeMonth} picker="month" disabled/>  
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
        { RenderDataPicker() }
        </Space>
    )
}

export default AccountSelectedPeriod;

