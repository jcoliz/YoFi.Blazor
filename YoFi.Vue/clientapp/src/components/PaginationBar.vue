<script setup>
import PageItemLink from "@/components/PageItemLink.vue";
</script>

<template>
  <div data-test-id="PaginationBar" class="mt-2 row">
    <div class="col-sm-7">
      <p class="fs-6">
        Displaying
        <span data-test-id="firstitem">{{ firstItem }}</span>
        through
        <span data-test-id="lastitem">{{ firstItem + numItems - 1 }}</span>
        of
        <span data-test-id="totalitems">{{ totalItems }}</span
        >.
      </p>
    </div>
    <nav class="col-sm-5" aria-label="Pagination control">
      <ul class="pagination justify-content-end">
        <PageItemLink
          :page="1"
          v-if="page > 2 && totalPages > 3"
          @new-page="lastUpdate"
          >&laquo;</PageItemLink
        >
        <PageItemLink
          :page="page - 2"
          v-if="page == totalPages && totalPages > 2"
          @new-page="lastUpdate"
        />
        <PageItemLink :page="page - 1" v-if="page > 1" @new-page="lastUpdate" />
        <li class="page-item active" aria-current="page">
          <span v-if="this.loading" class="page-link">
            <i class="fas fa-hourglass-half"></i>
          </span>
          <span v-else class="page-link">{{ page }}</span>
        </li>
        <PageItemLink
          :page="page + 1"
          v-if="page < totalPages"
          @new-page="lastUpdate"
        />
        <PageItemLink
          :page="page + 2"
          v-if="page == 1 && totalPages > 2"
          @new-page="lastUpdate"
        />
        <PageItemLink
          :page="totalPages"
          label="Last Page"
          v-if="page + 1 < totalPages && totalPages > 3"
          @new-page="lastUpdate"
          >&raquo;</PageItemLink
        >
      </ul>
    </nav>
  </div>
</template>

<script>
export default {
  name: "PaginationBar",
  props: {
    page: Number,
    pageSize: Number,
    firstItem: Number,
    numItems: Number,
    totalPages: Number,
    totalItems: Number,
    loading: Boolean
  },
  emits: {
    newPage: (p) => {
      return p > 0;
    }
  },
  methods: {
    lastUpdate(p) {
      console.info("PaginationBar: " + p);
      this.$emit("newPage", p);
    }
  },
  components: {
    PageItemLink
  }
};
</script>

<style></style>
