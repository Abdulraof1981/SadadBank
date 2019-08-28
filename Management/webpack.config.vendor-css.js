const path = require('path');
const glob = require('glob-all')
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const PurifyCSSPlugin = require('purifycss-webpack');
var OptimizeCSSPlugin = require('optimize-css-assets-webpack-plugin');
const outputDir = './wwwroot';

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    return [{
        stats: { modules: false },
        entry: {            
            vendor: [path.resolve(path.resolve(__dirname, './node_modules/element-ui/lib/theme-chalk/index.css')],
        },
        output: {
            path: path.join(__dirname, outputDir),
            filename: 'css/[name].css',
            publicPath: '/'
        },
        resolve: {
            extensions: ['.js', '.css', '.less'],            
        },
        module: {
            rules: [                                
                {
                    test: /\.css$/,
                    use: isDevBuild ? ['style-loader', 'css-loader'] : ExtractTextPlugin.extract({ use: 'css-loader' })
                },
                {
                    test: /\.less/,
                    use: isDevBuild ? ['style-loader', 'css-loader', 'less-loader'] : ExtractTextPlugin.extract({ use: ['css-loader', 'less-loader'] })
                },
                {
                    test: /\.(png|jpg|jpeg|gif|svg)$/,
                    use: {
                        loader: 'url-loader',
                        options: {
                            limit: 8192,
                            name: 'img/[name].[ext]?[hash]'
                        }
                    }
                },
                {
                    test: /\.(eot|ttf|woff|woff2)(\?\S*)?$/,
                    use: {
                        loader: 'file-loader',
                        options: {
                            name: 'fonts/[name].[ext]?[hash]'
                        }
                    }
                }
            ]
        },
        plugins: [
            new OptimizeCSSPlugin({
                cssProcessorOptions: {
                    safe: true
                }
            }),
            new ExtractTextPlugin('css/[name].css'),
            // Uncomment the following line to strip out unused CSS
            //new PurifyCSSPlugin({
            //    paths: glob.sync(path.join(__dirname, 'App/*.html'))
            //})   
        ]
    }];
};
