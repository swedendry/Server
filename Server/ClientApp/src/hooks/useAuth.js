import { useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { check, login, logout } from '../modules/auth';

export default function useAuth() {
    const view = useSelector(state => state.auth, []);
    const dispatch = useDispatch();

    const onCheck = useCallback(() => dispatch(check()), [dispatch]);
    const onLogin = useCallback((id, password) => dispatch(login(id, password)), [dispatch]);
    const onLogout = useCallback(() => dispatch(logout()), [dispatch]);

    return {
        view,
        onCheck,
        onLogin,
        onLogout,
    };
}