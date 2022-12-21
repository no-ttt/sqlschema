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
import FunctionsIcon from '@mui/icons-material/Functions'

export default class Function extends Component {
	constructor(props) {
		super(props);
		this.state = {
			open: false,
		}
	}

	handleClick = () => {
		const { setData, GetFuncList } = this.props
		this.setState({
			open: !this.state.open,
		})
		GetFuncList()
		setData(6, "Functions")
	}

	ColumnClick = (name) => {
		const { setData, GetParamList, GetFuncObjUseList, GetFuncObjUsedList, GetFuncScriptList } = this.props
		GetParamList(name)
		GetFuncObjUseList(name)
		GetFuncObjUsedList(name)
		GetFuncScriptList(name)
		setData(7, name)
	}

	render() {
		const { open } = this.state
		const { funcList, click } = this.props
		return (
			<div>
				<ListItemButton onClick={this.handleClick} style={{ backgroundColor: click === "Functions" ? "#ececec" : "" }}>
					<ListItemIcon>
						<FolderIcon />
					</ListItemIcon>
					<ListItemText primary="Functions" />
					{ open ? <ExpandLess /> : <ExpandMore /> }
				</ListItemButton>
				<Collapse in={open} timeout="auto" unmountOnExit>
					<List component="div" disablePadding>
						{
							funcList.items.map((f, i) => (
								<Link
									to={{
										pathname: '/sqlschema/Functions/' + f.name,
									}}
									className="link-default"
								>
									<ListItemButton key={i} sx={{ pl: 4 }} onClick={ () => this.ColumnClick(f.name) } style={{ backgroundColor: click === f.name ? "#ececec" : "" }}>
										<ListItemIcon>
											<FunctionsIcon />
										</ListItemIcon>
										<ListItemText primary={f.name} />
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
