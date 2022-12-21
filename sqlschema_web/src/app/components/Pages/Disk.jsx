import React, { Component } from "react"
import { Link } from 'react-router-dom'
import Overview from '../../containers/Tabs/Disk/Overview'
import TableSpace from '../../containers/Tabs/Disk/TableSpace'
import OverviewContent from '../../containers/Contents/Disk/Overview'
import TableSpaceContent from '../../containers/Contents/Disk/TableSpace'
import Drawer from '@mui/material/Drawer'
import List from '@mui/material/List'
import HomeIcon from '@mui/icons-material/Home'
import SearchIcon from '@mui/icons-material/Search'

export default class Disk extends Component {
	constructor(props) {
		super(props)
		this.state = {
			tab: 0,
			click: "",
		}
	}

	_handleKeyDown = (e) => {
		if (e.key === 'Enter') {
		  alert(e.target.value);
		}
	}

	setData = (tab, name) => {
		this.setState({
			tab: tab,
			click: name
		})
	}

	render() {
		const { tab, click } = this.state
		return (
			<div>
				<Drawer variant="permanent" open={true}>
					<List className="drawer-list-width">
						<div className="drawer-title-pos">
							<div className="drawer-title">磁碟使用量</div>
							<Link
								to={{
									pathname: '/',
								}}
								className="drawer-home-btn"
							>
								<HomeIcon fontSize="large" />
							</Link>
						</div>
						{/* <div className="searchbox_block">
							<SearchIcon className="searchbox_icon" />
							<input type="text" placeholder="開始搜尋" 
								className="searchbox_input"
								onKeyDown={this._handleKeyDown}
							/>
						</div> */}
						<Overview setData={this.setData} click={click} />
						<TableSpace setData={this.setData} click={click} />
					</List>
				</Drawer>
				<div style={{ marginLeft: "400px", marginBottom: "100px" }}>
					{ (tab === 0)  && <OverviewContent /> }
					{ (tab === 1)  && <TableSpaceContent /> }
				</div>
			</div>
		)
	}
}

