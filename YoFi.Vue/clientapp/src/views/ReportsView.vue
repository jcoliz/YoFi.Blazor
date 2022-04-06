<script setup>
import PageNavBar from "@/components/PageNavBar.vue";
</script>

<template>
  <div data-test-id="ReportsView">
    <PageNavBar title="Reports" />
  </div>
</template>

<script>
export default {
  name: "ReportsView",
  pageTitle: "Reports",
  components: {
    PageNavBar
  },
  data() {
    return {
      loading: false,
      hasdata: false,
      results: {}
    };
  },
  async beforeCreate() {
    document.title = "Reports - YoFi";
  },
  async created() {
    this.getReports();
  },
  methods: {
    async getReports() {
      this.loading = true;
      this.hasdata = false;

      try {
        let parameters = {
          slug: "summary",
          month: 1,
          year: 2021,
        };
        let url = "/wireapi/Reports/Summary";
        const response = await fetch(url, {
          method: "POST",
          cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
          credentials: "same-origin", // include, *same-origin, omit
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify(parameters)
        });
        const data = await response.json();
        this.results = data;
        this.hasdata = true;
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style></style>
