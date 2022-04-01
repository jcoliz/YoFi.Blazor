using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YoFi.AspNet.Data;
using YoFi.Core.Reports;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.Experiments.WebApi.Tests
{
    [TestClass]
    public class ReportApiTests
    {
        #region Fields

        protected static IntegrationContext integrationcontext;
        protected static HttpClient client => integrationcontext.client;
        protected static ApplicationDbContext context => integrationcontext.context;
        protected static SampleDataStore data;

        protected string urlroot => "/Reports";

        #endregion

        #region Properties

        public TestContext TestContext { get; set; }

        #endregion

        #region Helpers
        protected async Task<JsonDocument> WhenGetAsync(string url, HttpStatusCode expectedresult = HttpStatusCode.OK)
        {
            // When: Getting {url}
            var response = await client.GetAsync(url);

            // Then: Result as expected
            Assert.AreEqual(expectedresult, response.StatusCode);

            // And: Response is valid JSON, if valid
            JsonDocument document = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

            return document;
        }
        protected async Task<HttpResponseMessage> WhenSendAsync<T>(string url, T item, HttpMethod method = null)
        {
            var request = new HttpRequestMessage(method ?? HttpMethod.Post, url);
            request.Content = new StringContent(JsonSerializer.Serialize<T>(item), Encoding.UTF8, "application/json");
            var outresponse = await client.SendAsync(request);

            return outresponse;
        }

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
            var response = await WhenSendAsync(urlroot, new ReportParameters() { id = name, year = 2021 });
            response.EnsureSuccessStatusCode();

            // Then: Expected report is returned
            var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var actual = JsonSerializer.Deserialize<WireReport>(document, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            Assert.AreEqual(name, actual.Definition);
            Assert.AreEqual(19908.15m, actual.GrandTotal);
        }

        [TestMethod]
        public async Task GetSummary()
        {
            // When: Requesting summary report
            var response = await WhenSendAsync($"{urlroot}/Summary", new ReportParameters() );
            response.EnsureSuccessStatusCode();

            // Then: Expected summary reports are returned
        }

        #endregion
    }
}
