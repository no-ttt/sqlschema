import React, { Component } from "react"
// import Collapse from '@material-ui/core/Collapse'
// import IconButton from '@material-ui/core/IconButton'
import TableCell from '@material-ui/core/TableCell'
import TableRow from '@material-ui/core/TableRow'
// import Box from '@material-ui/core/Box'
// import KeyboardArrowDownIcon from '@material-ui/icons/KeyboardArrowDown'
// import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp'
import CheckCircleIcon from '@mui/icons-material/CheckCircle'
import ErrorIcon from '@mui/icons-material/Error'
import ReportProblemIcon from '@mui/icons-material/ReportProblem'
import { blue, green } from '@mui/material/colors'
import SyntaxHighlighter from 'react-syntax-highlighter'
import { a11yLight } from 'react-syntax-highlighter/dist/esm/styles/hljs'
import Dialog from '@mui/material/Dialog'

export default class Row extends Component {
    constructor(props) {
			super(props)
			this.state = {
				open: false,
			}
    }

		handleClose = () => {
			this.setState ({
				open: false
			})
		}

    render() {
			const { open } = this.state
			const { row, status } = this.props
			return (
				<React.Fragment>
					<TableRow className="check-row" onClick={ () => this.setState({ open: true }) }>
						{/* <TableCell style={{  borderBottom: "none" }}>
							<IconButton
								aria-label="expand row"
								onClick={() => this.setState({ open: !open })}
							>
								{open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
							</IconButton>
						</TableCell> */}
						<TableCell style={{  borderBottom: "none", fontSize: "17px" }}>{row.id}</TableCell>
						<TableCell style={{  borderBottom: "none", fontSize: "17px" }}>{row.check}</TableCell>
						<TableCell style={{  borderBottom: "none", fontSize: "17px" }}>{row.category}</TableCell>
						<TableCell style={{  borderBottom: "none", fontSize: "17px" }}>
							{
								status === "failed"
								? <div style={{ display: "flex", alignItems: "center" }}>
										{ 
										row.risk === "Medium"
											? <ReportProblemIcon fontSize="small" style={{ color: "orange", marginRight: "5px" }} />
											: <ErrorIcon fontSize="small" style={{ color: row.risk === "High" ? "red" : blue[800], marginRight: "5px" }} />
										}
											{row.risk}
									</div>
								: <div style={{ display: "flex", alignItems: "center" }}>
										<CheckCircleIcon fontSize="small" style={{ color: green[800], marginRight: "5px"  }} />
										Pass
									</div>
							}
							
						</TableCell>
					</TableRow>
					<Dialog onClose={this.handleClose} open={open} maxWidth="1800px">
						<div style={{ padding: "40px" }}>
							<table>
								<tr className="check-td">
									<td>Name</td>
									<td>{row.id + " - " + row.check}</td>
								</tr>
								<tr className="check-td">
									<td>Description</td>
									<td>{row.des}</td>
								</tr>
								<tr className="check-td">
									<td>Impact</td>
									<td>{row.impact}</td>
								</tr>
								<tr className="check-td">
									<td>Rule Query</td>
									<td>
										<div className="check-script">
											<SyntaxHighlighter language="sql" style={a11yLight}>{row.query}</SyntaxHighlighter>
										</div>
										
									</td>
								</tr>
								{/* {
									row.result.length !== 0 && 
									<tr className="check-td">
										<td>Result</td>
										<td style={{ padding: "30px" }}>
											<table className="check-result-tb">
												<tr className="check-result-tr">
													{
														Object.keys(row.result[0]).map((r, i) => (
															<th key={i}>{r}</th>
														))
													}
												</tr>
												{
													row.result.map((r, i) => (
														<tr key={i}>
															{
																Object.keys(row.result[0]).map((subtitle, j) => (
																	<td key={j}>{r[subtitle]}</td>
																))
															}
														</tr>
													))
												}
											</table>
										</td>
									</tr>
								} */}
								<tr className="check-td">
									<td>Remediation</td>
									<td>{row.remediation}</td>
								</tr>
								<tr className="check-td">
									<td>Remediation Script</td>
									<td>{row.remediationScript}</td>
								</tr>
							</table>
						</div>
					</Dialog>
					{/* <TableRow>
						<TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={5}>
							<Collapse in={open} timeout="auto" unmountOnExit>
								<div style={{ borderTop: "1px solid rgba(224, 224, 224, 1)", width: "94%", margin: " 0 0 0 80px" }} />
								<Box style={{ margin: "0 80px 0 120px", padding: "20px" }}>
									<table>
										<tr className="check-td">
											<td>Name</td>
											<td>{row.id + " - " + row.check}</td>
										</tr>
										<tr className="check-td">
											<td>Description</td>
											<td>{row.des}</td>
										</tr>
										<tr className="check-td">
											<td>Impact</td>
											<td>{row.impact}</td>
										</tr>
										<tr className="check-td">
											<td>Rule Query</td>
											<td>
												<div className="check-script" id="style-3">
													<SyntaxHighlighter language="sql" style={a11yLight}>{row.query}</SyntaxHighlighter>
												</div>
												
											</td>
										</tr>
										{
											row.result.length !== 0 && 
											<tr className="check-td">
												<td>Result</td>
												<td style={{ padding: "30px" }}>
													<table className="check-result-tb">
														<tr className="check-result-tr">
															{
																Object.keys(row.result[0]).map((r, i) => (
																	<th key={i}>{r}</th>
																))
															}
														</tr>
														{
															row.result.map((r, i) => (
																<tr key={i}>
																	{
																		Object.keys(row.result[0]).map((subtitle, j) => (
																			<td key={j}>{r[subtitle]}</td>
																		))
																	}
																</tr>
															))
														}
													</table>
												</td>
											</tr>
										}
										<tr className="check-td">
											<td>Remediation</td>
											<td>{row.remediation}</td>
										</tr>
										<tr className="check-td">
											<td>Remediation Script</td>
											<td>{row.remediationScript}</td>
										</tr>
									</table>
								</Box>
							</Collapse>
						</TableCell>
					</TableRow> */}
				</React.Fragment>
			)
    }
}