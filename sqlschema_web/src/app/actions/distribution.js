import action from "../lib/createAction"

// column value distribution
export const GET_VALUE_LIST = "GET_VALUE_LIST"
export const GetValueList = (Tab) => action(GET_VALUE_LIST, { Tab })

export const SET_VALUE_LIST = "SET_VALUE_LIST"
export const SetValueList = (data) => action(SET_VALUE_LIST, { data })


// data type distribution
export const GET_TYPE_LIST = "GET_TYPE_LIST"
export const GetTypeList = () => action(GET_TYPE_LIST, {})

export const SET_TYPE_LIST = "SET_TYPE_LIST"
export const SetTypeList = (data) => action(SET_TYPE_LIST, { data })