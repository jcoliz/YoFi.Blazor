using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.WireApi.Tests.Tests
{
    [TestClass]
    public class ReportClientTests: IntegrationTests
    {
        #region Fields

        WireApi.Client.WireApiClient wireapi;
        private const int sampledatayear = 2021;

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
            wireapi = new WireApi.Client.WireApiClient("/", integrationcontext.client);
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
        #endregion

    }
}
