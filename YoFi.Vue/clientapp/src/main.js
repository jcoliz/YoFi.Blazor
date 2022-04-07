import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap";
import "fontawesome-free/css/all.min.css";
import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";

createApp(App).use(router).mount("#app");
