import { takeEvery, takeLatest, all } from "redux-saga/effects"

import * as actions from "../actions"
import * as count from "./count"
import * as user from "./user"
import * as table from "./table"
import * as view from "./view"
import * as func from "./func"
import * as param from "./param"
import * as proc from "./proc"
import * as remark from "./remark"
import * as disk from "./disk"
import * as distribution from "./distribution"
import * as assessment from "./assessment"

export default function* () {
	yield takeLatest(actions.GET_COUNT_LIST, count.GetCountList)
	yield takeLatest(actions.GET_USER_LIST, user.GetUserList)
	yield takeLatest(actions.GET_TABLE_PRIVILEGE_LIST, user.GetTablePrivilegeList)
	yield takeLatest(actions.GET_TABLE_LIST, table.GetTableList)
	yield takeLatest(actions.GET_COLUMN_LIST, table.GetColumnList)
	yield takeLatest(actions.GET_REL_LIST, table.GetRelList)
	yield takeLatest(actions.GET_UNIQUE_LIST, table.GetUniqueList)
	yield takeLatest(actions.GET_INDEX_LIST, table.GetIndexList)
	yield takeLatest(actions.GET_USES_LIST, table.GetUsesList)
	yield takeLatest(actions.GET_USED_LIST, table.GetUsedList)
	yield takeLatest(actions.GET_VIEW_LIST, view.GetViewList)
	yield takeLatest(actions.GET_VIEW_COLUMN_LIST, view.GetViewColumnList)
	yield takeLatest(actions.GET_USE_LIST, view.GetUseList)
	yield takeLatest(actions.GET_SCRIPT_LIST, view.GetScriptList)
	yield takeLatest(actions.GET_FUNC_LIST, func.GetFuncList)
	yield takeLatest(actions.GET_PARAM_LIST, param.GetParamList)
	yield takeLatest(actions.GET_FUNC_OBJ_USE_LIST, func.GetFuncObjUseList)
	yield takeLatest(actions.GET_FUNC_OBJ_USED_LIST, func.GetFuncObjUsedList)
	yield takeLatest(actions.GET_FUNC_SCRIPT_LIST, func.GetFuncScriptList)
	yield takeLatest(actions.GET_PROC_LIST, proc.GetProcList)
	yield takeLatest(actions.GET_PROC_OBJ_USE_LIST, proc.GetProcObjUseList)
	yield takeLatest(actions.GET_PROC_OBJ_USED_LIST, proc.GetProcObjUsedList)
	yield takeLatest(actions.GET_PROC_SCRIPT_LIST, proc.GetProcScriptList)
	yield takeLatest(actions.POST_REMARK_LIST, remark.PostRemarkList)
	yield takeLatest(actions.GET_DISK_LIST, disk.GetDiskList)
	yield takeLatest(actions.GET_DATA_USAGE_LIST, disk.GetDataUsageList)
	yield takeLatest(actions.GET_TABLE_USAGE_LIST, disk.GetTableUsageList)
	yield takeLatest(actions.GET_VALUE_LIST, distribution.GetValueList)
	yield takeLatest(actions.GET_TYPE_LIST, distribution.GetTypeList)
	yield takeLatest(actions.GET_CHECK_LIST, assessment.GetCheckList)
}
