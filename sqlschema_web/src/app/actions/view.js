import action from "../lib/createAction"

// All View Tables
export const GET_VIEW_LIST = "GET_VIEW_LIST"
export const GetViewList = (sortWay) => action(GET_VIEW_LIST, { sortWay })

export const SET_VIEW_LIST = "SET_VIEW_LIST"
export const SetViewList = (data) => action(SET_VIEW_LIST, { data })


// 指定 View Table 的欄位
export const GET_VIEW_COLUMN_LIST = "GET_VIEW_COLUMN_LIST"
export const GetViewColumnList = (Tab, sortWay) => action(GET_VIEW_COLUMN_LIST, { Tab, sortWay })

export const SET_VIEW_COLUMN_LIST = "SET_VIEW_COLUMN_LIST"
export const SetViewColumnList = (data) => action(SET_VIEW_COLUMN_LIST, { data })


// 引用哪些 Table
export const GET_USE_LIST = "GET_USE_LIST"
export const GetUseList = (Tab) => action(GET_USE_LIST, { Tab })

export const SET_USE_LIST = "SET_USE_LIST"
export const SetUseList = (data) => action(SET_USE_LIST, { data })


// Script
export const GET_SCRIPT_LIST = "GET_SCRIPT_LIST"
export const GetScriptList = (Tab) => action(GET_SCRIPT_LIST, { Tab })

export const SET_SCRIPT_LIST = "SET_SCRIPT_LIST"
export const SetScriptList = (data) => action(SET_SCRIPT_LIST, { data })
