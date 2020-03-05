import { useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { get, add, update, del, clear } from '../modules/users';

export default function useUsers() {
    const view = useSelector(state => state.users, []);
    const dispatch = useDispatch();

    const onGet = useCallback(() => dispatch(get()), [dispatch]);
    const onAdd = useCallback((user) => dispatch(add(user)), [dispatch]);
    const onUpdate = useCallback((id, user) => dispatch(update(id, user)), [dispatch]);
    const onDel = useCallback((id) => dispatch(del(id)), [dispatch]);
    const onClear = useCallback(() => dispatch(clear()), [dispatch]);

    return {
        view,
        onGet,
        onAdd,
        onUpdate,
        onDel,
        onClear,
    };
}