<script setup>
import PageNavBar from "@/components/PageNavBar.vue";
import DisplayReport from "@/components/DisplayReport.vue";
import ReportPicker from "@/components/ReportPicker.vue";
</script>

<template>
  <div data-test-id="ReportSingleView">
    <PageNavBar title="Reports">
      <ReportPicker :definitions="this.definitions" />
    </PageNavBar>
    <template v-if="this.hasdata">
      <h2 class="px-1">{{ this.report.name }}</h2>
      <h4 v-if="this.report.description" class="px-1">{{ this.report.description }}</h4>
      <DisplayReport v-bind="this.report" class="mt-4" />
    </template>
  </div>
</template>

<script>
export default {
  name: "ReportSingleView",
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
      slug: "",
      definitions: {},
      report: {}
    };
  },
  async beforeCreate() {
    document.title = "Reports - YoFi";
  },
  async created() {
    this.slug = this.$route.params.id;
    this.getReport();
  },
  methods: {
    async getReport() {
      this.loading = true;

      try {
        let parameters = {
          slug: this.slug,
          month: 1,
          year: 2021
        };
        let url = "/wireapi/Reports/";
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
        this.report = data;
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
