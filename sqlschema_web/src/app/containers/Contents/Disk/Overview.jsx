import { connect } from "react-redux"
import Overview from '../../../components/Contents/Disk/Overview'

const mapStateToProps = (state) => ({
	diskList: state.diskList,
	dataUsageList: state.dataUsageList,
})

const mapDispatchToProps = {
}

export default connect(mapStateToProps, mapDispatchToProps)(Overview)
