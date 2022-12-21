import React, { Component } from "react"
import TableTemplate from '../../../containers/TableTemplate'
import ArrowRightIcon from '@mui/icons-material/ArrowRight'
import PushPinIcon from '@mui/icons-material/PushPin'
import { orange } from '@mui/material/colors'
import SyntaxHighlighter from 'react-syntax-highlighter'
import { a11yLight } from 'react-syntax-highlighter/dist/esm/styles/hljs'

const allFuncs = ["name", "created", "last_modified", "remark"]
const params = ["mode", "name", "data_type", "max_length", "precision", "not_nullable", "remark"]
const use = ["object_name", "object_type"]
const tableSortList = ["name", "created", "last_modified", "remark"]
const columnSortList = ["name", "mode", "data_type", "remark"]

export default class Function extends Component {
	render() {
		const { tab, funcList, paramList, funcObjUseList, funcObjUsedList, funcScriptList } = this.props
		let tableDes = ""
		if (funcList.items.length !== 0 && paramList.items.length !== 0 && tab !== 6)
			tableDes = funcList.items.find(x => x.name === paramList.name).remark
	
		return (
			<div>
				{
					tab === 6
					? <div>
							<div className="tb-title">All Functions</div>
							<TableTemplate columns={allFuncs} data={funcList} level={0} leveltype="FUNCTION" sortList={tableSortList} defaultSort="name" />
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
								<TableTemplate columns={params} data={paramList} level={1} leveltype="FUNCTION" tableName={paramList.name} sortList={columnSortList} defaultSort="name" />
							</div>
							{ funcObjUseList.items.length !== 0 &&
								<div style={{ marginBottom: "40px" }}>
									<div style={{ display: "flex", alignItems: "center" }}>
										<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
										<div className="subtitle">References</div>
									</div>
									<TableTemplate columns={use} data={funcObjUseList} />
								</div>
							}
							{ funcObjUsedList.items.length !== 0 &&
								<div style={{ marginBottom: "40px" }}>
									<div style={{ display: "flex", alignItems: "center" }}>
										<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
										<div className="subtitle">Referenced by</div>
									</div>
									<TableTemplate columns={use} data={funcObjUsedList} />
								</div>
							}
							<div style={{ marginBottom: "40px" }}>
								<div style={{ display: "flex", alignItems: "center" }}>
									<PushPinIcon sx={{ color: orange[500] }} fontSize="small" />
									<div className="subtitle">Script</div>
								</div>
								{
									funcScriptList.items.map((s, i) => (
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
