<template>
  <div data-test-id="RowActions" class="dropdown">
    <button
      href="#"
      class="btn btn-sm btn-outline-secondary my-1 dropdown-toggle dropdown-noarrow"
      id="actions-@line"
      data-bs-toggle="dropdown"
      aria-haspopup="true"
      aria-expanded="false"
    >
      <i class="fas fa-ellipsis-h"></i>
    </button>
    <div
      class="dropdown-menu dropdown-menu-end"
      aria-labelledby="actions-@line"
    >
      <button
        data-test-id="edit"
        class="btn dropdown-item"
        @click="this.$emit('actionClicked', this.item, 'edit')"
      >
        <i class="fa fa-edit"></i> Edit
      </button>
      <slot />
      <button
        data-test-id="delete"
        class="btn dropdown-item"
        @click="this.$emit('actionClicked', this.item, 'delete')"
      >
        <i class="fa fa-trash-alt"></i> Delete
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: "PageActions",
  props: {
    item: Number
  },
  emits: {
    actionClicked: (which, action) => {
      return which > 0 && action;
    }
  }
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
