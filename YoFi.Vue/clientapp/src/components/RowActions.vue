<template>
  <div data-test-id="RowActions" class="dropdown">
      <button href="#" class="btn btn-sm btn-outline-secondary my-1 dropdown-toggle dropdown-noarrow" id="actions-@line" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
          <i class="fas fa-ellipsis-h"></i>
      </button>
      <div class="dropdown-menu dropdown-menu-end" aria-labelledby="actions-@line">
          <a asp-action="Edit" asp-route-id="@item.ID" data-test-id="edit" class="btn dropdown-item">
              <i class="fa fa-edit"></i> Edit
          </a>
          <slot />
          <a asp-action="Delete" asp-route-id="@item.ID" data-test-id="delete" class="btn dropdown-item">
              <i class="fa fa-trash-alt"></i> Delete
          </a>
      </div>
  </div>
</template>

<script>
export default {
  name: "PageActions"
};
</script>

<style scoped>
.dropdown-menu i {
    color: var(--color-2);
    width: 20px;
}

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
