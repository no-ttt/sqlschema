import action from "../lib/createAction"

export const GET_COUNT_LIST = "GET_COUNT_LIST"
export const GetCountList = () => action(GET_COUNT_LIST, {})

export const SET_COUNT_LIST = "SET_COUNT_LIST"
export const SetCountList = (data) => action(SET_COUNT_LIST, { data })


