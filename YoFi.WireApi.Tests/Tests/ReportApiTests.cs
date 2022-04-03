using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using YoFi.Core.Reports;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.WireApi.Tests
{
    [TestClass]
    public class ReportApiTests: BaseApiTests
    {
        #region Fields

        private const int sampledatayear = 2021;

        #endregion

        #region Properties

        protected override string urlroot { get; set; } = "wireapi/Reports";

        #endregion

        #region Init/Cleanup

        [ClassInitialize]
        public static async Task InitialSetup(TestContext tcontext)
        {
            integrationcontext = new IntegrationContext(tcontext.FullyQualifiedTestClassName);

            await SampleDataStore.LoadFullAsync();
            data = SampleDataStore.Single;

            context.Transactions.AddRange(data.Transactions);
            context.BudgetTxs.AddRange(data.BudgetTxs);
            context.SaveChanges();
        }

        [ClassCleanup]
        public static void FinalCleanup()
        {
            integrationcontext.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public async Task ListDefinitions()
        {
            // When: Requesting report definitions
            var document = await WhenGetAsync(urlroot);

            // Then: Report definitions are returned
            var actual = JsonSerializer.Deserialize<List<ReportDefinition>>(document, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            Assert.IsTrue(actual.Count > 10);
        }

        [TestMethod]
        public async Task GetOneReport()
        {
            // When: Requesting report {name}
            var name = "all";
            var response = await WhenSendAsync(urlroot, new ReportParameters() { slug = name, year = sampledatayear });
            response.EnsureSuccessStatusCode();

            // Then: Expected report is returned
            var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var actual = JsonSerializer.Deserialize<WireReport>(document, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            Assert.AreEqual(name, actual.Definition);
            Assert.AreEqual(19908.15m, actual.GrandTotal);
        }

        [TestMethod]
        public async Task GetAllReports()
        {
            // Given: The list of all report definitions
            var document = await WhenGetAsync(urlroot);
            var definitions = JsonSerializer.Deserialize<List<ReportDefinition>>(document, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var slugs = definitions.Select(x => x.slug);

            foreach(var slug in slugs)
            {
                // When: Requesting each report {name}
                var response = await WhenSendAsync(urlroot, new ReportParameters() { slug = slug, year = sampledatayear });

                // Then: OK
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, slug);

                // And: Expected report is returned
                document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                var actual = JsonSerializer.Deserialize<WireReport>(document, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                Assert.AreEqual(slug, actual.Definition);
            }
        }

        [TestMethod]
        public async Task GetReportNotFound()
        {
            // When: Requesting report {name} where it won't be found
            var name = "bogus";
            var response = await WhenSendAsync(urlroot, new ReportParameters() { slug = name, year = sampledatayear });

            // Then: Not Found
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetSummary()
        {
            // When: Requesting summary report
            var response = await WhenSendAsync($"{urlroot}/Summary", new ReportParameters() { year = sampledatayear } );
            response.EnsureSuccessStatusCode();

            // Then: Expected summary reports are returned
            var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var actual = JsonSerializer.Deserialize<List<List<WireReport>>>(document, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            
            // Two report groups
            Assert.AreEqual(2, actual.Count);

            // The reports each group
            Assert.AreEqual(3, actual[0].Count);
            Assert.AreEqual(3, actual[1].Count);

            // Totals as expected
            Assert.AreEqual(31509.36m, actual[0].Last().GrandTotal);
            Assert.AreEqual(5629.74m, actual[1].Last().GrandTotal);
        }

        #endregion
    }
}
