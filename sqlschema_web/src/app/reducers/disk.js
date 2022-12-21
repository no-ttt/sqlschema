import { GET_DISK_LIST, SET_DISK_LIST, GET_DATA_USAGE_LIST, SET_DATA_USAGE_LIST, GET_TABLE_USAGE_LIST, SET_TABLE_USAGE_LIST } from "../actions/disk"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function diskList(state = initState, action) {
	switch (action.type) {
		case GET_DISK_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_DISK_LIST:
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

export function dataUsageList(state = initState, action) {
	switch (action.type) {
		case GET_DATA_USAGE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_DATA_USAGE_LIST:
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

export function tableUsageList(state = initState, action) {
	switch (action.type) {
		case GET_TABLE_USAGE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_TABLE_USAGE_LIST:
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
