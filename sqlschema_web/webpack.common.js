const webpack = require('webpack')
const {
    CleanWebpackPlugin
} = require('clean-webpack-plugin')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const {
    title
} = require('./config.json');

const path = require('path') // 需新增
const CopyPlugin = require('copy-webpack-plugin') // 需新增

const config = {

    entry: {

        vendors: [ 'react', 'react-dom'],
        app: [__dirname + '/src/app/index.jsx'],
  
    },
    resolve: {
        extensions: ['.jsx', '.js', '.styl'],
        alias: {
            Config:__dirname+'/config.dev.json'
        }
    },
    output: {
        path: __dirname + '/dist/',
        publicPath: '/',   // 加上這個可以重新整理頁面不會壞掉
        filename: '[name].js',
    },

    module: {
        rules: [{
                test: /\.jsx?$/,
                use: [ {
                    loader: 'babel-loader'
                }],
                exclude: /node_modules/
            }, {
                test: /\.styl?/,
                use: [{
                    loader: 'style-loader'
                }, {
                    loader: 'css-loader'
                }, {
                    loader: 'stylus-loader'
                }]
            }, {
                test: /\.(png|jpg|gif|svg|eot|ttf|woff|woff2)$/,
                loader: 'url-loader',
                options: {
                    limit: 10000
                }
            }, {
                use: {
                    loader: 'file-loader',
                    options: {
                        name: 'font/[hash].[ext]'
                    }
                },
                test: /\.(woff|woff2|eot|ttf|otf)$/i,
                exclude: /node_modules/
            }

        ]
    },
    plugins: [
        new CleanWebpackPlugin(),
        new CopyPlugin({
            patterns: [{ from: 'src/public', to: path.resolve(__dirname, "dist/"), }]
        }),            
        new HtmlWebpackPlugin({
            title: title,
            template: 'src/public/index.html',
            inject: true
            // minify: {
            //   removeComments: true, // 清理html中的註釋
            //   removeEmptyElements: false, // 清理內容為空的元素
            //   collapseWhitespace: true, // 清理html中的空格、換行符
            //   minifyCSS: true, // 壓縮html內的樣式
            //   minifyJS: true, // 壓縮html內的js (not working)
            //   preserveLineBreaks: false // 保留換行符
            // }
          }),
    ], optimization: {
        // runtimeChunk: 'single',
        splitChunks: {
          chunks: 'all',
          maxInitialRequests: Infinity,
          minSize: 0,
          cacheGroups: {
            vendors: {
              test: /[\\/]node_modules[\\/]/,
              name: 'vendors',
              // name (module) {
              //   // get the name. E.g. node_modules/packageName/not/this/part.js
              //   // or node_modules/packageName
              //   const packageName = module.context.match(/[\\/]node_modules[\\/](.*?)([\\/]|$)/)[1]
  
              //   // npm package names are URL-safe, but some servers don't like @ symbols
              //   return `npm.${packageName.replace('@', '')}`
              // },
              enforce: true
            },
            // styles: {
            //   name: 'styles',
            //   test: /\.css$/,
            //   chunks: 'all',
            //   enforce: true
            // }
          }
        }
      },


}


module.exports = config