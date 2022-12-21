import { connect } from "react-redux"
import Type from '../../../components/Contents/ColumnValue/Type'

const mapStateToProps = (state) => ({
	typeList: state.typeList,
})

const mapDispatchToProps = {
}

export default connect(mapStateToProps, mapDispatchToProps)(Type)
