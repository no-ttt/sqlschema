import { connect } from "react-redux"
import Function from '../../../components/Contents/Browse/Function'

const mapStateToProps = (state) => ({
	funcList: state.funcList,
	paramList: state.paramList,
	funcObjUseList: state.funcObjUseList,
	funcObjUsedList: state.funcObjUsedList,
	funcScriptList: state.funcScriptList,
})

const mapDispatchToProps = {
}

export default connect(mapStateToProps, mapDispatchToProps)(Function)
