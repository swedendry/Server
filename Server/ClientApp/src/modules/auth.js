import axios from 'axios';

import { getToken, setToken, delToken } from '../services/auth';

const CHECK_SUCCESS = 'auth/CHECK_SUCCESS';
const CHECK_FAIL = 'auth/CHECK_FAIL';
const LOGIN_SUCCESS = 'auth/LOGIN_SUCCESS';
const LOGIN_FAIL = 'auth/LOGIN_FAIL';
const LOGOUT = 'auth/LOGOUT';

export const check = () => async (dispatch) => {
    try {
        const res = await axios.get('/api/auth');

        dispatch({ type: CHECK_SUCCESS, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: CHECK_FAIL })
    }
}

export const login = (id, password) => async (dispatch) => {
    try {
        const data = { id, password };
        const res = await axios.post('/api/auth/login', data);
        if (res.data.code === 0)
            dispatch({ type: LOGIN_SUCCESS, payload: res.data.data })
        else
            dispatch({ type: LOGIN_FAIL })
    } catch (e) {
        dispatch({ type: LOGIN_FAIL })
    }
};

export const logout = () => ({ type: LOGOUT });

const initialState = {
    token: getToken(),
    isAuthenticated: false,
    user: null,
    loading: true,
};

const auth = (state = initialState, action) => {
    switch (action.type) {
        case CHECK_SUCCESS:
            return {
                ...state,
                isAuthenticated: true,
                loading: false,
                user: { ...state.user, id: action.payload.id, password: action.payload.password }
            };
        case LOGIN_SUCCESS:
            setToken(action.payload.token);
            return {
                ...state,
                token: action.payload.token,
                isAuthenticated: true,
                loading: false,
                user: { ...state.user, id: action.payload.user.id, password: action.payload.user.password }
            };
        case CHECK_FAIL:
        case LOGIN_FAIL:
        case LOGOUT:
            delToken();
            return {
                ...state,
                token: null,
                isAuthenticated: false,
                loading: false,
                user: null,
            };
        default:
            return state;
    }
};

export default auth;