import * as signalR from '@aspnet/signalr';

const ERROR = 'hubs/ERROR';
const CONNECT = 'hubs/CONNECT';
const STOP = 'hubs/STOP';
const CLOSE = 'hubs/CLOSE';
const LOGIN = 'hubs/LOGIN';
const SEND = 'hubs/SEND';
const RECEIVE = 'hubs/RECEIVE';

export const connect = (url, id) => async (dispatch) => {
    try {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${url}`)
            .configureLogging(signalR.LogLevel.Information)
            .build();

        hubConnection.on('Login', (pack) => {
            const text = `login ${pack.user.id}`;

            dispatch({ type: RECEIVE, payload: text })
        });

        hubConnection.on('Chat', (pack) => {
            //const data = JSON.stringify(pack);
            const text = `${pack.id} : ${pack.message}`;

            dispatch({ type: RECEIVE, payload: text })
        });

        hubConnection.onclose(e => {
            dispatch({ type: CLOSE })
        });

        hubConnection.start()
            .then(() => dispatch({ type: CONNECT, payload: { id, hubConnection } }))
            .then(() => dispatch(login(hubConnection, id)), [dispatch]);
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const stop = (hubConnection) => async (dispatch) => {
    try {
        hubConnection.stop();

        dispatch({ type: STOP })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const login = (hubConnection, id) => async (dispatch) => {
    try {
        const pack = { Id: id };

        hubConnection.invoke('Login', pack);

        dispatch({ type: LOGIN })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const send = (hubConnection, message) => async (dispatch) => {
    try {
        const pack = { Message: message };

        hubConnection.invoke('Chat', pack);

        dispatch({ type: SEND })
    }
    catch (e) {
        dispatch({ type: ERROR })
    }
}

export const error = () => ({ type: ERROR });

const initialState = {
    id: '',
    messages: [],
    hubConnection: null,
    isConnected: false,
};

const hubs = (state = initialState, action) => {
    switch (action.type) {
        case CONNECT:
            return {
                ...state,
                isConnected: true,
                id: action.payload.id,
                hubConnection: action.payload.hubConnection,
            };
        case LOGIN:
            return {
                ...state,
            };
        case SEND:
            return {
                ...state,
            };
        case RECEIVE:
            return {
                ...state,
                messages: [action.payload, ...state.messages],
            };
        //case STOP:
        case CLOSE:
        case ERROR:
            return {
                ...state,
                isConnected: false,
                id: '',
                hubConnection: null,
                messages: []
            };
        default:
            return state;
    }
};

export default hubs;