import React, { Component } from "react"

const columns = ["table_count", "view_count", "function_count", "procedure_count"]

export default class Overview extends Component {
	render() {
		const { countList } = this.props
		return (
			<div>
				<div className="tb-title">DB Overview</div>
				<table className="tem-table">
					<tr className="tem-tr">
						<th>Type</th>
						<th>Quantity</th>
					</tr>
					{ countList.items.length != 0 && 
						columns.map((c, i) => (
							<tr key={i} className="tem-tr">
								<td>{c}</td>
								<td style={{ textAlign: "right" }}>{countList.items[0][c]}</td>
							</tr>
						))
					}
				</table>
			</div>
		)
	}
}
