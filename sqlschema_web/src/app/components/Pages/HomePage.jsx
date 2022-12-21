import React, { Component } from "react"
import { Link } from 'react-router-dom'
import bg from '../../img/home-bg.png'
import Divider from '@mui/material/Divider'
import Button from '@mui/material/Button'
import Dialog from '@mui/material/Dialog'
import MenuBookIcon from '@mui/icons-material/MenuBook'
import SearchIcon from '@mui/icons-material/Search'
import PrintIcon from '@mui/icons-material/Print'
import CategoryIcon from '@mui/icons-material/Category'
import CrisisAlertIcon   from '@mui/icons-material/CrisisAlert'
import PercentIcon from '@mui/icons-material/Percent'
import { apiurl } from "Config"

export default class HomePage extends Component {
	constructor(props) {
		super(props)
		this.state = {
			open: false,
			value: "",
		}
	}

	handleClick = () => {
		this.setState({
			open: true
		})
	}

	handleClose = (value) => {
		this.setState({
			open: false,
			value: value,
		})
	}

	render() {
		const { open } = this.state
		return (
			<div>
				<div className="home-header">
					<div className="home-title">資料庫資安健檢與分析系統</div>
					<a href="https://hackmd.io/@nooo/SkQkJjoJo" target="_blank" className="home-menu">
						<MenuBookIcon />
					</a>
				</div>
				<Divider />
				<img src={bg} alt="bg" className="home-bg" />
				<div style={{ display: "flex", justifyContent: "center", margin: "10px" }}>
					<div className="home-func-title">功能分類</div>
				</div>
				<div className="home-func">
					<div className="home-func-row">
						<Link
							to={{
								pathname: '/sqlschema/Overview/All',
							}}
							className="home-func-name"
						>
							<div className="home-func-item">
								<SearchIcon fontSize="large" />
								<div>資料庫瀏覽</div>
							</div>
						</Link>

						<div className="home-func-item" onClick={this.handleClick}>
							<PrintIcon fontSize="large" />
							<div>文件生成</div>
						</div>
						<Dialog open={open} onClose={() => this.setState({ open: false })}>
							{/* <DialogTitle>Template</DialogTitle> */}
							<div style={{ display: "flex", justifyContent: "space-around", width: 300, margin: 30 }}>
								<Button variant="outlined" size="large" onClick={() => this.handleClose("結案文件")}>
									<a href={`${apiurl}/Doc`} style={{ textDecoration: "none", color: "#1976d2" }} download>結案文件</a>
								</Button>
								<Button variant="outlined" size="large" onClick={() => this.handleClose("交接文件")}>
									<a href={`${apiurl}/Doc/Handover`} style={{ textDecoration: "none", color: "#1976d2" }} download>交接文件</a>
								</Button>
							</div>
						</Dialog>
						<Link
							to={{
								pathname: '/disk',
							}}
							className="home-func-name"
						>
							<div className="home-func-item">
								<PercentIcon fontSize="large" />
								<div>磁碟使用量</div>
							</div>
						</Link>
					</div>
					<div className="home-func-row">
						<Link
							to={{
								pathname: '/columnValue',
							}}
							className="home-func-name"
						>
							<div className="home-func-item">
								<CategoryIcon fontSize="large" />
								<div>值的分布狀態</div>
							</div>
						</Link>
						<Link
							to={{
								pathname: '/assessment',
							}}
							className="home-func-name"
						>
							<div className="home-func-item">
								<CrisisAlertIcon fontSize="large" />
								<div>資料庫風險評估</div>
							</div>
						</Link>
					</div>
				</div>
			</div>
		)
	}
}

