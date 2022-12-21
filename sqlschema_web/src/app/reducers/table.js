import { GET_TABLE_LIST, SET_TABLE_LIST, GET_COLUMN_LIST, SET_COLUMN_LIST, GET_REL_LIST, SET_REL_LIST, 
			GET_UNIQUE_LIST, SET_UNIQUE_LIST, GET_USES_LIST, SET_USES_LIST, GET_USED_LIST, SET_USED_LIST,
			GET_INDEX_LIST, SET_INDEX_LIST } from "../actions/table"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function tableList(state = initState, action) {
	switch (action.type) {
		case GET_TABLE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_TABLE_LIST:
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

export function columnList(state = initState, action) {
	switch (action.type) {
		case GET_COLUMN_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_COLUMN_LIST:
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

export function relList(state = initState, action) {
	switch (action.type) {
		case GET_REL_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_REL_LIST:
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

export function uniqueList(state = initState, action) {
	switch (action.type) {
		case GET_UNIQUE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_UNIQUE_LIST:
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

export function indexList(state = initState, action) {
	switch (action.type) {
		case GET_INDEX_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_INDEX_LIST:
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

export function usesList(state = initState, action) {
	switch (action.type) {
		case GET_USES_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_USES_LIST:
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

export function usedList(state = initState, action) {
	switch (action.type) {
		case GET_USED_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_USED_LIST:
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