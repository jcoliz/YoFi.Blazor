<script setup>
import PageNavBar from "@/components/PageNavBar.vue";
import PagePicker from "@/components/PagePicker.vue";
import PageActions from "@/components/PageActions.vue";
import RowActions from "@/components/RowActions.vue";
import DialogModal from "@/components/DialogModal.vue";
import LoadingSpinner from "@/components/LoadingSpinner.vue";
import moment from "moment";
</script>

<template>
  <div data-test-id="TransactionsView">
    <PageNavBar title="Transactions">
      <PageActions>
        <a
          href="#"
          data-bs-toggle="modal"
          data-bs-target="#createModal"
          class="btn"
          >Create New</a
        >
      </PageActions>
    </PageNavBar>
    <table
      data-test-id="results"
      className="table table-striped"
      v-if="this.hasdata"
    >
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
          <td class="col-right">
            <RowActions :item="item.id" @actionClicked="this.actionGo" />
          </td>
        </tr>
      </tbody>
    </table>
    <PagePicker
      v-if="this.hasdata"
      v-bind="this.results.pageInfo"
      @new-page="pageUpdate"
      :loading="this.loading"
    />
    <LoadingSpinner v-if="this.loading" />
    <DialogModal id="createModal" title="Create Transaction" />
    <DialogModal id="helpModal" title="Help Topic" />
  </div>
</template>

<script>
export default {
  name: "TransactionsView",
  pageTitle: "Transactions",
  components: {
    PageNavBar,
    PagePicker,
    PageActions,
    RowActions,
    DialogModal,
    LoadingSpinner
  },
  data() {
    return {
      loading: false,
      hasdata: false,
      results: {}
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
    }
  },
  methods: {
    async getList(p) {
      this.loading = true;

      try {
        let url = "/wireapi/Transactions";
        if (p > 1) url = url + "/?page=" + p;
        const response = await fetch(url);
        const data = await response.json();
        this.results = data;
        this.hasdata = true;
      } finally {
        this.loading = false;
      }
    },
    pageUpdate(p) {
      this.getList(p);
    },
    actionGo(i,action) {
      console.info("RowActions: " + i + " " + action);
    }
  }
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
