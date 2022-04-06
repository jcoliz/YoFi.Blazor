<template>
  <nav
    data-test-id="PageNavBar"
    class="navbar navbar-light navbar-page mb-2 px-2"
  >
    <div class="container-fluid">
      <span class="navbar-brand h1 mt-1" data-test-id="title">{{ title }}</span>
    </div>
  </nav>
</template>

<script>
export default {
  name: "PageNavBar",
  props: {
    title: String,
  },
};
</script>

<style scoped>
.navbar-page {
  background-color: var(--color-2);
}
</style>
