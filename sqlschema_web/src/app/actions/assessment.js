import action from "../lib/createAction"

export const GET_CHECK_LIST = "GET_CHECK_LIST"
export const GetCheckList = () => action(GET_CHECK_LIST, {})

export const SET_CHECK_LIST = "SET_CHECK_LIST"
export const SetCheckList = (data) => action(SET_CHECK_LIST, { data })
