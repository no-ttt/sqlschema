import React, { Component } from "react"

export default class Tabs extends Component {
	constructor(props) {
		super(props)
		this.state = {
			isClick: 0,
		}
	}

	render() {
		const { isClick } = this.state
		const { title, children } = this.props
		return (
			<div>
				<ul className="tabs-ul">
					{
						title.map((subtitle, i) => (
							<li className="tabs-li">
								<button key={i} className={ isClick == i ? "nav-click focus" : "nav-click" } 
									onClick = {() => this.setState({ isClick: i })}
								>
									{ subtitle }
								</button>
							</li>
						))
					}
				</ul>
				<div className="tab-content">{ children[isClick] }</div>
			</div>
		)
	}
}