import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Select, MenuItem } from '@material-ui/core';
import BaseTable from '../Common/BaseTable';

import useLeaderboards from '../../hooks/useLeaderboards';

export default function Main() {
    const { view, onGet, onAdd, onUpdate, onDel, onClear } = useLeaderboards();

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
            render: rowData => {
                var url = `leaderboards/${rowData.id}`;
                return (
                    <Link to={url}>
                        {rowData.id}
                    </Link>
                )
            }
        },
        {
            title: 'Order', field: 'order',
            lookup: { 0: 'Ascending', 1: 'Descending' },
            editComponent: props => (
                <Select
                    value={props.value}
                    onChange={e => props.onChange(parseInt(e.target.value))}
                    displayEmpty
                >
                    {
                        Object.keys(props.columnDef.lookup).map((key) =>
                            <MenuItem value={key}>{props.columnDef.lookup[key]}</MenuItem>)
                    }
                </Select>
            )
        },
        {
            title: 'ScoreType', field: 'scoreType',
            lookup: { 0: 'Overwriting', 1: 'HighScore', 2: 'Increment', 3: 'Decrement' },
            editComponent: props => (
                <Select
                    value={props.value}
                    onChange={e => props.onChange(parseInt(e.target.value))}
                    displayEmpty
                >
                    {
                        Object.keys(props.columnDef.lookup).map((key) =>
                            <MenuItem value={key}>{props.columnDef.lookup[key]}</MenuItem>)
                    }
                </Select>
            )
        },
    ]);

    return (
        <BaseTable
            columns={columns}
            view={view.leaderboards}
            editable={{
                onRowAdd: newData => onAdd(newData),
                onRowUpdate: (newData, oldData) => onUpdate(oldData.id, newData),
                onRowDelete: oldData => onDel(oldData.id),
            }}
        />
    );
}