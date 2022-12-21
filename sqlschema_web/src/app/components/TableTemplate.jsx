import React, { Component } from "react"
import { green, red } from '@mui/material/colors'
import CheckIcon from '@mui/icons-material/Check'
import CloseIcon from '@mui/icons-material/Close'
import EditIcon from '@mui/icons-material/Edit'
import SpellcheckIcon from '@mui/icons-material/Spellcheck'
import MenuItem from '@mui/material/MenuItem'
import FormControl from '@mui/material/FormControl'
import Select from '@mui/material/Select'

export default class TableTemplate extends Component {
	constructor(props) {
		super(props)
		this.state = {
			toggle: true,
			sort: this.props.defaultSort,
		}
	}

	toggleInput = () => {
		const { GetTableList, GetColumnList, GetViewList, GetViewColumnList, GetFuncList, GetProcList, GetParamList, tableName, leveltype } = this.props
		const { sort } = this.state

		this.setState({
			toggle: !this.state.toggle
		})

		if (!this.state.toggle) {
			switch (leveltype) {
				case "TABLE": {
					GetTableList(sort)
					GetColumnList(tableName)
				}
				case "VIEW": {
					GetViewList(sort)
					GetViewColumnList(tableName)
				}
				case "FUNCTION": {
					GetFuncList(sort)
					GetParamList(tableName)
				}
				case "PROCEDURE": {
					GetProcList(sort)
					GetParamList(tableName)
				}
			}
		}	
	}

	handleChange = (name) => {
		const { PostRemarkList, tableName, leveltype } = this.props

		if (tableName !== undefined)
			PostRemarkList(leveltype, event.target.value, tableName, name)
		else
			PostRemarkList(leveltype, event.target.value, name, "")
	}

	sortWay = (way) => {
		const { level, leveltype , GetTableList, GetViewList, GetFuncList, GetProcList, GetColumnList, GetViewColumnList, GetParamList, tableName } = this.props

		if (level === 0) {
			switch (leveltype) {
				case "TABLE": {
					GetTableList(way)
				}
				case "VIEW": {
					GetViewList(way)
				}
				case "FUNCTION": {
					GetFuncList(way)
				}
				case "PROCEDURE": {
					GetProcList(way)
				}
			}
		} else if (level === 1) {
			switch (leveltype) {
				case "TABLE": {
					GetColumnList(tableName, way)
				}
				case "VIEW": {
					GetViewColumnList(tableName, way)
				}
				case "FUNCTION": {
					GetParamList(tableName, way)
				}
				case "PROCEDURE": {
					GetParamList(tableName, way)
				}
			}
		}
		
	}

	render() {
		const { data, columns, leveltype, sortList } = this.props
		const { toggle, sort } = this.state
		return (
			<div>
				{
					(leveltype === "TABLE" || leveltype === "VIEW" || leveltype === "FUNCTION" || leveltype === "PROCEDURE") &&
					<div style={{ display: "flex", alignItems: "center", marginBottom: "20px" }}>
						<div>Order by : </div>
						<FormControl sx={{ m: 1, minWidth: 150 }}>
						{
							<Select
								labelId="select-label"
								id="select"
								size="small"
								value={sort}
								onChange={(e) => {
									this.setState({ 
										sort: e.target.value
									})
									this.sortWay(e.target.value)
								}}
							>
								{
									sortList.map((t, i) => (
										<MenuItem key={i} value={t}>{t}</MenuItem>
									))
								}
							</Select>
						}
						</FormControl>
					</div>
				}
				<div style={{ overflowX: "auto", paddingRight: "60px" }} id="style-3">
					<table className="tem-table">
						<tr className="tem-tr">
							{
								columns.map((t, i) => (
										<th key={i}>
											<div style={{ display: "flex", flexDirection: "row" }}>
												<div style={{ marginRight: "10px" }}>{t}</div>
													<button className="edit-btn" onClick={this.toggleInput}>
														{ t === "remark" && toggle ? <EditIcon fontSize="small" /> : t === "remark" && <SpellcheckIcon fontSize="small" /> }
													</button>
											</div>
										</th>
								))
							}
						</tr>
						{
							data.items.map((d, i) => (
								<tr key={i} className="tem-tr">
									{
										columns.map((c, j) => (
											d[c] === "True" || d[c] === "YES" || d[c] === true 
											? <td key={j} style={{ textAlign: "center" }}><CheckIcon sx={{ color: green[800] }} /></td> 
											: d[c] === "False" || d[c] === "NO" || d[c] === false 
												? <td key={j} style={{ textAlign: "center" }}><CloseIcon sx={{ color: red[500] }} /></td>
												: !toggle && columns[j] === "remark"
													? <td style={{ display: "flex", alignItems: "center" }}>
															<input key={j} type="text" defaultValue={d[c]} size="small" className="edit-input" onBlur={() => this.handleChange(d.name)} />
														</td>
													: <td key={j} style={{ textAlign: isNaN(d[c]) ? "left" : "right" }}>{d[c]}</td>	
										))
									}
								</tr>
							))
						}
					</table>
				</div>
			</div>
		)
	}
}
