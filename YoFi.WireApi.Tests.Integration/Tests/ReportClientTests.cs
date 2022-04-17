using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoFi.Data.SampleData;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.WireApi.Tests.Integration
{
    [TestClass]
    public class ReportClientTests: IntegrationTests
    {
        #region Fields

        WireApi.Client.WireApiClient wireapi;
        private const int sampledatayear = 2022;

        #endregion

        #region Init/Cleanup

        [ClassInitialize]
        public static async Task InitialSetup(TestContext tcontext)
        {
            integrationcontext = new IntegrationContext(tcontext.FullyQualifiedTestClassName);

            await SampleDataStore.LoadFullAsync();
            var data = SampleDataStore.Single;

            context.Transactions.AddRange(data.Transactions);
            context.BudgetTxs.AddRange(data.BudgetTxs);
            context.SaveChanges();
        }

        [ClassCleanup]
        public static void FinalCleanup()
        {
            integrationcontext.Dispose();
        }

        [TestInitialize]
        public void SetUp()
        {
            wireapi = new Client.WireApiClient("/", integrationcontext.client);
        }

        #endregion

        #region Tests

        [TestMethod]
        public async Task ListDefinitions()
        {
            // When: Requesting report definitions
            var result = await wireapi.ListReportsAsync();

            // Then: Report definitions are returned
            Assert.IsTrue(result.Count > 10);
        }

        [TestMethod]
        public async Task GetOneReport()
        {
            // When: Requesting report {name}
            var name = "all";
            var result = await wireapi.BuildReportAsync(new Client.ReportParameters() { Slug = name, Year = sampledatayear });

            // Then: Expected report is returned
            Assert.AreEqual(name, result.Definition);
            Assert.AreEqual(18389.23m, (decimal)result.GrandTotal);
        }

        [TestMethod]
        public async Task GetAllReports()
        {
            // Given: The list of all report definitions
            var definitions = await wireapi.ListReportsAsync();
            var slugs = definitions.Select(x => x.Slug);

            foreach (var slug in slugs)
            {
                // When: Requesting each report {name}
                var result = await wireapi.BuildReportAsync(new Client.ReportParameters() { Slug = slug, Year = sampledatayear });

                // Then: Expected report is returned
                Assert.AreEqual(slug, result.Definition);
            }
        }

        [TestMethod]
        public async Task GetReportNotFound()
        {
            try
            {
                // When: Requesting report {name} where it won't be found
                var name = "bogus";
                var result = await wireapi.BuildReportAsync(new Client.ReportParameters() { Slug = name, Year = sampledatayear });
            }
            catch (Client.ApiException<Client.ProblemDetails> ex)
            {
                // Then: Not Found
                Assert.AreEqual(StatusCodes.Status404NotFound, ex.StatusCode);
            }
            catch
            {
                throw new Exception("Unexpected exception type");
            }
        }

        [TestMethod]
        public async Task GetSummary()
        {
            // When: Requesting summary report
            var result = await wireapi.BuildSummaryReportAsync(new Client.ReportParameters() { Year = sampledatayear });

            // Then: Expected summary reports are returned

            // Two report groups
            Assert.AreEqual(2, result.Count);

            // The reports each group
            Assert.AreEqual(3, result.First().Count);
            Assert.AreEqual(3, result.Last().Count);

            // Totals as expected (Note that OpenAPI has no fixed point (decimal) data type)
            Assert.AreEqual(30309.36m, (decimal)result.First().Last().GrandTotal);
            Assert.AreEqual(10972.65m, (decimal)result.Last().Last().GrandTotal);
        }

        #endregion
    }
}
