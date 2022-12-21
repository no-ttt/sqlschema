import { GET_COUNT_LIST, SET_COUNT_LIST } from "../actions/count"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function countList(state = initState, action) {
	switch (action.type) {
		case GET_COUNT_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_COUNT_LIST:
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

