import { GET_CHECK_LIST, SET_CHECK_LIST } from "../actions/assessment"

const initState = {
	fetching: false,
	failedItems: [],
	passedItems: [],
	risk: [],
	error: ""
}

export function checkList(state = initState, action) {
	switch (action.type) {
		case GET_CHECK_LIST:
			return {
				...state,
				fetching: true,
				failedItems: [],
				passedItems: [],
				risk: [],
				error: "",
			}
		case SET_CHECK_LIST:
			return {
				...state,
				fetching: true,
				failedItems: action.data.data[0].failed,
				passedItems: action.data.data[0].passed,
				failed: action.data.count[0].failed,
				passed: action.data.count[0].passed,
				risk: action.data.count[0].risk,
				error: "",
			}
		default:
			return state
	}
}

