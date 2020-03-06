import React, { useEffect } from 'react';
import { IoLogoGithub, IoIosSearch, IoIosRocket } from 'react-icons/io';
import DashboardCard from '../../components/Common/DashboardCard';

export default function Main() {
    const Cards = [
        {
            backgroud: 'linear-gradient(to right, #6190E8 100%, #A7BFE8 80%, #6190E8 100%)',
            icon: <IoLogoGithub size={60} color="#fff" />,
            description: 'Source(Github)',
            url: 'https://github.com/swedendry/Server',
        },
        {
            backgroud: 'linear-gradient(to right, #614385 0%, #516395 100%, #614385 100%)',
            icon: <IoIosSearch size={60} color="#fff" />,
            description: 'Log(Kibana)',
            url: 'http://52.231.118.142:5601/',
        },
        {
            backgroud: 'linear-gradient(to right, #ff7e5f 0%, #feb47b 100%, #ff7e5f 100%)',
            icon: <IoIosRocket size={60} color="#fff" />,
            description: 'Api(Swagger)',
            url: 'http://52.231.118.149:80/swagger/index.html',
        },
    ];

    /**
     * Main
     */

    // Mount
    useEffect(() => {
        document.title = 'thegido - Home';
    }, []);

    // Unmount
    useEffect(() => {
        return () => {
            document.title = '';
        };
    }, []);

    return <DashboardCard cards={Cards} />
}
