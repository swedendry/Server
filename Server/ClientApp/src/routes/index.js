import React from 'react';

import { delToken } from '../services/auth';

import Dashboard from '../views/Dashboard';
import Users from '../views/Users';
import Hubs from '../views/Hubs';
import Leaderboards from '../views/Leaderboards';
import LeaderboardMembers from '../views/Leaderboards/Content/LeaderboardMembers';

// Icons
import {
    IoMdOptions,
    IoMdPeople,
    IoLogoGameControllerB,
    IoMdRibbon
} from 'react-icons/io'

// Routes
const Routes = {
    dashboard: '/',
    users: '/users',
    hubs: '/hubs',
    leaderboards: '/leaderboards',
    leaderboardMembers: '/leaderboards/:leaderboardId',
};

const routes = {
    navbar: {
        dropdown: {
            user: {
                avatar: 'https://i.imgur.com/NpICPSl.jpg',
                name: 'thegido',
                jobRole: 'Administrator',
            },

            buttons: {
                settings: {
                    name: 'Settings',
                    event: () => { }
                },
                profile: {
                    name: 'Profile',
                    event: () => { }
                },
                logout: {
                    name: 'Logout',
                    event: () => {
                        delToken();
                        document.location.reload();
                    }
                }
            }
        }
    },

    sidebar: {
        brand: {
            max: 'thegido',
            min: 'gd'
        },

        buttons: [
            {
                name: 'Dashboard',
                icon: <IoMdOptions />,
                route: Routes.dashboard,
            },
            {
                name: 'Users',
                icon: <IoMdPeople />,
                route: Routes.users,
            },
            {
                name: 'Chat',
                icon: <IoLogoGameControllerB />,
                route: Routes.hubs,
            },
            {
                name: 'Leaderboards',
                icon: <IoMdRibbon />,
                route: Routes.leaderboards,
            },
        ]
    },

    content: [
        {
            route: Routes.dashboard,
            page: Dashboard
        },
        {
            route: Routes.users,
            page: Users
        },
        {
            route: Routes.hubs,
            page: Hubs
        },
        {
            route: Routes.leaderboards,
            page: Leaderboards
        },
        {
            route: Routes.leaderboardMembers,
            page: LeaderboardMembers
        },
    ]
};

export default routes;
