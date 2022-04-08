<script setup>
import HelloWorld from "@/components/HelloWorld.vue";
import PageNavBar from "@/components/PageNavBar.vue";
import PageActions from "@/components/PageActions.vue";
import DialogModal from "@/components/DialogModal.vue";
</script>

<template>
  <div data-test-id="AboutView">
    <PageNavBar title="About">
      <PageActions />
    </PageNavBar>
    <div class="home">
      <p><img alt="Vue logo" src="../assets/logo.png" /></p>
      <h2 v-if="$route.params.id">{{ $route.params.id }}</h2>

      <!-- Button trigger modal -->
      <button
        data-bs-target="#exampleModal"
        data-bs-toggle="modal"
        type="button"
        class="btn btn-primary mb-4"
      >
        Launch demo modal
      </button>

      <HelloWorld msg="Welcome to Your Vue.js App" />
      <DialogModal id="exampleModal" title="About" class="hide">
        <p>This is an interesting app!</p>
      </DialogModal>

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
    PageActions,
    DialogModal
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
