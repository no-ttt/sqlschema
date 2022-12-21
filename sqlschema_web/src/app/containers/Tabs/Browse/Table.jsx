import { connect } from "react-redux"
import Table from '../../../components/Tabs/Browse/Table'
import { GetTableList, GetColumnList, GetRelList, GetUniqueList, GetIndexList, GetUsesList, GetUsedList } from '../../../actions/table'

const mapStateToProps = (state) => ({
	tableList: state.tableList,
	columnList: state.columnList,
	relList: state.relList,
	uniqueList: state.uniqueList,
	indexList: state.indexList,
	usesList: state.usesList,
	usedList: state.usedList,
})

const mapDispatchToProps = {
	GetTableList,
	GetColumnList,
	GetRelList,
	GetUniqueList,
	GetIndexList,
	GetUsesList,
	GetUsedList,
}

export default connect(mapStateToProps, mapDispatchToProps)(Table)
