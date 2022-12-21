import { connect } from "react-redux"
import TableTemplate from "../components/TableTemplate"
import { PostRemarkList } from "../actions/remark"
import { GetTableList, GetColumnList } from "../actions/table"
import { GetViewList, GetViewColumnList } from "../actions/view"
import { GetFuncList } from "../actions/func"
import { GetProcList } from "../actions/proc"
import { GetParamList } from "../actions/param"


const mapStateToProps = (state) => ({
	remarkList: state.remarkList,
})

const mapDispatchToProps = {
    PostRemarkList,
    GetTableList,
    GetColumnList,
    GetViewList,
    GetViewColumnList,
    GetFuncList,
    GetProcList,
    GetParamList,
}

export default connect(mapStateToProps, mapDispatchToProps)(TableTemplate)
