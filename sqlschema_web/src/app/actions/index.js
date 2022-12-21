const importAll = require('../lib/action').importAll

const cnt = require.context(__dirname, true, /\.js$/)
const actionsNames = importAll(cnt, true)

module.exports = actionsNames
