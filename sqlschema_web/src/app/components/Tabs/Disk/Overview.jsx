import React, { Component } from "react"
import ListItemButton from '@mui/material/ListItemButton'
import ListItemIcon from '@mui/material/ListItemIcon'
import ListItemText from '@mui/material/ListItemText'
import ImportContactsIcon from '@mui/icons-material/ImportContacts'

export default class Overview extends Component {
	componentDidMount() {
		const { GetDiskList, GetDataUsageList } = this.props
		GetDiskList()
		GetDataUsageList()
	}

	handleClick = () => {
		const { setData, GetDiskList, GetDataUsageList } = this.props
		GetDiskList()
		GetDataUsageList()
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
					<ListItemText primary="Disk Overview" />
				</ListItemButton>
			</div>
		)
	}
}
