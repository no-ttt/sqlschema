import { connect } from "react-redux"
import Assessment from '../components/Pages/Assessment'
import { GetCheckList } from "../actions/assessment"

const mapStateToProps = (state) => ({
	checkList: state.checkList,
})

const mapDispatchToProps = {
	GetCheckList,
}

export default connect(mapStateToProps, mapDispatchToProps)(Assessment)
