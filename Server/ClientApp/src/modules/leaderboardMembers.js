import axios from 'axios';

const ERROR = 'leaderboardMembers/ERROR';
const SEARCH = 'leaderboardMembers/SEARCH';
const UPDATE = 'leaderboardMembers/UPDATE';
const DEL = 'leaderboardMembers/DEL';
const CLEAR = 'leaderboardMembers/CLEAR';

export const search = (leaderboardId, start, stop) => async (dispatch) => {
    try {
        const res = await axios.get(`/api/leaderboards/${leaderboardId}/members?start=${start}&stop=${stop}`);

        dispatch({ type: SEARCH, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const update = (leaderboardId, member, score) => async (dispatch) => {
    try {
        const res = await axios.put(`/api/leaderboards/${leaderboardId}/members/${member}/score/${score}`);

        dispatch({ type: UPDATE, payload: res.data.data })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const del = (leaderboardId, member) => async (dispatch) => {
    try {
        const res = await axios.delete(`/api/leaderboards/${leaderboardId}/members/${member}`);

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
    leaderboardMembers: null,
};

const leaderboardMembers = (state = initialState, action) => {
    switch (action.type) {
        case SEARCH:
            return {
                ...state,
                leaderboardMembers: action.payload,
                loading: false,
            };
        case UPDATE:
            return {
                ...state,
                leaderboardMembers: state.leaderboardMembers.map(leaderboardMember => leaderboardMember.member === action.payload.member ? action.payload : leaderboardMember),
                loading: false
            };
        case DEL:
            return {
                ...state,
                leaderboardMembers: state.leaderboardMembers.filter(leaderboardMember => leaderboardMember.member !== action.payload.member)
            };
        case CLEAR:
            return {
                ...state,
                leaderboardMembers: null,
            };
        case ERROR:
            return {
                ...state,
            };
        default:
            return state;
    }
};

export default leaderboardMembers;