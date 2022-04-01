﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.Experiments.WebApi.Tests.Tests
{
    [TestClass]
    public class TransactionClientTests: IntegrationTests
    {
        #region Fields

        Client.WebApiClient webapiclient;

        #endregion

        #region Init/Cleanup

        [ClassInitialize]
        public static void InitialSetup(TestContext tcontext)
        {
            integrationcontext = new IntegrationContext(tcontext.FullyQualifiedTestClassName);
        }

        [ClassCleanup]
        public static void FinalCleanup()
        {
            integrationcontext.Dispose();
        }

        [TestInitialize]
        public void SetUp()
        {
            webapiclient = new Client.WebApiClient("/", integrationcontext.client);
        }

        #endregion

        #region Tests

        [TestMethod]
        public async Task IndexEmpty()
        {
            // Given: No data in database

            // When: Getting "/"
            var response = await webapiclient.TransactionsAsync(null,null,null,null,null);

            // Then: No items returned
            Assert.AreEqual(0, response.Items.Count);
            Assert.AreEqual(0, response.PageInfo.TotalItems);
        }
        #endregion
    }
}
