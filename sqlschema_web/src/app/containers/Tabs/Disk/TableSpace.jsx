import { connect } from "react-redux"
import TableSpace from '../../../components/Tabs/Disk/TableSpace'
import { GetTableUsageList } from "../../../actions/disk"

const mapStateToProps = (state) => ({
	tableUsageList: state.tableUsageList,
})

const mapDispatchToProps = {
	GetTableUsageList,
}

export default connect(mapStateToProps, mapDispatchToProps)(TableSpace)
