import React, { Component } from "react"
import TableTemplate from '../../../containers/TableTemplate'
import ArrowRightIcon from '@mui/icons-material/ArrowRight'
import PushPinIcon from '@mui/icons-material/PushPin'
import { orange } from '@mui/material/colors'
import SyntaxHighlighter from 'react-syntax-highlighter'
import { a11yLight } from 'react-syntax-highlighter/dist/esm/styles/hljs'

const allProcs = ["name", "created", "last_modified", "remark"]
const params = ["mode", "name", "data_type", "max_length", "precision", "not_nullable", "remark"]
const use = ["object_name", "object_type"]
const tableSortList = ["name", "created", "last_modified", "remark"]
const columnSortList = ["name", "mode", "data_type", "remark"]

export default class Procedure extends Component {
	render() {
		const { tab, procList, paramList, procObjUseList, procObjUsedList, procScriptList } = this.props
		let tableDes = ""
		if (procList.items.length !== 0 && paramList.items.length !== 0 && tab !== 8)
			tableDes = procList.items.find(x => x.name === paramList.name).remark

		return (
			<div>
				{
					tab === 8
					? <div>
							<div className="tb-title">All Procedures</div>
							<TableTemplate columns={allProcs} data={procList} level={0} leveltype="PROCEDURE" sortList={tableSortList} defaultSort="name"  />
						</div>
					: <div>
							<div className="tb-title">{paramList.name}</div>
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
									<div className="subtitle">Input / Output</div>
								</div>
								<TableTemplate columns={params} data={paramList} level={1} leveltype="PROCEDURE" tableName={paramList.name} sortList={columnSortList} defaultSort="name" />
							</div>
							{ procObjUseList.items.length !== 0 &&
								<div style={{ marginBottom: "40px" }}>
									<div style={{ display: "flex", alignItems: "center" }}>
										<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
										<div className="subtitle">References</div>
									</div>
									<TableTemplate columns={use} data={procObjUseList} />
								</div>
							}
							{ procObjUsedList.items.length !== 0 &&
								<div style={{ marginBottom: "40px" }}>
									<div style={{ display: "flex", alignItems: "center" }}>
										<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
										<div className="subtitle">Referenced by</div>
									</div>
									<TableTemplate columns={use} data={procObjUsedList} />
								</div>
							}
							<div style={{ marginBottom: "40px" }}>
								<div style={{ display: "flex", alignItems: "center" }}>
									<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
									<div className="subtitle">Script</div>
								</div>
								{
									procScriptList.items.map((p, i) => (
										<div key={i} className="script">
											<SyntaxHighlighter language="sql" style={a11yLight}>{p.script}</SyntaxHighlighter>
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
