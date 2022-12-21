import { call, put } from "redux-saga/effects"
import * as actions from "../actions"
import api from "../lib/api"

/* 
    take:  等待一個 action 傳入
    call:  呼叫一個 function
    put :  呼叫一個 action
    select 從 store 取出資料

*/

export function* GetViewList(action) {
	let data = yield call(api, {
		cmd: `View?sortWay=${action.sortWay}`,
	})
	yield put(actions.SetViewList(data.body))
}

export function* GetViewColumnList(action) {
	let data = yield call(api, {
		cmd: `View/Column?Tab=${action.Tab}&sortWay=${action.sortWay}`,
	})
	yield put(actions.SetViewColumnList(data.body))
}

export function* GetUseList(action) {
	let data = yield call(api, {
		cmd: `View/Uses?Tab=${action.Tab}`,
	})
	yield put(actions.SetUseList(data.body))
}

export function* GetScriptList(action) {
	let data = yield call(api, {
		cmd: `View/Script?Tab=${action.Tab}`,
	})
	yield put(actions.SetScriptList(data.body))
}
