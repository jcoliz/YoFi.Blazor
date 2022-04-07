<template>
  <div data-test-id="ReportPicker" class="nav-item dropdown me-auto">
    <a
      id="dropdownMenuReportId"
      class="nav-link dropdown-toggle"
      data-bs-toggle="dropdown"
      role="button"
      href="#"
      aria-haspopup="true"
      aria-expanded="false"
      >Choose a Report</a
    >
    <div class="dropdown-menu" aria-labelledby="dropdownMenuReportId">
      <router-link class="dropdown-item" to="/reports/">Summary</router-link>
      <template v-if="this.definitions">
        <router-link
          :to="`/report/${report.definition}-detail`"
          :data-test-id="`${report.definition}-detail`"
          class="ms-auto btn btn-outline-secondary"
          ><i class="fas fa-angle-double-right"></i
        ></router-link>
        <router-link
          v-for="item in this.definitions"
          :key="item.slug"
          :to="`/report/${item.slug}`"
          :data-test-id="`link-${report.definition}`"
          class="dropdown-item"
          >{{ item.name }}</router-link
        >
      </template>
    </div>
  </div>
</template>

<script>
export default {
  name: "ReportPicker",
  props: {
    definitions: Array
  }
};
</script>

<style></style>
