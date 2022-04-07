<template>
  <nav
    data-test-id="PageNavBar"
    class="navbar navbar-light navbar-page mb-2 px-2"
  >
    <div data-test-id="contents" class="container-fluid">
      <span data-test-id="title" class="navbar-brand h1 mt-1">{{ title }}</span>
      <slot />
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
