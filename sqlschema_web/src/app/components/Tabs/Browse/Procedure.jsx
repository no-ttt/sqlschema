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
import DisplaySettingsIcon from '@mui/icons-material/DisplaySettings'

export default class Procedure extends Component {
	constructor(props) {
		super(props);
		this.state = {
			open: false,
		}
	}

	handleClick = () => {
		const { setData, GetProcList } = this.props
		this.setState({
			open: !this.state.open,
		})
		GetProcList()
		setData(8, "Procedures")
	}

	ColumnClick = (name) => {
		const { setData, GetParamList, GetProcObjUseList, GetProcObjUsedList, GetProcScriptList } = this.props
		GetParamList(name)
		GetProcObjUseList(name)
		GetProcObjUsedList(name)
		GetProcScriptList(name)
		setData(9, name)
	}

	render() {
		const { open } = this.state
		const { procList, click } = this.props
		return (
			<div>
				<ListItemButton onClick={this.handleClick} style={{ backgroundColor: click === "Procedures" ? "#ececec" : "" }}>
					<ListItemIcon>
						<FolderIcon />
					</ListItemIcon>
					<ListItemText primary="Procedures" />
					{ open ? <ExpandLess /> : <ExpandMore /> }
				</ListItemButton>
				<Collapse in={open} timeout="auto" unmountOnExit>
					<List component="div" disablePadding>
						{
							procList.items.map((p, i) => (
								<Link
									to={{
										pathname: '/sqlschema/Procedures/' + p.name,
									}}
									className="link-default"
								>
									<ListItemButton key={i} sx={{ pl: 4 }} onClick={ () => this.ColumnClick(p.name) } style={{ backgroundColor: click === p.name ? "#ececec" : "" }}>
										<ListItemIcon>
											<DisplaySettingsIcon />
										</ListItemIcon>
										<ListItemText primary={p.name} />
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
