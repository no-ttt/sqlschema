import React, { Component } from "react"
import ListItemButton from '@mui/material/ListItemButton'
import ListItemIcon from '@mui/material/ListItemIcon'
import ListItemText from '@mui/material/ListItemText'
import ImportContactsIcon from '@mui/icons-material/ImportContacts'

export default class Overview extends Component {
	componentDidMount() {
		const { GetCountList } = this.props
		GetCountList()
	}

	handleClick = () => {
		const { setData, GetCountList } = this.props
		GetCountList()
		setData(0, "Overview")
	}

	render() {
		const { click } = this.props
		return (
			<div>
				<ListItemButton onClick={this.handleClick} style={{ backgroundColor: click === "Overview" ? "#ececec" : "" }}>
					<ListItemIcon>
						<ImportContactsIcon />
					</ListItemIcon>
					<ListItemText primary="DB Overview" />
				</ListItemButton>
			</div>
		)
	}
}
