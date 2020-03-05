import React, { useState, useEffect } from 'react';
import BaseTable from '../Common/BaseTable';

import useUsers from '../../hooks/useUsers';

export default function Main() {
    const { view, onGet, onAdd, onDel, onClear } = useUsers();

    useEffect(() => {
        onGet();
    }, []);

    useEffect(() => {
        return () => {
            onClear();
        }
    }, []);

    const [columns, setColumns] = useState([
        {
            title: 'Id',
            field: 'id',
            customSort: (a, b) => {
                var left = a.id.toUpperCase();
                var right = b.id.toUpperCase();

                return left === right ? 0 : left > right ? 1 : -1;
            },
        },
    ]);

    return (
        <BaseTable
            columns={columns}
            view={view.users}
        />
    );
}