const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
  transpileDependencies: true,
    devServer: {
      proxy: {
        '^/weatherforecast': {
          target: 'http://localhost:5003',
          ws: true,
          changeOrigin: true
        }
      }
    }
  });
