<script setup>
import NavItemLink from '@/components/NavItemLink.vue'
</script>

<template>
<div class="mb-4">
    <nav class="navbar navbar-expand-md navbar-dark bg-dark">
        <div class="container-fluid">
            <a class="navbar-brand" href="">Navbar</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarTogglerDemo02" aria-controls="navbarTogglerDemo02" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse me-auto" id="navbarTogglerDemo02">
                <ul class="navbar-nav">
                    <NavItemLink title="Home" link="/" />
                    <NavItemLink title="About" link="/about" />
                    <NavItemLink title="Reports" link="/reports" />
                </ul>
            </div>
        </div>
    </nav>
</div>
</template>

<style scoped>
nav {
  width: 100%;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

</style>