<script setup>
import PageNavBar from "@/components/PageNavBar.vue";
import DisplayReport from "@/components/DisplayReport.vue";
</script>

<template>
  <div data-test-id="ReportsView">
    <PageNavBar title="Reports" />
    <h2>Summary</h2>
    <div data-test-id="reports-wrapper" v-if="this.hasdata" class="row">
      <div
        v-for="(group, index) in this.results"
        :key="`group-${index}`"
        :data-test-id="`report-group-${index}`"
        class="col-lg-6 p-4"
      >
        <div
          v-for="(report, rindex) in group"
          :key="`report-${index}-${rindex}`"
          :data-test-id="`report-${index}-${rindex}`"
        >
          <h3>{{ report.name }}</h3>
          <DisplayReport v-bind="report" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ReportsView",
  pageTitle: "Reports",
  components: {
    PageNavBar,
    DisplayReport
  },
  data() {
    return {
      loading: false,
      hasdata: false,
      results: []
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
          year: 2021
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
