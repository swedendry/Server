import React from 'react';

import routes from '../routes';

import { Sidebar, Navbar, Content } from '../components/Layout';

import useAuth from '../hooks/useAuth'; 

export default function Main() {
    const { view } = useAuth();

    routes.navbar.dropdown.user.name = view.user.id;

    return (
        <>
            <Navbar
                dropdown={routes.navbar.dropdown}
            />
            <Sidebar
                brand={routes.sidebar.brand}
                buttons={routes.sidebar.buttons}
            />
            <Content
                routes={routes.content}
            />
        </>
    )
}
