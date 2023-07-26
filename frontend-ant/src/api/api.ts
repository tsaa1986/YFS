import axios from "axios";
import moment from "moment";
import { getMaxListeners } from "process";
import { start } from "repl";
import { IUser } from "../components/types/types";
import CookieService from "../services/CookieService";

const BASE_URL = 'https://localhost:5001/api';
let token: string | null;

const options = {
    path: '/',
    secure: true
}

const saveJwtCookie = (jwtToken: string) => {
    CookieService.set('jwtAccess_token', jwtToken, options);
}

const instance = axios.create({
    withCredentials: false,
    baseURL: BASE_URL,
    headers: {
        'Authorization': "JWT_TOKEN",
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
      },
});
const instancePrivate = axios.create({
    baseURL: BASE_URL,
    headers: {
        'Authorization': "JWT_TOKEN",
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*',
        //'Access-Control-Allow-Credentials': 'true'
    },
    withCredentials: true,
})

export type AccountGroupType = {
    accountGroupId:	number,
    userId:	string,
    accountGroupNameRu:	string | null,
    accountGroupNameEn:	string,
    accountGroupNameUa:	string | null,
    groupOrederBy: number}

export type AccountGroupsResponseType = {
    map: any;
    data: Array<AccountGroupType>
    status: number
} 

export const accountGroups = {
    get() {
        token = CookieService.get('jwtAccess_token');
        instancePrivate.interceptors.request.use ( config => {
            config.headers.Authorization = `Bearer ${token}`;
            return config;
        });
        return instancePrivate.get<AccountGroupsResponseType>(`${BASE_URL}/AccountGroups`)
                .then(response => {
                    //debugger
                    //console.log(response.status);
                    //console.log(response.data);
                    return response;
                });
    },
    addAccountGroup(accountGroup: AccountGroupType) {
        //debugger
        return instancePrivate.post<AccountGroupType>(`${BASE_URL}/AccountGroups`,
        accountGroup
        )//.then(response => {return response}
    }
}

export type accountTypesResponseType = [{
    accountTypeId: number
    nameUa: string
    nameRu: string
    nameEn: string
    noteUa: string | null
    noteEn: string | null
    typeOrederBy: number
}]

export type accountType = {
    id: number,
    accountStatus: number,
    favorites: number,
    accountGroupId: number,
    accountTypeId: number,
    currencyId: number,
    currencyName: string,
    bankId: number,
    name: string,
    openingDate: Date,
    note: string,
    balance: number
}
export type accountListType = Promise<accountType[]> | undefined

interface IAccountListTypeResponse {
    data: Array<accountType>
}

export const account = {
    getAccountTypes() {

        return instance.get(`${BASE_URL}/AccountTypes`)
            .then(res=> {return res.data})
            .catch((err) => console.log(err))
    },
    add(account: accountType) {
        //debugger
    return instancePrivate.post<accountType>(`${BASE_URL}/Accounts`,
        account)
    },
    getListOpenAccountByUserId() {
        return instancePrivate.get<accountListType>(`${BASE_URL}/Accounts/openAccountsByUserId`)
        .then( res=> {
            //debugger
            //console.log(res.data)
            return res.data} )
        .catch((err) => {
            console.log(err)
            return undefined
        })
    },
    getListByGroupId(accountGroupId: string) {
        return instancePrivate.get<accountListType>(`${BASE_URL}/Accounts/${accountGroupId}`)
        .then( res=> {
            //console.log(res.data)
            return res.data} )
        .catch((err) => {
            console.log(err)
        })
    },
    getListByFavorites() {
        return instancePrivate.get<accountListType>(`${BASE_URL}/Accounts`)
        .then( res=> {
            console.log(res.data)
            return res.data} )
        .catch((err) => {
            console.log(err)
            return null
        })
    }
}

export interface ICategory {
    id: number,
    rootId: number | null,
    userId: string,
    name_UA: string | null,
    name_ENG: string | null,
    name_RU: string | null,
    note: string
}

interface ICategoryResponseType {
    data: Array<ICategory> | null
}

