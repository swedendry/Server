import React from 'react';
import styled from 'styled-components';
import BaseTab from '../../../components/Common/BaseTab';

import LeaderboardMembers from '../../../components/Games/LeaderboardMembers';

// Styles
const Title = styled.h1`
  color: #656565;
  font-size: 1.2rem;
`;

const Description = styled.h1`
  color: #656565;
  font-size: 1rem;
`;

export default function Main(props) {
    const leaderboardId = props.match.params.leaderboardId;

    return (
        <div>
            <Title>{leaderboardId}</Title>
            <BaseTab tabs={[
                {
                    name: 'LeaderboardMembers',
                    view: <LeaderboardMembers leaderboardId={leaderboardId} />,
                },
            ]} />
        </div>
    );
}