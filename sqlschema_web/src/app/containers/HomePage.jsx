import { connect } from "react-redux"
import HomePage from "../components/Pages/HomePage"

const mapStateToProps = (state) => ({
	docList: state.docList,
})

const mapDispatchToProps = {
}

export default connect(mapStateToProps, mapDispatchToProps)(HomePage)
