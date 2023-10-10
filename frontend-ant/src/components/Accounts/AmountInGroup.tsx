import React from "react";
import {accountType, AccountGroupType } from '../../api/api';

interface IAmountInGroupProps {
    accountList: accountType[];
    accountGroupData?: AccountGroupType | null;
}

const AmountInGroup: React.FC<IAmountInGroupProps> = ({accountList, accountGroupData}) => {
    const groupedBalances = accountList.reduce(
        (accumulator: Record<string, number>, account: accountType) => {
          const currencyName = account.currencyName;
          const balance = account.balance;
    
          if (!accumulator[currencyName]) {
            accumulator[currencyName] = 0;
          }
    
          accumulator[currencyName] += balance;
    
          return accumulator;
        },
        {}
      );
    
    const result = Object.entries(groupedBalances).map(
        ([currencyName, totalBalance]) => ({
          CurrencyName: currencyName,
          TotalBalance: totalBalance,
        })
    );

    return (           
    <div className="amount-container">
      <div className='amount-container-header'>
        amount in {accountGroupData?.accountGroupNameEn} group
      </div>
      {result.map((currencyGroup) => (
        <div key={currencyGroup.CurrencyName}>
          {currencyGroup.CurrencyName}:{' '}
          {currencyGroup.TotalBalance}
        </div>
      ))}
    </div>
  );
}

export default AmountInGroup;