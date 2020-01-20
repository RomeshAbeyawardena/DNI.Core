const path = require('path');

module.exports = {
    mode: "development",
    output: {
        path: path.resolve(__dirname, "../wwwroot/content"),
        filename: "app.js"
    },
    module: {
        rules: [
          { test: /\.scss/, use: ['style-loader','css-loader','sass-loader'] }
        ]
    }
}