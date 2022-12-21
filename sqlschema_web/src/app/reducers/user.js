import { GET_USER_LIST, SET_USER_LIST, GET_TABLE_PRIVILEGE_LIST, SET_TABLE_PRIVILEGE_LIST, GET_COLUMN_PRIVILEGE_LIST, SET_COLUMN_PRIVILEGE_LIST } from "../actions/user"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function userList(state = initState, action) {
	switch (action.type) {
		case GET_USER_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_USER_LIST:
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

export function tablePrivilegeList(state = initState, action) {
	switch (action.type) {
		case GET_TABLE_PRIVILEGE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_TABLE_PRIVILEGE_LIST:
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

export function columnPrivilegeList(state = initState, action) {
	switch (action.type) {
		case GET_COLUMN_PRIVILEGE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_COLUMN_PRIVILEGE_LIST:
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