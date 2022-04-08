<template>
  <div
    data-test-id="PageActions"
    title="Actions"
    class="nav-item dropdown"
    data-tooltip="tooltip"
    data-bs-placement="bottom"
  >
    <button
      id="dropdownMenuButtonAction"
      class="btn btn-secondary dropdown-toggle dropdown-noarrow"
      data-bs-toggle="dropdown"
      aria-haspopup="true"
      aria-expanded="false"
    >
      <i class="fas fa-ellipsis-h"></i>
    </button>
    <div class="dropdown-menu dropdown-menu-end" aria-labelledby="#dropdownMenuButtonAction">
      <slot />
      <a href="#" data-bs-toggle="modal" data-bs-target="#helpModal" class="btn"
        >Help Topic</a
      >
    </div>
  </div>
</template>

<script>
export default {
  name: "PageActions"
};
</script>

<style>
.dropdown-noarrow::after {
  content: none;
}

.dropdown-noarrow::before {
  content: none;
}

.dropstart .dropdown-noarrow::before {
  content: none;
}
</style>
