import { connect } from "react-redux"
import Function from '../../../components/Tabs/Browse/Function'
import { GetFuncList, GetFuncObjUseList, GetFuncObjUsedList, GetFuncScriptList } from '../../../actions/func'
import { GetParamList } from '../../../actions/param'

const mapStateToProps = (state) => ({
	funcList: state.funcList,
	paramList: state.paramList,
	funcObjUseList: state.funcObjUseList,
	funcObjUsedList: state.funcObjUsedList,
	funcScriptList: state.funcScriptList,
})

const mapDispatchToProps = {
	GetFuncList, 
	GetParamList,
	GetFuncObjUseList,
	GetFuncObjUsedList,
	GetFuncScriptList,
}

export default connect(mapStateToProps, mapDispatchToProps)(Function)
