<template>
  <div
    data-test-id="LoadingSpinner"
    class="d-flex justify-content-center spinner-container"
  >
    <div class="spinner-border my-5" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
</template>

<script>
export default {
  name: "LoadingSpinner"
};
</script>

<style>
@keyframes delayVisibility {
  0% {
    opacity: 0;
  }

  50% {
    opacity: 0;
  }

  100% {
    opacity: 1;
  }
}

.spinner-container {
  animation: delayVisibility linear 1s;
}
</style>
