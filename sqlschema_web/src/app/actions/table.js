import action from "../lib/createAction"

// All Tables
export const GET_TABLE_LIST = "GET_TABLE_LIST"
export const GetTableList = (sortWay) => action(GET_TABLE_LIST, { sortWay })

export const SET_TABLE_LIST = "SET_TABLE_LIST"
export const SetTableList = (data) => action(SET_TABLE_LIST, { data })


// 指定 Table 的欄位
export const GET_COLUMN_LIST = "GET_COLUMN_LIST"
export const GetColumnList = (Tab, sortWay) => action(GET_COLUMN_LIST, { Tab, sortWay })

export const SET_COLUMN_LIST = "SET_COLUMN_LIST"
export const SetColumnList = (data) => action(SET_COLUMN_LIST, { data })


// 指定 Table 的 Relation
export const GET_REL_LIST = "GET_REL_LIST"
export const GetRelList = (Tab) => action(GET_REL_LIST, { Tab })

export const SET_REL_LIST = "SET_REL_LIST"
export const SetRelList = (data) => action(SET_REL_LIST, { data })


// 指定 Table 的 Unique Key
export const GET_UNIQUE_LIST = "GET_UNIQUE_LIST"
export const GetUniqueList = (Tab) => action(GET_UNIQUE_LIST, { Tab })

export const SET_UNIQUE_LIST = "SET_UNIQUE_LIST"
export const SetUniqueList = (data) => action(SET_UNIQUE_LIST, { data })


// 指定 Table 的 Index
export const GET_INDEX_LIST = "GET_INDEX_LIST"
export const GetIndexList = (Tab) => action(GET_INDEX_LIST, { Tab })

export const SET_INDEX_LIST = "SET_INDEX_LIST"
export const SetIndexList = (data) => action(SET_INDEX_LIST, { data })


// 指定 Table 使用哪些 Table
export const GET_USES_LIST = "GET_USES_LIST"
export const GetUsesList = (Tab) => action(GET_USES_LIST, { Tab })

export const SET_USES_LIST = "SET_USES_LIST"
export const SetUsesList = (data) => action(SET_USES_LIST, { data })


// 指定 Table 被哪些 Table 使用
export const GET_USED_LIST = "GET_USED_LIST"
export const GetUsedList = (Tab) => action(GET_USED_LIST, { Tab })

export const SET_USED_LIST = "SET_USED_LIST"
export const SetUsedList = (data) => action(SET_USED_LIST, { data })
