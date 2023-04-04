import axios from "axios";
import { getMaxListeners } from "process";
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
        'Access-Control-Allow-Origin': 'origin-list',
      },
});
const instancePrivate = axios.create({
    baseURL: BASE_URL,
    headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*/*',
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
    typeId: number
    nameUa: string
    nameRu: string
    nameEn: string
    noteUa: string | null
    noteEn: string | null
    typeOrederBy: number
}]

export type accountType = {
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
}
export type accountListType = Array<accountType> | undefined

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
    getListByGroupId(accountGroupId: string) {
        return instancePrivate.get<accountListType>(`${BASE_URL}/Accounts/${accountGroupId}`)
        .then( res=> {
            console.log(res.data)
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
    },
    getListByGroupId2(accountGroupId: string) {
        return instancePrivate.get<AccountGroupsResponseType>(`${BASE_URL}/Accounts/${accountGroupId}`)
        .then( res=> {
            console.log(res.data)
            return res.data} )
        .catch((err) => {
            console.log(err);
            return null})
    }
}

export type currencyType = [{   
        id: number,
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

export type UserAccountType = {
    firstName: string | null,
    lastName: string | null,
    userName:	string,
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
        
         return instancePrivate.get<UserAccountType>(`${BASE_URL}/Authentication/me`)
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
    }
    /*logout () {
        return instan.delete(`Authentication/sign-in`)
    }*/
}