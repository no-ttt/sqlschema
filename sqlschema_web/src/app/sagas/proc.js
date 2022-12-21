import { call, put } from "redux-saga/effects"
import * as actions from "../actions"
import api from "../lib/api"

/* 
    take:  等待一個 action 傳入
    call:  呼叫一個 function
    put :  呼叫一個 action
    select 從 store 取出資料

*/

export function* GetProcList(action) {
	let data = yield call(api, {
		cmd: `Procedure?sortWay=${action.sortWay}`,
	})
	yield put(actions.SetProcList(data.body))
}

export function* GetProcObjUseList(action) {
	let data = yield call(api, {
		cmd: `Procedure/Uses?proc=${action.Proc}`,
	})
	yield put(actions.SetProcObjUseList(data.body))
}

export function* GetProcObjUsedList(action) {
	let data = yield call(api, {
		cmd: `Procedure/Used?proc=${action.Proc}`,
	})
	yield put(actions.SetProcObjUsedList(data.body))
}

export function* GetProcScriptList(action) {
	let data = yield call(api, {
		cmd: `Procedure/Info?proc=${action.Proc}`,
	})
	yield put(actions.SetProcScriptList(data.body))
}