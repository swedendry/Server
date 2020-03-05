import axios from 'axios';

const ERROR = 'leaderboards/ERROR';
const GET = 'leaderboards/GET';
const ADD = 'leaderboards/ADD';
const UPDATE = 'leaderboards/UPDATE'
const DEL = 'leaderboards/DEL';
const CLEAR = 'leaderboards/CLEAR';

export const get = () => async (dispatch) => {
    try {
        const res = await axios.get(`/api/leaderboards`);

        dispatch({ type: GET, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const add = (leaderboard) => async (dispatch) => {
    try {
        const res = await axios.post(`/api/leaderboards`, leaderboard);

        dispatch({ type: ADD, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const update = (leaderboardId, leaderboard) => async (dispatch) => {
    try {
        const res = await axios.put(`/api/leaderboards/${leaderboardId}`, leaderboard);

        dispatch({ type: UPDATE, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const del = (leaderboardId) => async (dispatch) => {
    try {
        const res = await axios.delete(`/api/leaderboards/${leaderboardId}`);

        dispatch({ type: DEL, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const clear = () => async (dispatch) => {
    try {
        dispatch({ type: CLEAR })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

const initialState = {
    leaderboards: null,
};

const leaderboards = (state = initialState, action) => {
    switch (action.type) {
        case GET:
            return {
                ...state,
                leaderboards: action.payload,
                loading: false,
            };
        case ADD:
            return {
                ...state,
                leaderboards: [action.payload, ...state.leaderboards],
                loading: false
            };
        case UPDATE:
            return {
                ...state,
                leaderboards: state.leaderboards.map(leaderboard => leaderboard.id === action.payload.id ? action.payload : leaderboard),
                loading: false
            };
        case DEL:
            return {
                ...state,
                leaderboards: state.leaderboards.filter(leaderboard => leaderboard.id !== action.payload)
            };
        case CLEAR:
            return {
                ...state,
                leaderboards: null,
            };
        case ERROR:
            return {
                ...state,
            };
        default:
            return state;
    }
};

export default leaderboards;