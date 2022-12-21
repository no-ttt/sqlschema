import React, { Component } from "react"
import { Link } from 'react-router-dom'
import List from '@mui/material/List'
import ListItemButton from '@mui/material/ListItemButton'
import ListItemIcon from '@mui/material/ListItemIcon'
import ListItemText from '@mui/material/ListItemText'
import Collapse from '@mui/material/Collapse'
import FolderIcon from '@mui/icons-material/Folder'
import ExpandLess from '@mui/icons-material/ExpandLess'
import ExpandMore from '@mui/icons-material/ExpandMore'
import TableChartOutlinedIcon from '@mui/icons-material/TableChartOutlined'

export default class Table extends Component {
	constructor(props) {
		super(props);
		this.state = {
			open: false,
		}
	}

	handleClick = () => {
		const { setData, GetTableList } = this.props
		this.setState({
			open: !this.state.open,
		})
		GetTableList()
		setData(2, "Tables")
	}

	ColumnClick = (name) => {
		const { setData, GetColumnList, GetRelList, GetUniqueList, GetIndexList, GetUsesList, GetUsedList } = this.props
		GetColumnList(name)
		GetRelList(name)
		GetUniqueList(name)
		GetIndexList(name)
		GetUsesList(name)
		GetUsedList(name)
		setData(3, name)
	}

	render() {
		const { open } = this.state
		const { tableList, click } = this.props
		return (
			<div>
				<ListItemButton onClick={this.handleClick} style={{ backgroundColor: click === "Tables" ? "#ececec" : "" }}>
					<ListItemIcon>
						<FolderIcon />
					</ListItemIcon>
					<ListItemText primary="Tables" />
					{ open ? <ExpandLess /> : <ExpandMore /> }
				</ListItemButton>
				<Collapse in={open} timeout="auto" unmountOnExit>
					<List component="div" disablePadding>
						{
							tableList.items.map((t, i) => (
								<Link
									to={{
										pathname: '/sqlschema/Tables/' + t.name,
									}}
									className="link-default"
								>
									<ListItemButton key={i} sx={{ pl: 4 }} onClick={ () => this.ColumnClick(t.name) } style={{ backgroundColor: click === t.name ? "#ececec" : "" }}>
										<ListItemIcon>
											<TableChartOutlinedIcon />
										</ListItemIcon>
										<ListItemText primary={t.name} />
									</ListItemButton>
								</Link>

							))
						}
					</List>
				</Collapse>
			</div>
		)
	}
}
