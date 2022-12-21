import { connect } from "react-redux"
import View from '../../../components/Contents/Browse/View'

const mapStateToProps = (state) => ({
	viewList: state.viewList,
	viewColumnList: state.viewColumnList,
	useList: state.useList,
	scriptList: state.scriptList,
})

const mapDispatchToProps = {
}

export default connect(mapStateToProps, mapDispatchToProps)(View)
