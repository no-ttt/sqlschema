import React, { Component } from "react"
import TableTemplate from '../../../containers/TableTemplate'
import ArrowRightIcon from '@mui/icons-material/ArrowRight'
import PushPinIcon from '@mui/icons-material/PushPin'
import { orange } from '@mui/material/colors'

const allTables = ["name", "created", "last_modified", "remark"]
const columns = ["id", "name", "data_type", "max_length", "precision", "not_nullable", "remark"]
const rel = ["foreign_table", "primary_table", "rel", "fk_constraint_name"]
const unique = ["key_name", "columns", "constraint_type"]
const index = ["index_name", "type", "columns", "index_type"]
const use = ["table_name"]
const tableSortList = ["name", "created", "last_modified", "remark"]
const columnSortList = ["id", "name", "data_type", "remark"]

export default class Table extends Component {
	render() {
		const { tab, tableList, columnList, relList, uniqueList, indexList, usesList, usedList } = this.props
		let tableDes = ""
		if (tableList.items.length !== 0 && columnList.items.length !== 0 && tab !== 2)
			tableDes = tableList.items.find(x => x.name === columnList.tab).remark
		
			return (
			<div>
				{ tab === 2 
					? <div>
							<div className="tb-title">All Tables</div>
							<TableTemplate columns={allTables} data={tableList} level={0} leveltype="TABLE" sortList={tableSortList} defaultSort="name" />
						</div>
					: <div>
							<div className="tb-title">{columnList.tab}</div>
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
								<TableTemplate columns={columns} data={columnList} level={1} leveltype="TABLE" tableName={columnList.tab} sortList={columnSortList} defaultSort="id"  />
							</div>
							{ relList.items.length !== 0 && 
								<div style={{ marginBottom: "40px" }}>
									<div style={{ display: "flex", alignItems: "center" }}>
										<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
										<div className="subtitle">Relations</div>
									</div>
									<TableTemplate columns={rel} data={relList} />
								</div>
							}
							{ uniqueList.items.length !== 0 &&
								<div style={{ marginBottom: "40px" }}>
									<div style={{ display: "flex", alignItems: "center" }}>
										<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
										<div className="subtitle">Unique Keys</div>
									</div>
									<TableTemplate columns={unique} data={uniqueList} />
								</div>
							}
							{ indexList.items.length !== 0 &&
								<div style={{ marginBottom: "40px" }}>
									<div style={{ display: "flex", alignItems: "center" }}>
										<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
										<div className="subtitle">Indexes</div>
									</div>
									<TableTemplate columns={index} data={indexList} />
								</div>
							}
							{ usesList.items.length !== 0 &&
								<div style={{ marginBottom: "40px" }}>
									<div style={{ display: "flex", alignItems: "center" }}>
										<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
										<div className="subtitle">References</div>
									</div>
									<TableTemplate columns={use} data={usesList} />
								</div>
							}
							{ usedList.items.length !== 0 &&
								<div style={{ marginBottom: "40px" }}>
									<div style={{ display: "flex", alignItems: "center" }}>
										<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
										<div className="subtitle">Referenced by</div>
									</div>
									<TableTemplate columns={use} data={usedList} />
								</div>
							}
						</div>
				}
			</div>
		)
	}
}
