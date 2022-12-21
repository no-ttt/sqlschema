import React, { Component } from "react"
import TableTemplate from '../../../containers/TableTemplate'
import ArrowRightIcon from '@mui/icons-material/ArrowRight'
import PushPinIcon from '@mui/icons-material/PushPin'
import { orange } from '@mui/material/colors'
import SyntaxHighlighter from 'react-syntax-highlighter'
import { a11yLight } from 'react-syntax-highlighter/dist/esm/styles/hljs'

const allTables = ["name", "created", "last_modified", "remark"]
const columns = ["id", "name", "data_type", "max_length", "precision", "not_nullable", "remark"]
const use = ["table_name"]
const tableSortList = ["name", "created", "last_modified", "remark"]
const columnSortList = ["id", "name" , "data_type", "remark"]

export default class View extends Component {
	render() {
		const { tab, viewList, viewColumnList, useList, scriptList } = this.props
		let tableDes = ""
		if (viewList.items.length !== 0 && viewColumnList.items.length !== 0 && tab !== 4)
			tableDes = viewList.items.find(x => x.name === viewColumnList.tab).remark
		return (
			<div>
			{
				tab === 4
					? <div>
							<div className="tb-title">All View Tables</div>
							<TableTemplate columns={allTables} data={viewList} level={0} leveltype="VIEW" sortList={tableSortList} defaultSort="name" />
						</div>
					: <div>
							<div className="tb-title">{viewColumnList.tab}</div>
							{ 
								tableDes !== "" &&
								<div style={{ display: "flex", alignItems: "center" }}>
									<ArrowRightIcon />
									<div className="tb-des">{tableDes}</div> 
								</div>
							}
							<div style={{ marginBottom: "40px" }}>
								<div style={{ display: "flex", alignItems: "center" }}>
									<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
									<div className="subtitle">All Columns</div>
								</div>
								<TableTemplate columns={columns} data={viewColumnList} level={1} leveltype="VIEW" tableName={viewColumnList.tab} sortList={columnSortList} defaultSort="id" />
							</div>
							<div style={{ marginBottom: "40px" }}>
								<div style={{ display: "flex", alignItems: "center" }}>
									<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
									<div className="subtitle">References</div>
								</div>
								<TableTemplate columns={use} data={useList} />
							</div>
							<div style={{ marginBottom: "40px" }}>
								<div style={{ display: "flex", alignItems: "center" }}>
									<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
									<div className="subtitle">Script</div>
								</div>
								{
									scriptList.items.map((s, i) => (
										<div key={i} className="script">
											<SyntaxHighlighter language="sql" style={a11yLight}>{s.script}</SyntaxHighlighter>
										</div>
									))
								}
							</div>
						</div>
			}
			</div>
		)
	}
}
