import { connect } from "react-redux"
import Overview from '../../../components/Contents/ColumnValue/Overview'
import { GetValueList } from "../../../actions/distribution"

const mapStateToProps = (state) => ({
	valueList: state.valueList,
	tableList: state.tableList,
})

const mapDispatchToProps = {
	GetValueList,
}

export default connect(mapStateToProps, mapDispatchToProps)(Overview)
