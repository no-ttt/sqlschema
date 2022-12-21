import React, { Component } from "react"
import ListItemButton from '@mui/material/ListItemButton'
import ListItemIcon from '@mui/material/ListItemIcon'
import ListItemText from '@mui/material/ListItemText'
import TableChartOutlinedIcon from '@mui/icons-material/TableChartOutlined'

export default class TableSpace extends Component {
	handleClick = () => {
		const { setData, GetTableUsageList } = this.props
		GetTableUsageList()
		setData(1, "TableSpace")
	}

	render() {
		const { click } = this.props
		return (
			<div>
				<ListItemButton onClick={this.handleClick} style={{ backgroundColor: click === "TableSpace" ? "#ececec" : "" }}>
					<ListItemIcon>
						<TableChartOutlinedIcon />
					</ListItemIcon>
					<ListItemText primary="Table Space" />
				</ListItemButton>
			</div>
		)
	}
}
