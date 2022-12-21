import { call, put } from "redux-saga/effects"
import * as actions from "../actions"
import api from "../lib/api"

/* 
    take:  等待一個 action 傳入
    call:  呼叫一個 function
    put :  呼叫一個 action
    select 從 store 取出資料

*/

export function* GetTableList(action) {
	let data = yield call(api, {
		cmd: `Table?sortWay=${action.sortWay}`,
	})
	yield put(actions.SetTableList(data.body))
}

export function* GetColumnList(action) {
	let data = yield call(api, {
		cmd: `Table/Column?Tab=${action.Tab}&sortWay=${action.sortWay}`,
	})
	yield put(actions.SetColumnList(data.body))
}

export function* GetRelList(action) {
	let data = yield call(api, {
		cmd: `Table/Relation?Tab=${action.Tab}`,
	})
	yield put(actions.SetRelList(data.body))
}

export function* GetUniqueList(action) {
	let data = yield call(api, {
		cmd: `Table/Unique?Tab=${action.Tab}`,
	})
	yield put(actions.SetUniqueList(data.body))
}

export function* GetIndexList(action) {
	let data = yield call(api, {
		cmd: `Table/Index?Tab=${action.Tab}`,
	})
	yield put(actions.SetIndexList(data.body))
}

export function* GetUsesList(action) {
	let data = yield call(api, {
		cmd: `Table/Uses?Tab=${action.Tab}`,
	})
	yield put(actions.SetUsesList(data.body))
}

export function* GetUsedList(action) {
	let data = yield call(api, {
		cmd: `Table/Used?Tab=${action.Tab}`,
	})
	yield put(actions.SetUsedList(data.body))
}