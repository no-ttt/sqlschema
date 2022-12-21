import { GET_FUNC_LIST, SET_FUNC_LIST, GET_FUNC_OBJ_USE_LIST, SET_FUNC_OBJ_USE_LIST, GET_FUNC_OBJ_USED_LIST, SET_FUNC_OBJ_USED_LIST, 
			GET_FUNC_SCRIPT_LIST, SET_FUNC_SCRIPT_LIST } from "../actions/func"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function funcList(state = initState, action) {
	switch (action.type) {
		case GET_FUNC_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_FUNC_LIST:
			return {
				...state,
				fetching: true,
				items: action.data.data,
				error: "",
			}
		default:
			return state
	}
}

export function funcObjUseList(state = initState, action) {
	switch (action.type) {
		case GET_FUNC_OBJ_USE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_FUNC_OBJ_USE_LIST:
			return {
				...state,
				fetching: true,
				items: action.data.data,
				error: "",
			}
		default:
			return state
	}
}

export function funcObjUsedList(state = initState, action) {
	switch (action.type) {
		case GET_FUNC_OBJ_USED_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_FUNC_OBJ_USED_LIST:
			return {
				...state,
				fetching: true,
				items: action.data.data,
				error: "",
			}
		default:
			return state
	}
}

export function funcScriptList(state = initState, action) {
	switch (action.type) {
		case GET_FUNC_SCRIPT_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_FUNC_SCRIPT_LIST:
			return {
				...state,
				fetching: true,
				items: action.data.data,
				error: "",
			}
		default:
			return state
	}
}