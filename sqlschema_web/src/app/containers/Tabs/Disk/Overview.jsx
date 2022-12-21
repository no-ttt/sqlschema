import { connect } from "react-redux"
import Overview from '../../../components/Tabs/Disk/Overview'
import { GetDiskList, GetDataUsageList } from "../../../actions/disk"

const mapStateToProps = (state) => ({
	diskList: state.diskList,
	dataUsageList: state.dataUsageList,
})

const mapDispatchToProps = {
	GetDiskList,
	GetDataUsageList,
}

export default connect(mapStateToProps, mapDispatchToProps)(Overview)
