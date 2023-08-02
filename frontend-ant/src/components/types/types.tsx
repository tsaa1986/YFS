import { Dispatch, SetStateAction } from "react";

export interface IUser {
    id: number;
    firstName: string;
    lastName: string;
    userName: string;    
    email: string;
    phoneNumber: string | null;
}

export interface HeaderTypeProps {
    isLoggedIn: Boolean,
    languageDisplay: String,
    setLanguageDisplay: Dispatch<SetStateAction<any>>
    user: IUser | null;
    //setisLoggedIn: Dispatch<SetStateAction<any>>
}

/*
export interface IAccount {
    id: number,
    favorites: number,
    accountGroupId: number,
    accountTypeId: number,
    currencyId: number,
    bankId: number,
    name: string,
    openingDate: Date,
    note: string,
    balance: number
}*/