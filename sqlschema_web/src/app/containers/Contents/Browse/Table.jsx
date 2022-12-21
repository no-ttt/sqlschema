import { connect } from "react-redux"
import Table from '../../../components/Contents/Browse/Table'

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
}

export default connect(mapStateToProps, mapDispatchToProps)(Table)
