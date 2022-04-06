<template>
  <div data-test-id="App">
    <NavBar>
      <NavItemLink title="Transactions" link="/" />
      <NavItemLink title="Reports" link="/reports" />
      <NavItemLink title="About" link="/about" />
    </NavBar>
    <div class="container">
      <router-view />
    </div>
  </div>
</template>

<script>
import NavItemLink from "@/components/NavItemLink.vue";
import NavBar from "@/components/NavBar.vue";

export default {
  name: "App",
  components: {
    NavItemLink,
    NavBar,
  },
};
</script>

<style lang="scss">
:root {
  --color-1: #290661; /* $indigo-800 */
  --color-1-darker: #140330; /* $indigo-900 */
  --color-1-lighter: #c29ffa; /* $indigo-200 */
  --color-2: #c29ffa; /* $indigo-200 */
  --color-3: #e0cffc; /* $indigo-100 */
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  a {
    &.router-link-exact-active {
      color: #42b983;
    }
  }
}
</style>
