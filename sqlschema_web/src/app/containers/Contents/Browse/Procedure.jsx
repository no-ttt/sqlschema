import { connect } from "react-redux"
import Procedure from '../../../components/Contents/Browse/Procedure'

const mapStateToProps = (state) => ({
	procList: state.procList,
	paramList: state.paramList,
	procObjUseList: state.procObjUseList,
	procObjUsedList: state.procObjUsedList,
	procScriptList: state.procScriptList,
})

const mapDispatchToProps = {
}

export default connect(mapStateToProps, mapDispatchToProps)(Procedure)
