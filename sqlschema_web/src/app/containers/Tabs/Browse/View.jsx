import { connect } from "react-redux"
import View from '../../../components/Tabs/Browse/View'
import { GetViewList, GetViewColumnList, GetUseList, GetScriptList } from '../../../actions/view'

const mapStateToProps = (state) => ({
	viewList: state.viewList,
	viewColumnList: state.viewColumnList,
	useList: state.useList,
	scriptList: state.scriptList,
})

const mapDispatchToProps = {
	GetViewList,
	GetViewColumnList,
	GetUseList,
	GetScriptList,
}

export default connect(mapStateToProps, mapDispatchToProps)(View)
