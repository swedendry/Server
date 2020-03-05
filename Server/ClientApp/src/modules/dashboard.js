const TOGGLE_SIDEBAR = 'dashboard/TOGGLE_SIDEBAR';

const initialState = {
    sidebar: 'desktop',
    navbar: 'desktop',
    content: 'desktop',
};

export const toggle = (dashboard) => ({ type: TOGGLE_SIDEBAR, payload: dashboard });

const dashboard = (state = initialState, action) => {
    switch (action.type) {
        case TOGGLE_SIDEBAR:
            if (action.payload.sidebar === 'desktop' && window.innerWidth < 576) {
                return ({
                    ...state,
                    sidebar: 'mobile',
                    navbar: 'mobile',
                });
            } if (action.payload.sidebar === 'desktop' && window.innerWidth > 576) {
                return ({
                    ...state,
                    sidebar: 'min',
                    navbar: 'min',
                    content: 'min',
                });
            }
            return ({
                ...state,
                sidebar: 'desktop',
                navbar: 'desktop',
                content: 'desktop',
            });
        default:
            return state;
    }
};

export default dashboard;