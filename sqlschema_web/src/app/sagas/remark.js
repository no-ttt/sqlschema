import { call, put } from "redux-saga/effects"
import * as actions from "../actions"
import api from "../lib/api"

/* 
    take:  等待一個 action 傳入
    call:  呼叫一個 function
    put :  呼叫一個 action
    select 從 store 取出資料

*/

export function* PostRemarkList(action) {
	let data = yield call(api, {
		cmd: `Extendedproperty/${action.level1type}`,
        method: 'POST',
        data: {
            "value": action.value,
            "tableName": action.tableName,
            "columnName": action.columnName
        }
	})
	yield put(actions.SetRemarkList(data.body))
}
