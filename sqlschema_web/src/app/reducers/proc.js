import { GET_PROC_LIST, SET_PROC_LIST, GET_PROC_OBJ_USE_LIST, SET_PROC_OBJ_USE_LIST, GET_PROC_OBJ_USED_LIST, SET_PROC_OBJ_USED_LIST,
			GET_PROC_SCRIPT_LIST, SET_PROC_SCRIPT_LIST } from "../actions/proc"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function procList(state = initState, action) {
	switch (action.type) {
		case GET_PROC_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_PROC_LIST:
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

export function procObjUseList(state = initState, action) {
	switch (action.type) {
		case GET_PROC_OBJ_USE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_PROC_OBJ_USE_LIST:
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

export function procObjUsedList(state = initState, action) {
	switch (action.type) {
		case GET_PROC_OBJ_USED_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_PROC_OBJ_USED_LIST:
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

export function procScriptList(state = initState, action) {
	switch (action.type) {
		case GET_PROC_SCRIPT_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_PROC_SCRIPT_LIST:
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