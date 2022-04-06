<template>
  <div data-test-id="AboutView" class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <HelloWorld msg="Welcome to Your Vue.js App" />
  </div>
</template>

<script>
// @ is an alias to /src
import HelloWorld from "@/components/HelloWorld.vue";

export default {
  name: "AboutView",
  pageTitle: "About",
  components: {
    HelloWorld,
  },
  async beforeCreate() {
    document.title = "About - YoFi";
  },
};
</script>

<style scoped>
div.home {
  text-align: center;
}
</style>
