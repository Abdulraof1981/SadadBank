const path = require('path');
const glob = require('glob-all');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const PurifyCSSPlugin = require('purifycss-webpack');
var OptimizeCSSPlugin = require('optimize-css-assets-webpack-plugin');
var CopyWebpackPlugin = require('copy-webpack-plugin');

const outputDir = './wwwroot';

module.exports = (env) => {
    const isDevBuild = !(env && env.prod);
    return [{
        stats: { modules: false },
        entry: {
            main: ['./App/app.js', './Content/site.css'],  
            login: ['./App/login.js', './Content/login.css'],     
            vueui: [path.resolve(__dirname, './node_modules/vuetify/dist/vuetify.min.css'), path.resolve(__dirname, './node_modules/element-ui/lib/theme-chalk/index.css')],
            vendor: ['vue', 'vue-router', 'axios']
        },
        output: {
            path: path.join(__dirname, outputDir),
            filename: 'js/[name].js',
            publicPath: '/',
            hotUpdateChunkFilename: 'hot-update.js',
            hotUpdateMainFilename: 'hot-update.json'
        },
        resolve: {
            extensions: ['.js', '.vue'],
            alias: {
                'vue$': 'vue/dist/vue.common.js'
            }
        },
        module: {           
            rules: [
                {
                    test: /\.vue$/,
                    include: /App/,
                    use: 'vue-loader'
                },
                {
                    test: /\.js$/,
                    include: /App/,
                    use: 'babel-loader',
					exclude: /node_modules/
                },
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
           
            new webpack.optimize.CommonsChunkPlugin({
                name: ['vendor', 'manifest'],
                minChunks: Infinity
            }),
            new CopyWebpackPlugin([{ from: 'Content/Images', to: 'img' }])
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            //new webpack.SourceMapDevToolPlugin({
            //    filename: '[file].map', // Remove this line if you prefer inline source maps
            //    moduleFilenameTemplate: path.relative(outputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            //})
        ] : [
                // Plugins that apply in production builds only                
                //new OptimizeCSSPlugin({
                //    cssProcessorOptions: {
                //        safe: true
                //    }
                //}),
                new webpack.optimize.UglifyJsPlugin(),
                new ExtractTextPlugin('css/[name].css')
                
                //new PurifyCSSPlugin({
                //    paths: glob.sync(path.join(__dirname, 'App/*.html'))
                //})
            ])
    }];
};
