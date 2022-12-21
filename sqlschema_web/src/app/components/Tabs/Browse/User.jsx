import React, { Component } from "react"
import ListItemButton from '@mui/material/ListItemButton'
import ListItemIcon from '@mui/material/ListItemIcon'
import ListItemText from '@mui/material/ListItemText'
import PersonIcon from '@mui/icons-material/Person'

export default class User extends Component {
	handleClick = () => {
		const { setData, GetUserList, GetTablePrivilegeList, GetColumnPrivilegeList } = this.props
		GetUserList()
		GetTablePrivilegeList()
		GetColumnPrivilegeList()
		setData(1, "User")
	}

	render() {
		const { click } = this.props
		return (
			<div>
				<ListItemButton onClick={this.handleClick} style={{ backgroundColor: click === "User" ? "#ececec" : "" }}>
					<ListItemIcon>
						<PersonIcon />
					</ListItemIcon>
					<ListItemText primary="Users Information" />
				</ListItemButton>
			</div>
		)
	}
}
