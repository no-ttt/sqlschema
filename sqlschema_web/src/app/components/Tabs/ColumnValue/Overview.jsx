import React, { Component } from "react"
import ListItemButton from '@mui/material/ListItemButton'
import ListItemIcon from '@mui/material/ListItemIcon'
import ListItemText from '@mui/material/ListItemText'
import SignalCellularAltIcon from '@mui/icons-material/SignalCellularAlt'

export default class Overview extends Component {
	componentDidMount() {
		const { GetTableList } = this.props
		GetTableList()
	}

	handleClick = () => {
		const { setData, GetTableList, GetValueList, tableList } = this.props
		GetTableList()
		tableList.items.length !== 0 &&
		GetValueList(tableList.items[0].name)
		setData(0, "Overview")
	}

	render() {
		const { click } = this.props
		return (
			<div>
				<ListItemButton onClick={this.handleClick} style={{ backgroundColor: click === "Overview" ? "#ececec" : "" }}>
					<ListItemIcon>
						<SignalCellularAltIcon />
					</ListItemIcon>
					<ListItemText primary="Column Values" />
				</ListItemButton>
			</div>
		)
	}
}
