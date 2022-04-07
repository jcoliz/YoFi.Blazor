<template>
  <nav
    data-test-id="NavBar"
    class="navbar navbar-expand-md navbar-dark bg-dark mb-4"
  >
    <div data-test-id="contents" class="container-fluid">
      <a class="navbar-brand" href="">YoFi</a>
      <button
        aria-label="Toggle navigation"
        class="navbar-toggler"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#navbarMembers"
        aria-controls="#navbarMembers"
        aria-expanded="false"
      >
        <span class="navbar-toggler-icon"></span>
      </button>
      <div id="navbarMembers" class="collapse navbar-collapse me-auto">
        <ul class="navbar-nav">
          <slot />
        </ul>
      </div>
    </div>
  </nav>
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
