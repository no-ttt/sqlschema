import 'core-js/stable'
import 'regenerator-runtime/runtime'
import React from 'react'
import { render } from 'react-dom'
import { Provider } from 'react-redux'
import configureStore from './store'
import App from './containers/App'
import { BrowserRouter as Router,Route } from 'react-router-dom'
import rootSaga from './sagas'

import './styles/HomePage'
import './styles/Drawer'
import './styles/Tem'
import './styles/Table'
import './styles/Disk'
import './styles/Assessment'
import './styles/Tabs'

const store = configureStore()
store.runSaga(rootSaga)
render(
    <Provider store={store}>
        <Router>
            <Route path="/" component={App} />
        </Router>
    </Provider>
    , document.getElementById('root'));
