<template>
  <div data-test-id="DialogModal" class="modal fade" ref="rootelement">
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
            <button
              type="button"
              class="btn btn-primary"
              data-bs-dismiss="modal"
            >
              Save
            </button>
          </slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Modal } from "bootstrap";

export default {
  name: "DialogModal",
  props: {
    title: String,
    show: {
      default: false,
      required: false,
      type: Boolean
    }
  },
  data() {
    return {
      modalElement: null
    };
  },
  async mounted() {
    this.modalElement = new Modal(this.$refs.rootelement);

    this.modalElement._element.addEventListener("hide.bs.modal", () => {
      this.$emit("close");
    });

    if (this.show) {
      this.modalElement.show();
    }
  },
  watch: {
    show(show) {
      if (this.modalElement) {
        show ? this.modalElement.show() : this.modalElement.hide();
      }
    }
  },
  emits: ["close"]
};
</script>

<style></style>
