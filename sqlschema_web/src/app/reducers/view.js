import { GET_VIEW_LIST, SET_VIEW_LIST, GET_VIEW_COLUMN_LIST, SET_VIEW_COLUMN_LIST, GET_USE_LIST, SET_USE_LIST,
			GET_SCRIPT_LIST, SET_SCRIPT_LIST } from "../actions/view"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function viewList(state = initState, action) {
	switch (action.type) {
		case GET_VIEW_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_VIEW_LIST:
			return {
				...state,
				fetching: true,
				items: action.data.data,
				tab: action.data.tab,
				error: "",
			}
		default:
			return state
	}
}

export function viewColumnList(state = initState, action) {
	switch (action.type) {
		case GET_VIEW_COLUMN_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_VIEW_COLUMN_LIST:
			return {
				...state,
				fetching: true,
				items: action.data.data,
				tab: action.data.tab,
				error: "",
			}
		default:
			return state
	}
}

export function useList(state = initState, action) {
	switch (action.type) {
		case GET_USE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_USE_LIST:
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

export function scriptList(state = initState, action) {
	switch (action.type) {
		case GET_SCRIPT_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_SCRIPT_LIST:
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