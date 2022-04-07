import { createRouter, createWebHistory } from "vue-router";
import AboutView from "../views/AboutView.vue";

const routes = [
  {
    path: "/about",
    name: "about",
    component: AboutView,
  },
  {
    path: "/reports",
    name: "reports",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/ReportsView.vue"),
  },
  {
    path: "/report/:id",
    name: "reportid",
    component: AboutView,
  },
  {
    path: "/",
    name: "txs",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/TransactionsView.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
