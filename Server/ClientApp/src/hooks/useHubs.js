import { useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { connect, stop, send } from '../modules/hubs';

export default function useNotices() {
    const view = useSelector(state => state.hubs, []);
    const dispatch = useDispatch();

    const onConnect = useCallback((url, id) => dispatch(connect(url, id)), [dispatch]);
    const onStop = useCallback((hubConnection) => dispatch(stop(hubConnection)), [dispatch]);
    const onSend = useCallback((hubConnection, message) => dispatch(send(hubConnection, message)), [dispatch]);

    return {
        view,
        onConnect,
        onStop,
        onSend,
    };
}