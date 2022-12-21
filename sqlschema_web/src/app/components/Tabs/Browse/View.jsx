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
import TableViewIcon from '@mui/icons-material/TableView'

export default class View extends Component {
	constructor(props) {
		super(props);
		this.state = {
			open: false,
		}
	}

	handleClick = () => {
		const { setData, GetViewList } = this.props
		this.setState({
			open: !this.state.open,
		})
		GetViewList()
		setData(4, "Views")
	}

	ColumnClick = (name) => {
		const { setData, GetViewColumnList, GetUseList, GetScriptList } = this.props
		this.setState({
			click: name,
		})
		GetViewColumnList(name)
		GetUseList(name)
		GetScriptList(name)
		setData(5, name)
	}

	render() {
		const { open } = this.state
		const { viewList, click } = this.props
		return (
			<div>
				<ListItemButton onClick={this.handleClick} style={{ backgroundColor: click === "Views" ? "#ececec" : "" }}>
					<ListItemIcon>
						<FolderIcon />
					</ListItemIcon>
					<ListItemText primary="Views" />
					{ open ? <ExpandLess /> : <ExpandMore /> }
				</ListItemButton>
				<Collapse in={open} timeout="auto" unmountOnExit>
					<List component="div" disablePadding>
						{
							viewList.items.map((v, i) => (
								<Link
									to={{
										pathname: '/sqlschema/Views/' + v.name,
									}}
									className="link-default"
								>
									<ListItemButton key={i} sx={{ pl: 4 }} onClick={ () => this.ColumnClick(v.name) } style={{ backgroundColor: click === v.name ? "#ececec" : "" }}>
										<ListItemIcon>
											<TableViewIcon />
										</ListItemIcon>
										<ListItemText primary={v.name} />
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
