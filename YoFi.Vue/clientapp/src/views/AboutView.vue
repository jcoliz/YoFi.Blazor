<script setup>
import HelloWorld from "@/components/HelloWorld.vue";
import PageNavBar from "@/components/PageNavBar.vue";
import PageActions from "@/components/PageActions.vue";
</script>

<template>
  <div data-test-id="AboutView">
    <PageNavBar title="About">
      <PageActions />
    </PageNavBar>
    <div class="home">
      <img alt="Vue logo" src="../assets/logo.png" />
      <h2 v-if="$route.params.id">{{ $route.params.id }}</h2>
      <HelloWorld msg="Welcome to Your Vue.js App" />
    </div>
  </div>
</template>

<script>

export default {
  name: "AboutView",
  pageTitle: "About",
  components: {
    HelloWorld,
    PageNavBar,
    PageActions
  },
  async beforeCreate() {
    document.title = "About - YoFi";
  }
};
</script>

<style scoped>
div.home {
  text-align: center;
}
</style>
