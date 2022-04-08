<template>
  <div data-test-id="TransactionEditDialog">
    <div class="form-group">
      <label class="control-label">Payee</label>
      <input :value="this.payee" class="form-control" />
    </div>
    <div class="form-group">
      <label class="control-label">Category</label>
      <input :value="this.category" class="form-control" />
    </div>
    <div class="form-group">
      <label class="control-label">Memo</label>
      <input :value="this.memo" class="form-control" />
    </div>
  </div>
</template>

<script>
export default {
  name: "TransactionEditDialog",
  props: {
    id: Number,
    payee: String,
    category: String,
    memo: String
  }
};
</script>

<style></style>
