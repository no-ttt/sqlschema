import { connect } from "react-redux"
import User from '../../../components/Contents/Browse/User'

const mapStateToProps = (state) => ({
	userList: state.userList,
	tablePrivilegeList: state.tablePrivilegeList,
	columnPrivilegeList: state.columnPrivilegeList,
})

const mapDispatchToProps = {
}

export default connect(mapStateToProps, mapDispatchToProps)(User)
