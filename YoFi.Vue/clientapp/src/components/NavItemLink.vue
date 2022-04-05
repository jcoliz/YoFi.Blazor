<template>
  <li class="nav-item">
    <router-link class="nav-link" :to="link">{{ title }}</router-link>
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
