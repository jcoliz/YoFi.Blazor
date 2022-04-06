<script setup>
import PageNavBar from "@/components/PageNavBar.vue";
</script>

<template>
  <div data-test-id="ReportsView">
    <PageNavBar title="Reports" />
  </div>
</template>

<script>
export default {
  name: "ReportsView",
  pageTitle: "Reports",
  components: {
    PageNavBar,
  },
  async beforeCreate() {
    document.title = "Reports - YoFi";
  },
};
</script>

<style></style>
