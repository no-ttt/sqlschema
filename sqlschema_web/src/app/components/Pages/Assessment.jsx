import React, { Component } from "react"
import { Link } from 'react-router-dom'
import Row from '../Row'
import Tabs from '../Tabs'
import Table from '@material-ui/core/Table'
import TableBody from '@material-ui/core/TableBody'
import TableCell from '@material-ui/core/TableCell'
import TableContainer from '@material-ui/core/TableContainer'
import TableHead from '@material-ui/core/TableHead'
import TableRow from '@material-ui/core/TableRow'
import Paper from '@material-ui/core/Paper'
import CircularProgress from '@mui/material/CircularProgress'
import Button from '@mui/material/Button'
import { green, red } from '@mui/material/colors'
import CancelIcon from '@mui/icons-material/Cancel'
import CheckCircleIcon from '@mui/icons-material/CheckCircle'
import PlayCircleFilledWhiteIcon from '@mui/icons-material/PlayCircleFilledWhite'
import RestartAltIcon from '@mui/icons-material/RestartAlt'
import HomeIcon from '@mui/icons-material/Home'

const failedRows = ["id", "check", "category", "risk"];
const passedRows = ["id", "check", "category", "status"];

function add0(Completion) {
	return Completion < 10 ? "0" + Completion:Completion;
}

export default class Assessment extends Component {
	constructor(props) {
		super(props)
		this.state = {
			isClick: false,
		}
	}

	clickStart = () => {
		const { GetCheckList } = this.props
		GetCheckList()
		this.setState({
			isClick: true,
		})
	}

	render() {
		const { checkList } = this.props
		const { isClick } = this.state
		return (
			<div style={{padding: "50px 200px 30px 200px", position: "relative" }}>
				{
					(checkList.passedItems.length === 0 && checkList.failedItems.length === 0 && isClick) && 
					<div className="check-render">
						<CircularProgress style={{ color: "#d1dfe8" }} />
					</div>
				}
				<div style={{ display: "flex", flexDirection: "row", justifyContent: "space-between", width: "1500px" }}>
					<div className="check-title">Vulnerability Assessment Result</div>
					<div style={{ display: "flex", flexDirection: "row", hight: "30px" }}>
						<Link
							to={{
								pathname: '/',
							}}
							className="check-home-btn"
						>
							<HomeIcon fontSize="large" />
						</Link>
						<div style={{ width: "180px", marginLeft: "20px" }}>
							<Button variant="outlined" onClick={this.clickStart}>
								{ 
									isClick 
									? <div style={{ display: "flex", alignItems: "center" }}>
											<RestartAltIcon />
											<span className="check-time">重新評估</span>
										</div>
									: <div style={{ display: "flex", alignItems: "center" }}>
											<PlayCircleFilledWhiteIcon />
											<span className="check-time">開始評估</span>
										</div>
								}
							</Button>
							<div className="check-last-time" style={{ visibility: isClick ? "" : "hidden" }}>
								最後更新時間
								{ isClick ? " " + add0(new Date().getHours()) + ":" + add0(new Date().getMinutes()) + ":" + add0(new Date().getSeconds()) : "" }
							</div>
						</div>
					</div>
				</div>
				
				<div style={{ display: "flex", justifyContent: "space-between", width: "1100px", height: "100px" }}>
					<div className="check-result">
						<div>
							<div className="check-count-txt">Total failing checks</div>
							<div className="check-count-block">
								<div className="check-count-num">{checkList.failed}</div>
								<CancelIcon fontSize="large" style={{ color: red[500] }} />
							</div>
						</div>
						<div>
							<div className="check-count-txt">Total passing checks</div>
							<div className="check-count-block">
								<div className="check-count-num">{checkList.passed}</div>
								<CheckCircleIcon fontSize="large" style={{ color: green[800] }} />
							</div>
						</div>
					</div>
					{
						checkList.risk.length !== 0 &&
						<div className="check-risk-rank">
							<div className="check-risk-progress-individual high">
								<label for="high" className="check-risk-rank-label">High Risk</label>
								<progress id="high" value={checkList.risk[0].high} max={checkList.failed} />
							</div>
							<div className="check-risk-progress-individual medium">
								<label for="medium" className="check-risk-rank-label">Medium Risk</label>
								<progress id="medium" value={checkList.risk[0].medium} max={checkList.failed} />
							</div>
							<div className="check-risk-progress-individual low">
								<label for="low" className="check-risk-rank-label">Low Risk</label>
								<progress id="low" value={checkList.risk[0].low} max={checkList.failed} />
							</div>
						</div>
					}
					</div>
				
				<div style={{ marginTop: "30px" }}>
					<Tabs title={ ["Failed", "Passed"] }>
						<TableContainer component={Paper} style={{ marginTop: "20px", width: "1500px" }}>
							<Table aria-label="collapsible table" className="check-th">
								<TableHead>
									<TableRow>
										{/* <TableCell /> */}
										{ failedRows.map((row, i) => (
											<TableCell key={i} style={{ fontSize: "17px", fontWeight: "bolder" }}>{row}</TableCell>
										))}
									</TableRow>
								</TableHead>
								<TableBody>
									{
										checkList.failedItems.map((data, i) => (
											<Row key={i} row={data} status="failed" />
										))
									}
								</TableBody>
							</Table>
						</TableContainer>
						<TableContainer component={Paper} style={{ marginTop: "20px", width: "1500px" }}>
							<Table aria-label="collapsible table" className="check-th">
								<TableHead>
									<TableRow>
										{/* <TableCell /> */}
										{ passedRows.map((row, i) => (
											<TableCell key={i} style={{ fontSize: "17px", fontWeight: "bolder" }}>{row}</TableCell>
										))}
									</TableRow>
								</TableHead>
								<TableBody>
									{
										checkList.passedItems.map((data, i) => (
											<Row key={i} row={data} status="passed" />
										))
									}
								</TableBody>
							</Table>
						</TableContainer>
					</Tabs>
				</div>
			</div>
		)
	}
}

