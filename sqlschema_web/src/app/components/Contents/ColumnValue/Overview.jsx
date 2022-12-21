import React, { Component } from "react"
import MenuItem from '@mui/material/MenuItem'
import FormControl from '@mui/material/FormControl'
import InputLabel from '@mui/material/InputLabel'
import Select from '@mui/material/Select'
import { BarChartStacked } from 'chart-component'

const columns = ["col", "k", "n", "ratio"]

export default class Overview extends Component {
	componentDidMount() {
		const { GetValueList, tableList } = this.props
		tableList.items.length !== 0 &&
		GetValueList(tableList.items[0].name)
	}

	constructor(props) {
		super(props);
		this.state = {
			table: "",
		};
	}

	render() {
		const { GetValueList, valueList, tableList } = this.props
		const { table } = this.state
		const data = valueList.items.map(({ col, k, n }) => {
			return {
				text: col,
				unique: k,
				non_unique: n-k,
			}
		})

		return (
			<div style={{ paddingRight: "60px" }}>
				<div className="tb-title">Column Values</div>
				<FormControl sx={{ m: 1, minWidth: 150 }}>
					<InputLabel id="select-label" size="small">Table</InputLabel>
					{
						tableList.items.length !== 0 &&
						<Select
							labelId="select-label"
							id="select"
							size="small"
							value={table === "" ? tableList.items[0].name : table}
							label="table"
							onChange={(e) => {
								this.setState({ table: e.target.value })
								GetValueList(e.target.value)
							}}
						>
							{
								tableList.items.map((t, i) => (
									<MenuItem key={i} value={t.name}>{t.name}</MenuItem>
								))
							}
						</Select>
					}
				</FormControl>
				<div className="disk-chart-table-space">
					{
						valueList.items.length !== 0 &&
						<BarChartStacked
							data={data}
							getX={d => d.text}
							keysOfGroups={['unique', 'non_unique']}
							color={["#FFDAB9", "#87CEEB", "#c4c4c4"]}
							width={1200}
							height={500}
							xAxisTicksTextRotation={45}
						/>
					}
				</div>
				<div className="subtitle">Detail</div>
				<table className="tem-table">
					<tr className="tem-tr">
						{
							columns.map((c, i) => (
								<th key={i}>{c === "k" ? "唯一值" : c === "n" ? "總筆數" : c}</th>
							))
						}
					</tr>
					{
						valueList.items.map((d, i) => (
							<tr key={i} className="tem-tr">  
							{
								columns.map((c, j) => (
									<td key={j} style={{ textAlign: isNaN(d[c]) ? "left" : "right" }}>
										{ isNaN(d[c]) ? d[c] : c === 'k' || c === 'n' ? d[c] : d[c].toFixed(2) }
									</td>
								))
							}
							</tr>
						))
					}
				</table>
			</div>
		)
	}
}
