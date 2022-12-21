import React, { Component } from "react"
import { PieChart } from 'chart-component'

const columns = ["name", "size", "spaceUsed", "spaceRemain", "position"]
const dataSpace = ["database_size", "unallocated_space", "reserved", "data", "index_size", "unused"]

export default class Overview extends Component {
	render() {
		const { diskList, dataUsageList } = this.props
		return (
			<div style={{ paddingRight: "60px" }}>
				<div className="tb-title">Disk Overview</div>
				{
					dataUsageList.items.length !== 0 &&
					<div className="disk-overview">database size: {dataUsageList.items[0].database_size} MB</div>
				}
				<table className="tem-table">
					<tr  className="tem-tr">
						{
							columns.map((c, i) => (
								<th key={i}>{c}{ c === "size" || c === "spaceUsed" || c === "spaceRemain" ? "(MB)" : "" }</th>
							))
						}
					</tr>
					{
						diskList.items.map((d, i) => (
							<tr key={i}  className="tem-tr"> 
							{
								columns.map((c, j) => (
									<td key={j} style={{ textAlign: isNaN(d[c]) ? "left" : "right" }}>{ isNaN(d[c]) ? d[c] : d[c].toFixed(2) }</td>
								))
							}
							</tr>
						))
					}
				</table>
				<div>
					{
						dataUsageList.items.length !== 0 &&
						<div className="disk-chart">
							<div className="disk-chart-frame">
								<PieChart
									data={
										[{
											count: dataUsageList.items[0].unallocated_space,
											text: "unallocated_space",
											detail: "未配置空間"
										}, {
											count: dataUsageList.items[0].data,
											text: "data",
											detail: "資料所用總量"
										}, {
											count: dataUsageList.items[0].index_size,
											text: "index_size",
											detail: "索引所用總量"
										}, {
											count: dataUsageList.items[0].unused,
											text: "unused",
											detail: "保留給物件但未使用空間"
										}]
									}
									color={["#FA8072", "#FFDAB9", "#fcac63", "#DDA0DD", "#c4c4c4"]}
									format=".2f"
									height={350}
									width={350}
									marginTop={80}
									getName={d => d.text}
									getValue={d => d.count}
									getDetail={d => d.detail}
									chartTitleText={"資料檔案空間使用"}
									textSize={14}
								/>
							</div>
							<div className="disk-chart-frame">
								<PieChart
									data = {
										[
											{
												count: diskList.items[1].spaceUsed,
												text: "spaceUsed",
												detail: "已使用"
											}, {
												count: diskList.items[1].spaceRemain,
												text: "spaceRemain",
												detail: "未使用"
											}
										]
									}
									color={["#a9dca2", "#87CEEB", "#c4c4c4"]}
									format=".2f"
									height={350}
									width={350}
									marginTop={80}
									getName={d => d.text}
									getValue={d => d.count}
									getDetail={d => d.detail}
									chartTitleText={"交易空間使用"}
									textSize={14}
								/>
							</div>
						</div>
					}
				</div>
				
				<div className="subtitle">Data Space Usage</div>
				<table className="tem-table">
					<tr className="tem-tr">
						{
							dataSpace.map((c, i) => (
								<th key={i}>{c}(MB)</th>
							))
						}
					</tr>
					{
						dataUsageList.items.map((d, i) => (
							<tr key={i} className="tem-tr"> 
							{
								dataSpace.map((c, j) => (
									<td key={j} style={{ textAlign: "right" }}>{ d[c].toFixed(2) }</td>
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
