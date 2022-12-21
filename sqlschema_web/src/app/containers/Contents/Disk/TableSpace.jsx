import { connect } from "react-redux"
import TableSpace from '../../../components/Contents/Disk/TableSpace'

const mapStateToProps = (state) => ({
    tableUsageList: state.tableUsageList,
})

const mapDispatchToProps = {
}

export default connect(mapStateToProps, mapDispatchToProps)(TableSpace)
