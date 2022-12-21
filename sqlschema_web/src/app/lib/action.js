function action (type, payload = {}) {
  return {
    type,
    ...payload
  }
}

export function importAll (cnt, noMatch = false) {
  let fileNames = {}
  // 取得當前目錄下所有 *.js 檔案內，有導出(export)的宣告
  // ex. export const, export let, export function...
  cnt.keys().forEach((key) => {
    // export function of every *.js file
    const func = cnt(key)
    // assign function to dict
    Object.keys(func).map((f) => {
      if (!noMatch) {
        // regex to get filename not extension
        let filename = key.match(/[^/]*(?=\.[^.]+($|\?))/i)[0]
        // use split to get first filename
        filename = filename.split('.')[0]
        fileNames[filename] = func[f]
      } else {
        fileNames[f] = func[f]
      }
    })
  })
  return fileNames
}

export default action
