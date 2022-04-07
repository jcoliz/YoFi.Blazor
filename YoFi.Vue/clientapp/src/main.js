import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap";
import "fontawesome-free/css/all.min.css";

createApp(App).use(router).mount("#app");
