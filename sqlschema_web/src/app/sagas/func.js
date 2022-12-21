import { call, put } from "redux-saga/effects"
import * as actions from "../actions"
import api from "../lib/api"

/* 
    take:  等待一個 action 傳入
    call:  呼叫一個 function
    put :  呼叫一個 action
    select 從 store 取出資料

*/

export function* GetFuncList(action) {
	let data = yield call(api, {
		cmd: `Function?sortWay=${action.sortWay}`,
	})
	yield put(actions.SetFuncList(data.body))
}

export function* GetFuncObjUseList(action) {
	let data = yield call(api, {
		cmd: `Function/Uses?func=${action.Func}`,
	})
	yield put(actions.SetFuncObjUseList(data.body))
}

export function* GetFuncObjUsedList(action) {
	let data = yield call(api, {
		cmd: `Function/Used?func=${action.Func}`,
	})
	yield put(actions.SetFuncObjUsedList(data.body))
}

export function* GetFuncScriptList(action) {
	let data = yield call(api, {
		cmd: `Function/Info?func=${action.Func}`,
	})
	yield put(actions.SetFuncScriptList(data.body))
}