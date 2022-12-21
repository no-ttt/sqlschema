import { combineReducers } from 'redux'
import { routerReducer } from 'react-router-redux'
import { importAll } from '../lib/action'

const cnt = require.context(__dirname, true, /^((?!index).)*\.js$/)
const reducersNames = importAll(cnt, true)

const rootReducer = combineReducers({
  routing: routerReducer,
  ...reducersNames
})

export default rootReducer
