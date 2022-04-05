<template>
    <li class="nav-item">
        <RouterLink class="nav-link" :to="link">{{title}}</RouterLink>
    </li>
</template>

<script>
import { RouterLink } from 'vue-router'
export default {
  props: {
    title: String,
    link: String
  }
}
</script>

<style>

</style>

