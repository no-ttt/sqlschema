import React, { Component } from "react"
import { connect } from "react-redux"
import { Switch, Route, BrowserRouter } from 'react-router-dom'
import HomePage from '../components/Pages/HomePage'
import Browse from '../components/Pages/Browse'
import Disk from '../components/Pages/Disk'
import ColumnValue from '../components/Pages/ColumnValue'
import Assessment from "./Assessment"

export class App extends Component {
	render() {
		return (
			<BrowserRouter>
				<Switch>
					<Route exact path="/" component={HomePage} />
					<Route exact path="/sqlschema/:type/:column" component={Browse} />
					<Route exact path="/disk" component={Disk} />
					<Route exact path="/columnValue" component={ColumnValue} />
					<Route exact path="/assessment" component={Assessment} />
				</Switch>
			</BrowserRouter>
		)
	}
}

const mapStateToProps = (state) => ({})

const mapDispatchToProps = {}

export default connect(mapStateToProps, mapDispatchToProps)(App)
