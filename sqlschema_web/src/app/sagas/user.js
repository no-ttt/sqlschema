import { call, put } from "redux-saga/effects"
import * as actions from "../actions"
import api from "../lib/api"

/* 
    take:  等待一個 action 傳入
    call:  呼叫一個 function
    put :  呼叫一個 action
    select 從 store 取出資料

*/

export function* GetUserList(action) {
	let data = yield call(api, {
		cmd: `User`,
	})
	yield put(actions.SetUserList(data.body))
}

export function* GetTablePrivilegeList(action) {
	let data = yield call(api, {
		cmd: `User/Table`,
	})
	yield put(actions.SetTablePrivilegeList(data.body))
}

export function* GetColumnPrivilegeList(action) {
	let data = yield call(api, {
		cmd: `User/Column`,
	})
	yield put(actions.SetColumnPrivilegeList(data.body))
}
