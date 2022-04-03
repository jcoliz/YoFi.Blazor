using AutoMapper;
using jcoliz.FakeObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.WireApi.Tests
{
    [TestClass]
    public class TransactionClientTests: IntegrationTests
    {
        #region Fields

        Client.WireApiClient wireapi;
        static IMapper mapper;

        #endregion

        #region Init/Cleanup

        [ClassInitialize]
        public static void InitialSetup(TestContext tcontext)
        {
            integrationcontext = new IntegrationContext(tcontext.FullyQualifiedTestClassName);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Core.Models.Transaction, Client.Transaction>();
                cfg.CreateMap<Client.Transaction, Core.Models.Transaction>();
                cfg.CreateMap<DateTimeOffset, DateTime>().ConstructUsing(x => x.LocalDateTime);
            });
            mapper = config.CreateMapper();
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
            var response = await wireapi.ListTransactionsAsync(null,null,null,null,null);

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
            var response = await wireapi.ListTransactionsAsync(null, null, null, null, null);

            // And: Expected items returned
            Assert.AreEqual(1, response.Items.Count);
            Assert.AreEqual(1, response.PageInfo.TotalItems);

            var mapped = mapper.Map<Core.Models.Transaction>(response.Items.Single());
            Assert.AreEqual(items.Single(), mapped);
        }

        [TestMethod]
        public async Task IndexPage1()
        {
            // Given: A long set of items, which is longer than one page, but not as long as two pages 
            var pagesize = 25; 
            var items = FakeObjects<Core.Models.Transaction>.Make(pagesize).Add(pagesize / 2).SaveTo(this);

            // When: Getting "/"
            var response = await wireapi.ListTransactionsAsync(null, null, null, null, null);

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
            var response = await wireapi.ListTransactionsAsync(null, 2, null, null, null);

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
            var response = await wireapi.ListTransactionsAsync(q, null, null, null, null);

            // Then: The expected items are returned
            Assert.AreEqual(1, response.Items.Count);

            var mapped = mapper.Map<Core.Models.Transaction>(response.Items.Single());
            Assert.AreEqual(chosen.Single(), mapped);
        }

        [TestMethod]
        public async Task Details()
        {
            // Given: There are 5 items in the database, one of which we care about
            var expected = FakeObjects<Core.Models.Transaction>.Make(5).SaveTo(this).Last();
            var id = expected.ID;

            // When: Getting details for the chosen item
            var actual = await wireapi.GetTransactionAsync(id);

            // Then: That item is shown
            var mapped = mapper.Map<Core.Models.Transaction>(actual);
            Assert.AreEqual(expected, mapped);
        }

        [TestMethod]
        public async Task DetailsNotFound()
        {
            // Given: There are 5 items in the database
            var items = FakeObjects<Core.Models.Transaction>.Make(5).SaveTo(this);

            try
            {
                // When: Getting details for an ID which is not in the set
                var id = items.Max(x => x.ID) + 1;
                var actual = await wireapi.GetTransactionAsync(id);
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
        public async Task Create()
        {
            // Given: There is one item in the database, and another one waiting to be created
            var items = FakeObjects<Core.Models.Transaction>.Make(1).SaveTo(this).Add(1);
            var expected = items.Last();

            // When: Creating a new item
            var response = await wireapi.CreateTransactionAsync(mapper.Map<Client.Transaction>(expected));

            // Then: Now are two items in database
            Assert.AreEqual(items.Count, context.Set<Core.Models.Transaction>().Count());

            // And: The last one is the one we just added
            var actual = context.Set<Core.Models.Transaction>().OrderBy(x => x.ID).AsNoTracking().Last();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Delete()
        {
            // Given: There are two items in the database, one of which we care about
            var items = FakeObjects<Core.Models.Transaction>.Make(5).Add(1).SaveTo(this);
            var id = items.Group(1).Single().ID;

            // When: Deleting the selected item
            await wireapi.DeleteTransactionAsync(id);

            // Then: Now is only first group of items in database
            Assert.AreEqual(items.Group(0).Count, context.Set<Core.Models.Transaction>().Count());

            // And: The deleted item cannot be found;
            Assert.IsFalse(context.Set<Core.Models.Transaction>().Any(x => x.ID == id));
        }

        [TestMethod]
        public async Task DeleteNotFound()
        {
            // Given: There are 5 items in the database
            var data = FakeObjects<Core.Models.Transaction>.Make(4).SaveTo(this).Add(1);
            var newvalues = data.Group(1).Single();

            try
            {
                // When: Deleting an ID which doesn't exist
                var id = data.Group(0).Max(x => x.ID) + 1;
                await wireapi.DeleteTransactionAsync(id);
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

        #endregion
    }
}
