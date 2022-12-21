import { connect } from "react-redux"
import Overview from '../../../components/Contents/Browse/Overview'

const mapStateToProps = (state) => ({
	countList: state.countList,
})

const mapDispatchToProps = {
}

export default connect(mapStateToProps, mapDispatchToProps)(Overview)
