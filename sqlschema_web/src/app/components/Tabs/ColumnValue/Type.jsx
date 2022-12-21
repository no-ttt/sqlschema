import React, { Component } from "react"
import ListItemButton from '@mui/material/ListItemButton'
import ListItemIcon from '@mui/material/ListItemIcon'
import ListItemText from '@mui/material/ListItemText'
import PieChartIcon from '@mui/icons-material/PieChart'

export default class Type extends Component {
	handleClick = () => {
		const { setData, GetTypeList } = this.props
		GetTypeList()
		setData(1, "Type")
	}

	render() {
		const { click } = this.props
		return (
			<div>
				<ListItemButton onClick={this.handleClick} style={{ backgroundColor: click === "Type" ? "#ececec" : "" }}>
					<ListItemIcon>
						<PieChartIcon />
					</ListItemIcon>
					<ListItemText primary="Data Type" />
				</ListItemButton>
			</div>
		)
	}
}
