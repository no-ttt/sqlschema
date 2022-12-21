import { POST_REMARK_LIST, SET_REMARK_LIST } from "../actions/remark"

const initState = {
	fetching: false,
	items: [],
	error: ""
}

export function remarkList(state = initState, action) {
	switch (action.type) {
		case POST_REMARK_LIST:
			return {
				...state,
				fetching: true,
				items: [],
				error: "",
			}
		case SET_REMARK_LIST:
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

