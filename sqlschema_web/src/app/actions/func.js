import action from "../lib/createAction"

// All functions
export const GET_FUNC_LIST = "GET_FUNC_LIST"
export const GetFuncList = (sortWay) => action(GET_FUNC_LIST, { sortWay })

export const SET_FUNC_LIST = "SET_FUNC_LIST"
export const SetFuncList = (data) => action(SET_FUNC_LIST, { data })


// 指定 Function 使用哪些 Object
export const GET_FUNC_OBJ_USE_LIST = "GET_FUNC_OBJ_USE_LIST"
export const GetFuncObjUseList = (Func) => action(GET_FUNC_OBJ_USE_LIST, { Func })

export const SET_FUNC_OBJ_USE_LIST = "SET_FUNC_OBJ_USE_LIST"
export const SetFuncObjUseList = (data) => action(SET_FUNC_OBJ_USE_LIST, { data })


// 指定 Function 被哪些 Object 使用
export const GET_FUNC_OBJ_USED_LIST = "GET_FUNC_OBJ_USED_LIST"
export const GetFuncObjUsedList = (Func) => action(GET_FUNC_OBJ_USED_LIST, { Func })

export const SET_FUNC_OBJ_USED_LIST = "SET_FUNC_OBJ_USED_LIST"
export const SetFuncObjUsedList = (data) => action(SET_FUNC_OBJ_USED_LIST, { data })


// Script & Type
export const GET_FUNC_SCRIPT_LIST = "GET_FUNC_SCRIPT_LIST"
export const GetFuncScriptList = (Func) => action(GET_FUNC_SCRIPT_LIST, { Func })

export const SET_FUNC_SCRIPT_LIST = "SET_FUNC_SCRIPT_LIST"
export const SetFuncScriptList = (data) => action(SET_FUNC_SCRIPT_LIST, { data })