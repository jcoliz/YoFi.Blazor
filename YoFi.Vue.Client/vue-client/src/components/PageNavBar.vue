<template>
    <nav class="navbar navbar-light navbar-page mb-2">
        <div class="container-fluid">
            <span class="navbar-brand h1 mt-1">{{title}}</span>
        </div>
    </nav>  
</template>

<script>
export default {
  props: {
    title: String
  }
}
</script>

<style scoped>
.navbar-page {
    background-color: var(--color-2);
}
</style>