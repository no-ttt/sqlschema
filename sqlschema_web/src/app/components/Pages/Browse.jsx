import React, { Component } from "react"
import { Link } from 'react-router-dom'
import Overview from '../../containers/Tabs/Browse/Overview'
import User from '../../containers/Tabs/Browse/User'
import Table from '../../containers/Tabs/Browse/Table'
import View from '../../containers/Tabs/Browse/View'
import Function from '../../containers/Tabs/Browse/Function'
import Procedure from '../../containers/Tabs/Browse/Procedure'
import OverviewContent from '../../containers/Contents/Browse/Overview'
import UserContent from '../../containers/Contents/Browse/User'
import TableContent from '../../containers/Contents/Browse/Table'
import ViewContent from '../../containers/Contents/Browse/View'
import FunctionContent from '../../containers/Contents/Browse/Function'
import ProcedureContent from '../../containers/Contents/Browse/Procedure'
import Drawer from '@mui/material/Drawer'
import List from '@mui/material/List'
import HomeIcon from '@mui/icons-material/Home'
import SearchIcon from '@mui/icons-material/Search'

export default class Browse extends Component {
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
							<div className="drawer-title">資料庫瀏覽</div>
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
						<Link
							to={{
								pathname: '/sqlschema/Overview/All',
							}}
							className="link-default"
						>
							<Overview setData={this.setData} click={click} />
						</Link>
						<Link
							to={{
								pathname: '/sqlschema/Users/All',
							}}
							className="link-default"
						>
							<User setData={this.setData} click={click} />
						</Link>
						<Link
							to={{
								pathname: '/sqlschema/Tables/All',
							}}
							className="link-default"
						>
							<Table setData={this.setData} click={click} />
						</Link>
						<Link
							to={{
								pathname: '/sqlschema/Views/All',
							}}
							className="link-default"
						>
							<View setData={this.setData} click={click} />
						</Link>
						<Link
							to={{
								pathname: '/sqlschema/Functions/All',
							}}
							className="link-default"
						>
							<Function setData={this.setData} click={click} />
						</Link>
						<Link
							to={{
								pathname: '/sqlschema/Procedures/All',
							}}
							className="link-default"
						>
							<Procedure setData={this.setData} click={click} />
						</Link>
					</List>
				</Drawer>

				<div style={{ marginLeft: "400px", marginBottom: "100px" }}>
					{ (tab === 0)  && <OverviewContent /> }
					{ (tab === 1)  && <UserContent /> }
					{ (tab === 2 || tab === 3)  && <TableContent tab={tab} /> }
					{ (tab === 4 || tab === 5)  && <ViewContent tab={tab} /> }
					{ (tab === 6 || tab === 7)  && <FunctionContent tab={tab} /> }
					{ (tab === 8|| tab === 9)  && <ProcedureContent tab={tab} /> }
				</div>
				
			</div>
		)
	}
}

