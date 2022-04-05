<template>
  <li class="nav-item">
    <reouter-link class="nav-link" :to="link">{{ title }}</reouter-link>
  </li>
</template>

<script>
export default {
  props: {
    title: String,
    link: String,
  },
};
</script>

<style></style>
