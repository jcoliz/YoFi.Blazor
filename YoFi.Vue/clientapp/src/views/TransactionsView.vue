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
          <th class="col-right">Date</th>
          <th class="col-left">Payee</th>
          <th class="col-right">Amount</th>
          <th class="col-left">Category</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in results.items" :key="item">
          <td class="col-right">{{ formatShortDate(item.timestamp) }}</td>
          <td class="col-left">{{ item.payee }}</td>
          <td class="col-right">{{ item.amount }}</td>
          <td class="col-left">{{ item.category }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: "TransactionsView",
  pageTitle: "Transactions",
  components: {
    PageNavBar,
  },
  data() {
    return {
      loading: false,
      results: {},
    };
  },
  async beforeCreate() {
    document.title = "Transactions - YoFi";
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
        const response = await fetch("/wireapi/Transactions");
        const data = await response.json();
        this.results = data;
      } finally {
        this.loading = false;
      }
    },
  },
};
</script>

<style scoped>
.col-right {
  text-align: right;
}

.col-left {
  text-align: left;
}
</style>
