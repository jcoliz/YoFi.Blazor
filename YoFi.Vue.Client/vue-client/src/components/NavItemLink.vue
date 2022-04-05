<template>
    <li class="nav-item">
        <a class="nav-link" href="">Hello, {{title}}</a>
    </li>
</template>

<script>
export default {
  props: {
    title: String,
    link: String
  }
}
</script>

<style>

</style>

