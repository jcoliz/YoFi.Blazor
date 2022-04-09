<template>
  <div
    data-test-id="DialogModal"
    v-if="isOpen"
    class="modal fade show"
    tabindex="-1"
    style="display: block"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <slot name="header">
            <h5 class="modal-title">{{ this.$props.title }}</h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
              @click="$emit('close')"
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
              @click="$emit('close')"
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
    visible: Boolean
  },
  data() {
    return {
      isOpen: this.visible
    };
  },
  emits: ["close"],
  watch: {
    visible: function (newVal, oldVal) {
      this.isOpen = newVal;
      console.log(`DialogModal: visible now ${newVal} was ${oldVal}`);
    }
  }
};
</script>

<style></style>
