import React, { useState, useEffect } from 'react';
import { Form, FormGroup, } from 'reactstrap';
import { Button, InputAdornment } from '@material-ui/core';
import { IoMdPerson, IoMdKey } from 'react-icons/io';
import { TextField as Input } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import styled from 'styled-components';

import { fieldValidator } from '../helpers/fieldValidator';
import history from '../helpers/history';
import Alert from '../components/Common/Alert';

import useAuth from '../hooks/useAuth';

// Styles
const Wrapper = styled.div`
  font-family: Arial, Helvetica, sans-serif;
  height: 80vh;
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
`;

const Box = styled.div`
  height: 350px;
  width: 400px;
  background: #ffffff;
  border-radius: 5px;
  padding: 20px;
`;

const Brand = styled.h1`
  font-family: 'Pacifico', cursive;
  font-size: 2.5rem;
  color: #57606f;
  text-align: center;
  margin-bottom: 10px;
`;

export const stylePrimary = {
    background: '#2980b9',
    color: '#fff',
    border: 'none',
    borderRadius: '4px',
    outline: 'none',
    width: '100%',
    height: '40px',
    margin: '0',
    textTransform: 'capitalize',
    backgroundImage: 'linear-gradient(to right, #6190E8 100%, #A7BFE8 100%, #6190E8 100%)',
};

const useStyles = makeStyles(theme => ({
    margin: {
        margin: theme.spacing(1),
    }
}));

export default function Main() {
    const classes = useStyles();

    const [user, setUser] = useState({ email: '', password: '' });
    const [error, setError] = useState({ field: '' });
    const [alert, setAlert] = useState({ type: '', status: false, message: '' });

    const { view, onLogin } = useAuth();

    useEffect(() => {
        document.title = 'Login';

        if (view.isAuthenticated) {
            history.push('/');
        }
    }, [view.isAuthenticated, history]);

    const validateFields = () => {
        let result = fieldValidator(user);
        return result.status ? true : setError({ field: result.field });
    };

    const handleLogin = e => {
        e.preventDefault();

        if (user.email === '') {    //guest
            user.email = 'guest';
            user.password = 'guest';
        }

        if (validateFields()) {
            onLogin(user.email, user.password);
        }
        else {
            setAlert({
                type: 'warning',
                status: true,
                message: 'Invalid Credentials.',
            });
        }
    };

    return (
        <Wrapper>
            <Box>
                <Form onSubmit={e => handleLogin(e)} noValidate autoComplete="off">
                    <Brand> thegido </Brand>
                    <Alert type={alert.type} status={alert.status} message={alert.message} />
                    <FormGroup>
                        <Input
                            label="Email:"
                            type="email"
                            name="email"
                            margin="normal"
                            fullWidth={true}
                            variant="outlined"
                            placeholder="email"
                            onChange={e => setUser({ ...user, email: e.target.value })}
                            error={error.field === 'email' && !user.email ? true : false}
                            InputProps={{
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <IoMdPerson size={22} style={{ color: '#57606f' }} />
                                    </InputAdornment>
                                ),
                            }}
                        />
                    </FormGroup>
                    <FormGroup>
                        <Input
                            label="Password:"
                            type="password"
                            name="password"
                            placeholder="********"
                            margin="normal"
                            fullWidth={true}
                            variant="outlined"
                            onChange={e => setUser({ ...user, password: e.target.value })}
                            error={error.field === 'password' && !user.password ? true : false}
                            InputProps={{
                                startAdornment: (
                                    <InputAdornment position="start">
                                        <IoMdKey size={22} style={{ color: '#57606f' }} />
                                    </InputAdornment>
                                ),
                            }}
                        />
                    </FormGroup>
                    <Button
                        variant="contained"
                        size="medium"
                        style={stylePrimary}
                        className={classes.margin}
                        type="submit"
                    >
                        Login
                    </Button>
                    <Button
                        variant="contained"
                        size="medium"
                        style={stylePrimary}
                        className={classes.margin}
                        type="submit"
                    >
                        Guest
                    </Button>
                </Form>
            </Box>
        </Wrapper>
    )
}