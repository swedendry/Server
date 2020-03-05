import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import useAuth from '../hooks/useAuth';

export default function Main({ component: Component, ...rest }) {
    const { view } = useAuth();

    return (
        <Route
            {...rest}
            render={props =>
                view.isAuthenticated
                    ? <Component {...props} />
                    : <Redirect to={{ pathname: '/login', state: { from: props.location } }} />
            }
        />
    )
};