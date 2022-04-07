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
        <PageLink
          :page="1"
          v-if="page > 2 && totalPages > 3"
          @new-page="lastUpdate"
          >&laquo;</PageLink
        >
        <PageLink
          :page="page - 2"
          v-if="page == totalPages && totalPages > 2"
          @new-page="lastUpdate"
        />
        <PageLink :page="page - 1" v-if="page > 1" @new-page="lastUpdate" />
        <li class="page-item active" aria-current="page">
          <span v-if="this.loading" class="page-link">
            <i class="fas fa-hourglass-half"></i>
          </span>
          <span v-else class="page-link">{{ page }}</span>
        </li>
        <PageLink
          :page="page + 1"
          v-if="page < totalPages"
          @new-page="lastUpdate"
        />
        <PageLink
          :page="page + 2"
          v-if="page == 1 && totalPages > 2"
          @new-page="lastUpdate"
        />
        <PageLink
          :page="totalPages"
          label="Last Page"
          v-if="page + 1 < totalPages && totalPages > 3"
          @new-page="lastUpdate"
          >&raquo;</PageLink
        >
      </ul>
    </nav>
  </div>
</template>

<script>
import PageLink from "@/components/PageLink.vue";
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
    PageLink
  }
};
</script>

<style></style>
