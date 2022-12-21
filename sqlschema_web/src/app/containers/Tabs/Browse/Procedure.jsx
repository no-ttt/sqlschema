import { connect } from "react-redux"
import Procedure from '../../../components/Tabs/Browse/Procedure'
import { GetProcList, GetProcObjUseList, GetProcObjUsedList, GetProcScriptList } from '../../../actions/proc'
import { GetParamList } from '../../../actions/param'

const mapStateToProps = (state) => ({
	procList: state.procList,
	paramList: state.paramList,
	procObjUseList: state.procObjUseList,
	procObjUsedList: state.procObjUsedList,
	procScriptList: state.procScriptList,
})

const mapDispatchToProps = {
	GetProcList,
	GetParamList,
	GetProcObjUseList,
	GetProcObjUsedList,
	GetProcScriptList,
}

export default connect(mapStateToProps, mapDispatchToProps)(Procedure)