export const category = {
    getCategoryListByUserId() {
        return instancePrivate.get<ICategory[]>(`${BASE_URL}/Category`)
        .then( res => {
            //debugger
            //console.log(res.data)
            return res.data} )
        .catch((err) => {
            console.log(err)
            return null
        })
    },
}

export interface IOperation {
    id: number,
    categoryId: number,
    typeOperation: number,
    accountId: number,
    operationCurrencyId: number,
    operationAmount: number,
    operationDate: Date,
    balance: number,
    description: string | null,
    tag: string | null
}

export const operationAccount = {
    add(operation: IOperation, targetAccountId: number) {
    return instancePrivate.post<IOperation[]>(`${BASE_URL}/Operations/${targetAccountId}`,
        operation)
    },
    remove(id: number) {
        return instancePrivate.delete<IOperation[]>(`${BASE_URL}/Operations/${id}`)
    },
    removeTransfer(id: number) {
        return instancePrivate.delete<IOperation[]>(`${BASE_URL}/Operations/transfer/${id}`)
    },
    getOperationsAccountForPeriod(accountId: number, startDate: Date, endDate: Date) {
        let sDate: string = moment(startDate).format('YYYY-MM-DD');
        let eDate: string = moment(endDate).format('YYYY-MM-DD');
        return instancePrivate.get<Array<IOperation>>(`${BASE_URL}/Operations/period/${accountId}/${sDate}/${eDate}`)
        .then( res=> {
            //debugger
            return res.data} )
        .catch((err) => {
            console.log(err)
            return undefined
        })
    },
    getLast10OperationsAccount(accountId: number) {
        return instancePrivate.get<Array<IOperation>>(`${BASE_URL}/Operations/last10/${accountId}`)
        .then( res=> {
            
            return res.data} )
        .catch((err) => {
            console.log(err)
            return undefined
        })
    }
} 

export type currencyType = [{   
        currencyId: number,
        shortNameUs: string,
        name_ru: string,
        name_ua: string,
        name_en: string
}]

export type bankType = [{   
    id: number,
    name: string,
}]

export const currency = {
    getAllCurrencies() {

        return instance.get(`${BASE_URL}/Currency`)
            .then(res=> {
                return res.data
            })
            .catch((err) => console.log(err))
    }
}

type LoginResponseType = {
    token: string
    responseCode: ResponseCodesEnum
}

export enum ResponseCodesEnum {
    Success = 0,
    Error = 1
}

export type UserRegistrationType = {
    firstName: string | null,
    lastName: string | null,
    userName:	string,
    password:	string,
    email: string | null,
    phoneNumber: string | null
}

export const authAPI = {
    //me() who am i from jwt
    me () {
        let jwt: string | null = CookieService.get('jwtAccess_token');

        if (jwt !== null) {
            instancePrivate.interceptors.request.use ( config => {
                config.headers.Authorization = `Bearer ${jwt}`;
                return config;});
        
         return instancePrivate.get<IUser>(`${BASE_URL}/Authentication/me`)
            .then(response => { 
                //console.log(response.status);
                //debugger
                //console.log(response.data);
                return response.data;
            });
        }
    },
    login (userName: string, password: string) {
        return instance.post<LoginResponseType>(`${BASE_URL}/Authentication/sign-in`, { userName, password })
        .then(
            response => {
                token = response.data.token;

                if (response.status === 200){
                    saveJwtCookie(token); 
                    console.log("function get saved token from cookie:" + CookieService.get('jwtAccess_token'));
                    instancePrivate.interceptors.request.use ( config => {
                        config.headers.Authorization = `Bearer ${token}`;
                        return config;
                    });
                    return token;
                    }
                    else return false;
            }
        );     
    },
    signUp (user: UserRegistrationType) {
        console.log('user form: ', user);
        return (
            instance.post<any>(`${BASE_URL}/Authentication/sign-up`, user).then(
               response => { 
                if (response.status === 201)
                { 
                    this.login(user.userName, user.password).then(

                    );
                    
                    return response.status
                }
                }
            )
        )
    },
    logOut () {
        return CookieService.remove('jwtAccess_token');//instan.delete(`Authentication/sign-in`)
    }
}