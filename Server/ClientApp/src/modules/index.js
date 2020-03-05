import { combineReducers } from 'redux';
import dashboard from './dashboard';
import auth from './auth';
import users from './users';
import hubs from './hubs';
import leaderboards from './leaderboards';
import leaderboardMembers from './leaderboardMembers';

const rootReducer = combineReducers({
    dashboard,
    auth,
    users,
    hubs,
    leaderboards,
    leaderboardMembers
});

export default rootReducer;