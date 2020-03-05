import React, { useState, useEffect } from 'react';
import { Button, Jumbotron, Input, Form, FormGroup } from 'reactstrap';

import useAuth from '../../hooks/useAuth';
import useHubs from '../../hooks/useHubs';
import useLeaderboardMembers from '../../hooks/useLeaderboardMembers';

export default function Main({ url }) {
    const [message, setMessage] = useState('');

    const { view : userView } = useAuth();
    const { view, onConnect, onStop, onSend } = useHubs();
    const { onUpdate : onUpdate_leaderboard } = useLeaderboardMembers();

    useEffect(() => {
        if (!view.isConnected) {
            onConnect(url, userView.user.id);
        }
    }, []);

    useEffect(() => {
        return () => {
            if (view.isConnected) {
                onStop(view.hubConnection);
            }
        }
    }, []);

    const handleSend = e => {
        e.preventDefault();

        onSend(view.hubConnection, message);
        onUpdate_leaderboard('leaderboard:chatcount', view.id, 1);
        onUpdate_leaderboard('leaderboard:chatlength', view.id, message.length);

        setMessage('');
    };

    return (
        <>
            <Form onSubmit={e => handleSend(e)}>
                <FormGroup>
                    <Button color="primary" type="submit">Send</Button>
                    <Input type="text" name="message" value={message} id="exampleMessage" onChange={e => setMessage(e.target.value)} />
                </FormGroup>
            </Form>

            <Jumbotron>
                {view.messages.map((message, index) => (
                    <span style={{ display: 'block' }} key={index}> {message} </span>
                ))}
            </Jumbotron>
        </>
    );
}