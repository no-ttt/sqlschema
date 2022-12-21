import React, { Component } from "react"
import { PieChart, BarChartStacked } from 'chart-component'

const datatype = ["data_type", "columns", "percent_columns", "tables", "percent_tables"]

export default class Type extends Component {
	render() {
		const { typeList } = this.props
		const pieData = typeList.items.map(({ data_type, columns, percent_columns }) => {
			return {
				count: columns,
				text: data_type,
				detail: percent_columns + "%",
			}
		})

		const barData = typeList.items.map(({ data_type, tables, percent_tables }) => {
			return {
				text: data_type,
				type: tables,
				non_type: typeList.count - tables,
			}
		})

		return (
			<div style={{ paddingRight: "60px" }}>
				<div className="tb-title">Data Type</div>
				<div style={{ display: "flex", justifyContent: "space-between", width: "80%" }}>
					{
						typeList.items.length !== 0 &&
						<PieChart
							data={pieData}
							height={420}
							width={460}
							getName={d => d.text}
							getValue={d => d.count}
							getDetail={d => d.detail}
							textSize={14}
							// marginTop={70}
							color={["#FA8072", "#FFDAB9", "#fcac63", "#FFD700", "#DDA0DD", "#a9dca2", "#87CEEB", "#082E54", "#8A2BE2", "#c4c4c4"]}
							chartTitleText = {"Columns 所占比率"}
						/>
					}
					{
						typeList.items.length !== 0 &&
						<BarChartStacked
							data={barData}
							getX={d => d.text}
							keysOfGroups={['type', 'non_type']}
							color={["#FFDAB9", "#87CEEB", "#c4c4c4"]}
							chartTitleText={"Tables 所占比率"}
							width={500}
							height={400}
							xAxisTicksTextRotation = {45}
						/>
					}
				</div>
				<div className="subtitle">Detail</div>
				<table className="tem-table">
					<tr className="tem-tr">
						{
							datatype.map((d, i) => (
								<th key={i}>{d}</th>
							))
						}
					</tr>
					{
						typeList.items.map((d, i) => (
							<tr key={i} className="tem-tr"> 
							{
								datatype.map((c, j) => (
									<td key={j} style={{ textAlign: isNaN(d[c]) ? "left" : "right" }}>{ d[c] }</td>
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
