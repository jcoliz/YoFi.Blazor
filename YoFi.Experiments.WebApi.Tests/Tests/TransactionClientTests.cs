﻿using jcoliz.FakeObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
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

        [TestCleanup]
        public void Cleanup()
        {
            // Reset the clock back
            //integrationcontext.clock.Reset();

            // Clean out database
            context.Set<Core.Models.Transaction>().RemoveRange(context.Set<Core.Models.Transaction>());
            context.Set<Core.Models.Payee>().RemoveRange(context.Set<Core.Models.Payee>());
            context.Set<Core.Models.Receipt>().RemoveRange(context.Set<Core.Models.Receipt>());
            context.SaveChanges();
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

        [TestMethod]
        public async Task IndexSingle()
        {
            // Given: One item in database
            var items = FakeObjects<Core.Models.Transaction>.Make(1).SaveTo(this);

            // When: Getting "/"
            var response = await webapiclient.TransactionsAsync(null, null, null, null, null);

            // And: Expected items returned
            Assert.AreEqual(1, response.Items.Count);
            Assert.AreEqual(1, response.PageInfo.TotalItems);
            Assert.AreEqual(items.Single().Memo, response.Items.First().Memo);
        }

        [TestMethod]
        public async Task IndexPage1()
        {
            // Given: A long set of items, which is longer than one page, but not as long as two pages 
            var pagesize = 25; 
            var items = FakeObjects<Core.Models.Transaction>.Make(pagesize).Add(pagesize / 2).SaveTo(this);

            // When: Getting "/"
            var response = await webapiclient.TransactionsAsync(null, null, null, null, null);

            // Then: Only first page of items returned
            Assert.AreEqual(items.Group(0).Count, response.Items.Count);
            Assert.AreEqual(items.Count, response.PageInfo.TotalItems);
            Assert.IsTrue(items.Group(0).Select(x=>x.Memo).SequenceEqual(response.Items.Select(x=>x.Memo)));
        }

        [TestMethod]
        public async Task IndexPage2()
        {
            // Given: A long set of items, which is longer than one page, but not as long as two pages 
            var pagesize = 25;
            var items = FakeObjects<Core.Models.Transaction>.Make(pagesize).Add(pagesize / 2).SaveTo(this);

            // When: Getting the Index for page 2
            var response = await webapiclient.TransactionsAsync(null, 2, null, null, null);

            // Then: Only 2nd page items returned
            Assert.AreEqual(items.Group(1).Count, response.Items.Count);
            Assert.AreEqual(items.Count, response.PageInfo.TotalItems);
            Assert.IsTrue(items.Group(1).Select(x => x.Memo).SequenceEqual(response.Items.Select(x => x.Memo)));
        }

        [TestMethod]
        public async Task IndexSearch()
        {
            // Given: There are 5 items in the database, one of which we care about
            var chosen = FakeObjects<Core.Models.Transaction>.Make(5).SaveTo(this).Take(1);

            // When: Searching the index for the focused item's testkey
            var q = chosen.Single().Memo;
            var response = await webapiclient.TransactionsAsync(q, null, null, null, null);

            // Then: The expected items are returned
            Assert.AreEqual(1, response.Items.Count);
            Assert.AreEqual(chosen.Single().Payee, response.Items.Single().Payee);
        }

        #endregion
    }
}
