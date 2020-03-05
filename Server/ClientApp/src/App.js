import React from 'react';
import { Router, Route, Switch } from "react-router-dom";
import history from './helpers/history';
import PrivateRoute from './helpers/privateRoute';

import Login from './views/Login';
import Home from './views/Home';

export default function App() {
    return (
        <Router history={history} >
            <Switch>
                <Route path="/login" component={Login} />
                <PrivateRoute path="/" component={Home} />
            </Switch>
        </Router>
    )
};