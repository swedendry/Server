import { useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { search, update, del, clear } from '../modules/leaderboardMembers';

export default function useLeaderboardMembers() {
    const view = useSelector(state => state.leaderboardMembers, []);
    const dispatch = useDispatch();

    const onSearch = useCallback((leaderboardId, start, stop) => dispatch(search(leaderboardId, start, stop)), [dispatch]);
    const onUpdate = useCallback((leaderboardId, member, score) => dispatch(update(leaderboardId, member, score)), [dispatch]);
    const onDel = useCallback((leaderboardId, member) => dispatch(del(leaderboardId, member)), [dispatch]);
    const onClear = useCallback(() => dispatch(clear()), [dispatch]);

    return {
        view,
        onSearch,
        onUpdate,
        onDel,
        onClear,
    };
}