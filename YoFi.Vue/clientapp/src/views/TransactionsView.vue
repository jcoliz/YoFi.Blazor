<script setup>
import PageNavBar from "@/components/PageNavBar.vue";
</script>

<template>
  <div>
    <PageNavBar title="Transactions" />
    <p v-if="this.loading"><em>Loading...</em></p>
    <table className="table table-striped">
      <thead>
        <tr>
          <th>Date</th>
          <th>Temp. (C)</th>
          <th>Temp. (F)</th>
          <th>Summary</th>
        </tr>
      </thead>
      <tbody v-for="forecast in results" :key="forecast.date">
        <tr>
          <td>{{ forecast.date }}</td>
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
