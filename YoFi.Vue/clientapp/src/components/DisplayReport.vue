<template>
  <div data-test-id="DisplayReport" class="table">
    <table :data-test-id="`report-${this.definition}`">
      <thead>
        <tr class="report-header">
          <th>{{ this.name }}</th>
          <th
            v-for="(column, index) in this.columnLabels"
            :key="`column-${index}`"
          >
            {{ column.isTotal ? "$ TOTAL" : column.name }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(row, index) in this.rowLabels"
          :key="`row-${index}`"
          :class="row.isTotal ? 'report-row-total' : `report-row-${row.level}`"
        >
          <td>
            {{ this.lines[index].name }}
          </td>
          <td
            v-for="(col, colindex) in this.columnLabels"
            :key="`cell-${index}-${colindex}`"
            :class="`report-col-amount ${
              col.isTotal ? 'report-col-total' : ''
            }`"
          >
            <span v-if="col.displayAsPercent"
              >{{
                (this.lines[index].values[colindex] * 100).toFixed(0)
              }}%</span
            >
            <span v-else>{{
              this.currencyFormatter.format(this.lines[index].values[colindex])
            }}</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: "DisplayReport",
  props: {
    name: String,
    description: String,
    definition: String,
    columnLabels: Array,
    rowLabels: Array,
    lines: Array
  },
  computed: {
    currencyFormatter() {
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
        maximumFractionDigits: 0 // (causes 2500.99 to be printed as $2,501)
      });
    }
  }
};
</script>

<style scoped>
tr.report-row-3 {
  font-weight: bold;
  background-color: #8985b3;
  border-top-style: solid;
  border-bottom-style: solid;
}

tr.report-row-2 {
  font-weight: bold;
  background-color: var(--color-2);
}

tr.report-row-1 {
  font-weight: bold;
}

tr.report-row-0 td.report-col-name {
  text-indent: 20px;
}

tr.report-row-total {
  font-weight: bold;
}

td.report-col-amount {
  text-align: right;
}

th.report-col-amount {
  text-align: right;
}

td.report-col-total {
  font-weight: bold;
}

td.report-col-name {
  word-break: break-word;
}
</style>
