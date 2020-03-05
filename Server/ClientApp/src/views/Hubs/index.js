import React from 'react';
import styled from 'styled-components';

import Chat from '../../components/Hubs/Chat';

// Styles
const Title = styled.h1`
  color: #656565;
  font-size: 1.2rem;
`;

export default function Main() {
    return (
        <>
            <Title>Chat</Title>
            <Chat url={`/chat`} />
        </>
    );
}

//<Chat url={`server:80/chat`} />
//<Chat url={`https://localhost:44360/chat`} />
//<Chat url={`http://40.89.248.76:80/chat`} />