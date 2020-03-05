import { useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { get, add, update, del, clear } from '../modules/leaderboards';

export default function useLeaderboards() {
    const view = useSelector(state => state.leaderboards, []);
    const dispatch = useDispatch();

    const onGet = useCallback(() => dispatch(get()), [dispatch]);
    const onAdd = useCallback((leaderboard) => dispatch(add(leaderboard)), [dispatch]);
    const onUpdate = useCallback((leaderboardId, leaderboard) => dispatch(update(leaderboardId, leaderboard)), [dispatch]);
    const onDel = useCallback((leaderboardId) => dispatch(del(leaderboardId)), [dispatch]);
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