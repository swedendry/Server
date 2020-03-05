import React, { useState, useEffect } from 'react';
import BaseTable from '../Common/BaseTable';

import useLeaderboardMembers from '../../hooks/useLeaderboardMembers';

export default function Main({ leaderboardId }) {
    const { view, onSearch, onDel, onClear } = useLeaderboardMembers();

    useEffect(() => {
        onSearch(leaderboardId, 0, 100);
    }, []);

    useEffect(() => {
        return () => {
            onClear();
        }
    }, []);

    const [columns, setColumns] = useState([
        {
            title: 'Rank',
            field: 'rank',
        },
        {
            title: 'Member', field: 'member',
            customSort: (a, b) => {
                var left = a.member.toUpperCase();
                var right = b.member.toUpperCase();

                return left === right ? 0 : left > right ? 1 : -1;
            },
        },
        {
            title: 'Score', field: 'score',
        },
    ]);

    return (
        <BaseTable
            columns={columns}
            view={view.leaderboardMembers}
            editable={{
                onRowDelete: oldData => onDel(leaderboardId, oldData.member),
            }}
        />
    );
}