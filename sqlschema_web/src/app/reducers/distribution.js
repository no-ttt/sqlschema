import { GET_VALUE_LIST, SET_VALUE_LIST, GET_TYPE_LIST, SET_TYPE_LIST } from "../actions/distribution"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function valueList(state = initState, action) {
	switch (action.type) {
		case GET_VALUE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_VALUE_LIST:
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

export function typeList(state = initState, action) {
	switch (action.type) {
		case GET_TYPE_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_TYPE_LIST:
			return {
				...state,
				fetching: true,
				items: action.data.data,
				count: action.data.count[0].tableCount,
				error: "",
			}
		default:
			return state
	}
}