<script setup>
import NavItemLink from '@/components/NavItemLink.vue'
</script>

<template>
<div class="mb-4">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
        <div class="container">
            <a class="navbar-brand" href="">Navbar</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarTogglerDemo02" aria-controls="navbarTogglerDemo02" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarTogglerDemo02">
                <ul class="navbar-nav ms-auto" v-for="n in 3" :key="n">
                    <NavItemLink :title="n" />
                </ul>
            </div>
        </div>
    </nav>
</div>
</template>