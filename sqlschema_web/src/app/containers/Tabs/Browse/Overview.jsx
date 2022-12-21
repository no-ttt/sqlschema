import { connect } from "react-redux"
import Overview from '../../../components/Tabs/Browse/Overview'
import { GetCountList } from "../../../actions/count"

const mapStateToProps = (state) => ({
	countList: state.countList,
})

const mapDispatchToProps = {
	GetCountList,
}

export default connect(mapStateToProps, mapDispatchToProps)(Overview)
