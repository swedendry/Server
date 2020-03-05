import axios from 'axios';

// Token
export const TOKEN_KEY = "@wisecat";

// Is isAuthenticated
export const isAuthenticated = () => sessionStorage.getItem(TOKEN_KEY) !== null;

// Get Token
export const getToken = () => sessionStorage.getItem(TOKEN_KEY);

// Login
export const setToken = token => {
    sessionStorage.setItem(TOKEN_KEY, token);
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
};

// Logout
export const delToken = () => {
    sessionStorage.removeItem(TOKEN_KEY)
    delete axios.defaults.headers.common['Authorization'];
};

//// Set axios header
//export const setAuthToken = token => {
//    if (token) {
//        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
//    } else {
//        delete axios.defaults.headers.common['Authorization'];
//    }
//};