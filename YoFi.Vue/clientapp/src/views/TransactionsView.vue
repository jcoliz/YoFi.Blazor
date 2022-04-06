<script setup>
import PageNavBar from "@/components/PageNavBar.vue";
import moment from "moment";
</script>

<template>
  <div data-test-id="TransactionsView">
    <PageNavBar title="Transactions" />
    <p v-if="this.loading"><em>Loading...</em></p>
    <table className="table table-striped" data-test-id="results">
      <thead>
        <tr>
          <th>Date</th>
          <th>Temp. (C)</th>
          <th>Temp. (F)</th>
          <th>Summary</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="forecast in results" :key="forecast.date">
          <td>{{ formatShortDate(forecast.date) }}</td>
          <td>{{ forecast.temperatureC }}</td>
          <td>{{ forecast.temperatureF }}</td>
          <td>{{ forecast.summary }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      loading: false,
      results: {},
    };
  },
  async created() {
    this.getList();
  },
  computed: {
    formatShortDate() {
      return (d) => {
        if (d) {
          return moment(String(d)).format("M/DD");
        }
      };
    },
  },
  methods: {
    async getList() {
      this.loading = true;

      try {
        const response = await fetch("/weatherforecast");
        const data = await response.json();
        this.results = data;
      } finally {
        this.loading = false;
      }
    },
  },
};
</script>

<style></style>
