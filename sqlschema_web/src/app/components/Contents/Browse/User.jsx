import React, { Component } from "react"
import TableTemplate from '../../../containers/TableTemplate'

const userColumns = ["name", "create_date", "modify_date", "type", "authentication_type", "not_disabled", "is_policy_checked", "is_expiration_checked"]
const tablePrivilege = ["user_name", "table_name", "privilege_type", "is_grantable"]
const columnPrivilege = ["user_name", "table_name", "column_name", "privilege_type", "is_grantable"]

export default class User extends Component {
	render() {
		const { userList, tablePrivilegeList, columnPrivilegeList } = this.props
		return (
			<div style={{ width: "90%" }}>
				<div className="tb-title">User Information</div>
				<div className="subtitle">All Users</div>
				<TableTemplate columns={userColumns} data={userList} />
				{
					tablePrivilegeList.items.length !== 0 &&
					<div>
						<div className="subtitle">Table Privilege</div>
						<TableTemplate columns={tablePrivilege} data={tablePrivilegeList} />
					</div>
				}
				{
					columnPrivilegeList.items.length !== 0 &&
					<div>
						<div className="subtitle">Column Privilege</div>
						<TableTemplate columns={columnPrivilege} data={columnPrivilegeList} />
					</div>
				}
			</div>
		)
	}
}
