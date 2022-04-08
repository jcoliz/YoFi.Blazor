<template>
  <div data-test-id="DialogModal" class="modal modal-fade" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <slot name="header">
            <h5 class="modal-title">{{this.$props.title}}</h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </slot>
        </div>
        <div class="modal-body">
          <slot></slot>
        </div>
        <div class="modal-footer">
          <slot name="footer">
            <button
              type="button"
              class="btn btn-outline-secondary"
              data-bs-dismiss="modal"
            >
              Cancel
            </button>
            <button type="button" class="btn btn-primary">Save</button>
          </slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "DialogModal",
  props: {
    title: String,
  },
  emits: {
    newPage: (p) => {
      return p > 0;
    }
  }
};
</script>

<style></style>
