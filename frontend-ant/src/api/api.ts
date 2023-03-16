import axios from "axios";
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
    accountGroupNameEn:	string | null,
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
                    //console.log(response.status);
                    //console.log(response.data);
                    return response;
                });
    },
    addAccountGroup(accountGroup: AccountGroupType) {
        debugger
        return instancePrivate.post<AccountGroupType>(`${BASE_URL}/AccountGroups`,
        accountGroup
        ).then(response => console.log(response));
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

export const authAPI = {
    login (userName: string, password: string) {
        return instance.post<LoginResponseType>(`${BASE_URL}/Authentication/sign-in`, { userName, password })
        .then(
            response => {
                token = response.data.token;
                saveJwtCookie(token); 
                console.log("function get saved token from cookie:" + CookieService.get('jwtAccess_token'));
                return token;
            }
        );     
    },
    /*logout () {
        return instan.delete(`Authentication/sign-in`)
    }*/
}