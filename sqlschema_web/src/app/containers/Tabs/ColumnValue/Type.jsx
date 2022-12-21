import { connect } from "react-redux"
import Type from '../../../components/Tabs/ColumnValue/Type'
import { GetTypeList } from "../../../actions/distribution"

const mapStateToProps = (state) => ({
	typeList: state.typeList,
})

const mapDispatchToProps = {
	GetTypeList,
}

export default connect(mapStateToProps, mapDispatchToProps)(Type)
