<script setup>
import PageNavBar from "@/components/PageNavBar.vue";
import PagePicker from "@/components/PagePicker.vue";
import PageActions from "@/components/PageActions.vue";
import RowActions from "@/components/RowActions.vue";
import DialogModal from "@/components/DialogModal.vue";
import LoadingSpinner from "@/components/LoadingSpinner.vue";
import TransactionEditDialog from "@/components/TransactionEditDialog.vue";
import Modal from "bootstrap";
import moment from "moment";
</script>

<template>
  <div data-test-id="TransactionsView">
    <PageNavBar title="Transactions">
      <PageActions>
        <a @click="this.showCreateModal = true" class="btn">Create New</a>
      </PageActions>
    </PageNavBar>
    <div data-test-id="temp-tx-edit" v-if="this.focusItemIndex">
      <h2>Edit Transaction</h2>
      <TransactionEditDialog
        v-bind="this.results.items[this.focusItemIndex - 1]"
      />
      <button type="button" class="btn btn-outline-secondary me-2 my-2">
        Cancel
      </button>
      <button type="button" class="btn btn-primary my-2">Save</button>
    </div>
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
        <tr v-for="(item, rowindex) in results.items" :key="item">
          <td class="col-right">{{ formatShortDate(item.timestamp) }}</td>
          <td class="col-left">{{ item.payee }}</td>
          <td class="col-right">{{ item.amount }}</td>
          <td class="col-left">{{ item.category }}</td>
          <td class="col-right">
            <RowActions :item="rowindex" @actionClicked="this.actionGo" />
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
    <DialogModal
      id="createModal"
      title="Create Transaction"
      v-if="this.showCreateModal"
      :show="this.showCreateModal"
      @close="this.showCreateModal = false"
    />
    <DialogModal
      v-if="false"
      id="editModal"
      ref="editModal"
      title="Edit Transaction"
    >
      <TransactionEditDialog
        v-if="this.focusItemIndex"
        v-bind="this.results.items[this.focusItemIndex - 1]"
      />
    </DialogModal>
    <DialogModal v-if="false" id="helpModal" title="Help Topic" />
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
    LoadingSpinner,
    TransactionEditDialog
  },
  data() {
    return {
      loading: false,
      hasdata: false,
      focusItemIndex: 0,
      showCreateModal: false,
      results: {},
      modal: {}
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
    async actionGo(i, action) {
      console.info(`RowActions: ${i} ${action}`);
      let item = this.results.items[i];
      console.info(item);
      this.focusItemIndex = 1 + i;
      console.info(this.$refs);
      console.info(this.$refs.createModal);
      console.info(this.$refs.createModal.el);
      var modal = new Modal(this.$refs.createModal);
      modal.show();
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
