import { connect } from "react-redux"
import User from '../../../components/Tabs/Browse/User'
import { GetUserList, GetTablePrivilegeList, GetColumnPrivilegeList } from '../../../actions/user'

const mapStateToProps = (state) => ({
	userList: state.userList,
	tablePrivilegeList: state.tablePrivilegeList,
	columnPrivilegeList: state.columnPrivilegeList,
})

const mapDispatchToProps = {
	GetUserList,
	GetTablePrivilegeList,
	GetColumnPrivilegeList,
}

export default connect(mapStateToProps, mapDispatchToProps)(User)
