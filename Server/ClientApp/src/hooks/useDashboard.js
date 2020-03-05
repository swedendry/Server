import { useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { toggle, changeView } from '../modules/dashboard';

export default function useDashboard() {
    const view = useSelector(state => state.dashboard, []);
    const dispatch = useDispatch();

    const onToggle = useCallback((dashboard) => dispatch(toggle(dashboard)), [dispatch]);
    
    return {
        view,
        onToggle,
    };
}