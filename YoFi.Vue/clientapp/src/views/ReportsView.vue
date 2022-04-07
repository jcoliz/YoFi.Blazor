<script setup>
import PageNavBar from "@/components/PageNavBar.vue";
import DisplayReport from "@/components/DisplayReport.vue";
import ReportPicker from "@/components/ReportPicker.vue";
</script>

<template>
  <div data-test-id="ReportsView">
    <PageNavBar title="Reports">
      <ReportPicker :definitions="this.definitions" />
    </PageNavBar>
    <h2>Summary</h2>
    <div data-test-id="reports-loaded" v-if="this.hasdata" class="row">
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
          class="mb-4 row"
        >
          <DisplayReport v-bind="report">
            <div class="d-flex w-100">
              <router-link
                :to="`/report/${report.definition}-detail`"
                :data-test-id="`${report.definition}-detail`"
                class="ms-auto btn btn-outline-secondary"
                ><i class="fas fa-angle-double-right"></i
              ></router-link>
            </div>
          </DisplayReport>
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
    DisplayReport,
    ReportPicker
  },
  data() {
    return {
      loading: false,
      hasdata: false,
      definitions: {},
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

        url = "/wireapi/Reports";
        const response2 = await fetch(url);
        const data2 = await response2.json();
        this.definitions = data2;
      } finally {
        this.loading = false;
      }
    }
  }
};
</script>

<style></style>
