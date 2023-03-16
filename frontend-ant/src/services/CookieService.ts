//import Cookie from 'universal-cookie';
import Cookies from "universal-cookie/es6/Cookies"; 

const cookie = new Cookies();

class CookieSercise {
    get(key: string) {
        let jwt = cookie.get(key);
        if (jwt === undefined)
            return null 
        return jwt;
    }

    set(key: string, value: string, options: Object) {
        cookie.set(key, value, options);
    }

    remove(key: string) {
        cookie.remove(key);
    }
}

export default new CookieSercise();