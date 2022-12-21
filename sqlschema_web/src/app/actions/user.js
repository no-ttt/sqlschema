import action from "../lib/createAction"

export const GET_USER_LIST = "GET_USER_LIST"
export const GetUserList = () => action(GET_USER_LIST, {})

export const SET_USER_LIST = "SET_USER_LIST"
export const SetUserList = (data) => action(SET_USER_LIST, { data })


export const GET_TABLE_PRIVILEGE_LIST = "GET_TABLE_PRIVILEGE_LIST"
export const GetTablePrivilegeList = () => action(GET_TABLE_PRIVILEGE_LIST, {})

export const SET_TABLE_PRIVILEGE_LIST = "SET_TABLE_PRIVILEGE_LIST"
export const SetTablePrivilegeList = (data) => action(SET_TABLE_PRIVILEGE_LIST, { data })


export const GET_COLUMN_PRIVILEGE_LIST = "GET_COLUMN_PRIVILEGE_LIST"
export const GetColumnPrivilegeList = () => action(GET_COLUMN_PRIVILEGE_LIST, {})

export const SET_COLUMN_PRIVILEGE_LIST = "SET_COLUMN_PRIVILEGE_LIST"
export const SetColumnPrivilegeList = (data) => action(SET_COLUMN_PRIVILEGE_LIST, { data })
