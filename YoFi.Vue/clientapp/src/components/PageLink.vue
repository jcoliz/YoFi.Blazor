<template>
  <li class="page-item">
    <button
      class="page-link"
      @click="this.$emit('newPage', this.page)"
      :aria-label="ariaLabel"
    >
      <slot>{{ this.page }}</slot>
    </button>
  </li>
</template>

<script>
export default {
  name: "PageLink",
  props: {
    page: Number,
    label: String
  },
  computed: {
    ariaLabel() {
      return this.label ? this.label : "Page " + this.page;
    }
  },
  emits: {
    newPage: (p) => {
      return p > 0;
    }
  }
};
</script>

<style>
button.page-link {
  outline: 0;
  box-shadow: none;
}
</style>
