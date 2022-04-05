<script setup>
import PageNavBar from '@/components/PageNavBar.vue';
import wireapiclient from '@/WireApiClient.js';
</script>

<template>
  <PageNavBar title="Transactions"/>
</template>

<script>
export default {
    data() {
      return {
        loading: false,
        wqr: {}
      };
    },
    async created() {
      this.getList()
    },    
    methods: {
      async getList() {
        this.loading = true

        try {
          this.wqr = await wireapiclient.listTransactions()
        } finally {
          this.loading = false
        }
      }
    }
}
</script>

<style>

</style>