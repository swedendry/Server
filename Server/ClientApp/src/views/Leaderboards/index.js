import React from 'react';
import styled from 'styled-components';
import Leaderboards from '../../components/Games/Leaderboards'

// Styles
const Title = styled.h1`
  color: #656565;
  font-size: 1.2rem;
`;

export default function Main() {
    return (
        <>
            <Title>Leaderboards</Title>
            <Leaderboards />
        </>
    );
}