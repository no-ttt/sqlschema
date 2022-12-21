import action from "../lib/createAction"

export const GET_PARAM_LIST = "GET_PARAM_LIST"
export const GetParamList = (name, sortWay) => action(GET_PARAM_LIST, { name, sortWay })

export const SET_PARAM_LIST = "SET_PARAM_LIST"
export const SetParamList = (data) => action(SET_PARAM_LIST, { data })


