import { connect } from "react-redux"
import Overview from '../../../components/Tabs/ColumnValue/Overview'
import { GetValueList } from "../../../actions/distribution"
import { GetTableList } from '../../../actions/table'

const mapStateToProps = (state) => ({
	valueList: state.valueList,
	tableList: state.tableList,
})

const mapDispatchToProps = {
	GetValueList,
	GetTableList,
}

export default connect(mapStateToProps, mapDispatchToProps)(Overview)
