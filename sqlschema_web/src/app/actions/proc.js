import action from "../lib/createAction"

// All Procedures
export const GET_PROC_LIST = "GET_PROC_LIST"
export const GetProcList = (sortWay) => action(GET_PROC_LIST, { sortWay })

export const SET_PROC_LIST = "SET_PROC_LIST"
export const SetProcList = (data) => action(SET_PROC_LIST, { data })


// 指定 Procedure 使用哪些 Object
export const GET_PROC_OBJ_USE_LIST = "GET_PROC_OBJ_USE_LIST"
export const GetProcObjUseList = (Proc) => action(GET_PROC_OBJ_USE_LIST, { Proc })

export const SET_PROC_OBJ_USE_LIST = "SET_PROC_OBJ_USE_LIST"
export const SetProcObjUseList = (data) => action(SET_PROC_OBJ_USE_LIST, { data })


// 指定 Procedure 被哪些 Object 使用
export const GET_PROC_OBJ_USED_LIST = "GET_PROC_OBJ_USED_LIST"
export const GetProcObjUsedList = (Proc) => action(GET_PROC_OBJ_USED_LIST, { Proc })

export const SET_PROC_OBJ_USED_LIST = "SET_PROC_OBJ_USED_LIST"
export const SetProcObjUsedList = (data) => action(SET_PROC_OBJ_USED_LIST, { data })


// Script & Type
export const GET_PROC_SCRIPT_LIST = "GET_PROC_SCRIPT_LIST"
export const GetProcScriptList = (Proc) => action(GET_PROC_SCRIPT_LIST, { Proc })

export const SET_PROC_SCRIPT_LIST = "SET_PROC_SCRIPT_LIST"
export const SetProcScriptList = (data) => action(SET_PROC_SCRIPT_LIST, { data })
