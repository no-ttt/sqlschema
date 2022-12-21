import { GET_PARAM_LIST, SET_PARAM_LIST } from "../actions/param"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function paramList(state = initState, action) {
	switch (action.type) {
		case GET_PARAM_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_PARAM_LIST:
			return {
				...state,
				fetching: true,
				items: action.data.data,
				name: action.data.name,
				error: "",
			}
		default:
			return state
	}
}

