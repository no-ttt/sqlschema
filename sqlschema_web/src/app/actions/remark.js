import action from "../lib/createAction"

export const POST_REMARK_LIST = "POST_REMARK_LIST"
export const PostRemarkList = (level1type, value, tableName, columnName) => action(POST_REMARK_LIST, { level1type, value, tableName, columnName })

export const SET_REMARK_LIST = "SET_REMARK_LIST"
export const SetRemarkList = (data) => action(SET_REMARK_LIST, { data })

