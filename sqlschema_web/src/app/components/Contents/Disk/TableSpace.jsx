import React, { Component } from "react"
import { BarChartStacked } from 'chart-component'

const tableSpace = ["tableName", "rows", "totalPages", "usedPages", "dataPages", "totalSpaceKB", "usedSpaceKB", "dataSpaceKB"]

export default class TableSpace extends Component {
	render() {
		const { tableUsageList } = this.props
		const data = tableUsageList.items.map(({ tableName, totalSpaceKB, usedSpaceKB }) => {
			return {
				text: tableName,
				usedSpaceKB: usedSpaceKB,
				unusedSpaceKB: totalSpaceKB - usedSpaceKB,
			}
		})

		return (
			<div>
				<div className="tb-title">Table Space</div>
				<div className="disk-chart-table-space">
					{
						tableUsageList.items.length !== 0 &&
						<BarChartStacked
							data={data}
							getX={d => d.text}
							keysOfGroups={['usedSpaceKB', 'unusedSpaceKB']}
							chartTitleText={"TableSpace"}
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
							tableSpace.map((c, i) => (
								<th key={i}>{c}</th>
							))
						}
					</tr>
					{
						tableUsageList.items.map((d, i) => (
							<tr key={i} className="tem-tr"> 
							{
								tableSpace.map((c, j) => (
									<td key={j} style={{ textAlign: isNaN(d[c]) ? "left" : "right" }}>{d[c]}</td>
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
