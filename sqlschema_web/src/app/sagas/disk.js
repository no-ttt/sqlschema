import { call, put } from "redux-saga/effects"
import * as actions from "../actions"
import api from "../lib/api"

/* 
    take:  等待一個 action 傳入
    call:  呼叫一個 function
    put :  呼叫一個 action
    select 從 store 取出資料

*/

export function* GetDiskList(action) {
	let data = yield call(api, {
		cmd: `Disk`,
	})
	yield put(actions.SetDiskList(data.body))
}

export function* GetDataUsageList(action) {
	let data = yield call(api, {
		cmd: `Disk/DataSpace`,
	})
	yield put(actions.SetDataUsageList(data.body))
}

export function* GetTableUsageList(action) {
	let data = yield call(api, {
		cmd: action.tab !== undefined ? `Disk/TableSpace?Tab=${action.tab}` : `Disk/TableSpace`,
	})
	yield put(actions.SetTableUsageList(data.body))
}