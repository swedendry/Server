import React, { useEffect } from 'react';
//import { IoMdPeople, IoMdStats, IoMdCart, IoMdPulse } from 'react-icons/io';
import { IoLogoGithub, IoIosSearch, IoIosRocket } from 'react-icons/io';
import DashboardCard from '../../components/Common/DashboardCard';

export default function Main() {
    const Cards = [
        {
            backgroud: 'linear-gradient(to right, #6190E8 100%, #A7BFE8 80%, #6190E8 100%)',
            icon: <IoIosSearch size={60} color="#fff" />,
            description: 'Github',
            url: 'https://github.com/swedendry',
        },
        {
            backgroud: 'linear-gradient(to right, #6190E8 100%, #A7BFE8 80%, #6190E8 100%)',
            icon: <IoIosSearch size={60} color="#fff" />,
            description: 'Kibana',
            url: 'http://52.231.118.142:5601/',
        },
        {
            backgroud: 'linear-gradient(to right, #6190E8 100%, #A7BFE8 80%, #6190E8 100%)',
            icon: <IoIosRocket size={60} color="#fff" />,
            description: 'Swagger',
            url: 'http://52.231.118.149:80/swagger/index.html',
        },
        //{
        //  backgroud: 'linear-gradient(to right, #02aab0, #00cdac)',
        //  icon: <IoMdCart size={60} color="#fff"/>,
        //  description: 'Registered Products',
        //  result: 521,
        //},
        //{
        //  backgroud: 'linear-gradient(to right, #614385 0%, #516395 100%, #614385 100%)',
        //  icon: <IoMdStats size={60} color="#fff"/>,
        //  description: 'Sales this month',
        //  result: 'R$ 1.325'
        //},
        //{
        //  backgroud: 'linear-gradient(to right, #ff7e5f 0%, #feb47b 100%, #ff7e5f 100%)',
        //  icon: <IoMdPulse size={60} color="#fff"/>,
        //  description: 'Reports',
        //  result: '+ 72%',
        //},
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
