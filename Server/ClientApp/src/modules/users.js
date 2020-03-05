import axios from 'axios';

const ERROR = 'users/ERROR';
const GET = 'users/GET';
const ADD = 'users/ADD';
const UPDATE = 'users/UPDATE'
const DEL = 'users/DEL';
const CLEAR = 'users/CLEAR';

export const get = () => async (dispatch) => {
    try {
        const res = await axios.get('/api/users');

        dispatch({ type: GET, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const add = (user) => async (dispatch) => {
    try {
        const res = await axios.post('/api/users', user);

        dispatch({ type: ADD, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const update = (id, user) => async (dispatch) => {
    try {
        const res = await axios.put(`/api/users/${id}`, user);

        dispatch({ type: UPDATE, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const del = (id) => async (dispatch) => {
    try {
        const res = await axios.delete(`/api/users/${id}`);

        dispatch({ type: DEL, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

const initialState = {
    users: null,
    loading: true,
};

export const clear = () => async (dispatch) => {
    try {
        dispatch({ type: CLEAR })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

const users = (state = initialState, action) => {
    switch (action.type) {
        case GET:
            return {
                ...state,
                users: action.payload,
                loading: false,
            };
        case ADD:
            return {
                ...state,
                users: [action.payload, ...state.users],
                loading: false
            };
        case UPDATE:
            return {
                ...state,
                users: state.users.map(user => user.id === action.payload.id ? action.payload : user),
                loading: false
            };
        case DEL:
            return {
                ...state,
                users: state.users.filter(user => user.id !== action.payload)
            };
        case CLEAR:
            return {
                ...state,
                users: null,
            };
        case ERROR:
            return {
                ...state,
            };
        default:
            return state;
    }
};

export default users;