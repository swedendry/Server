import React from 'react';
import MaterialTable from 'material-table';

export default function Main({ columns, view, editable }) {
    return (
        <>
            {view ?
                <MaterialTable
                    title=""
                    columns={columns}
                    data={view}
                    options={{
                        pageSize: 10,
                    }}
                    editable={editable}
                /> :
                <MaterialTable
                    title=""
                    columns={columns}
                    options={{
                        pageSize: 10,
                    }}
                    editable={editable}
                />}
        </>
    );
}