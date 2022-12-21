import action from "../lib/createAction"

// disk overview
export const GET_DISK_LIST = "GET_DISK_LIST"
export const GetDiskList = () => action(GET_DISK_LIST, {})

export const SET_DISK_LIST = "SET_DISK_LIST"
export const SetDiskList = (data) => action(SET_DISK_LIST, { data })


// data space usage
export const GET_DATA_USAGE_LIST = "GET_DATA_USAGE_LIST"
export const GetDataUsageList = () => action(GET_DATA_USAGE_LIST, {})

export const SET_DATA_USAGE_LIST = "SET_DATA_USAGE_LIST"
export const SetDataUsageList = (data) => action(SET_DATA_USAGE_LIST, { data })


// table space usage
export const GET_TABLE_USAGE_LIST = "GET_TABLE_USAGE_LIST"
export const GetTableUsageList = (tab) => action(GET_TABLE_USAGE_LIST, { tab })

export const SET_TABLE_USAGE_LIST = "SET_TABLE_USAGE_LIST"
export const SetTableUsageList = (data) => action(SET_TABLE_USAGE_LIST, { data })
