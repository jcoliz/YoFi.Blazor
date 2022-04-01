using jcoliz.FakeObjects;
using jcoliz.OfficeOpenXml.Serializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using YoFi.Core.Models;
using YoFi.Core.Repositories.Wire;

namespace YoFi.Experiments.WebApi.Tests
{
    [TestClass]
    public abstract class BaseObjectApiTests<T>: BaseApiTests where T : class, IID, new()
    {
        #region Helpers

        protected Task<JsonDocument> WhenGettingIndex(IWireQueryParameters parms)
        {
            var terms = new List<string>();

            if (!string.IsNullOrEmpty(parms.Query))
            {
                terms.Add($"query={HttpUtility.UrlEncode(parms.Query)}");
            }
            if (!string.IsNullOrEmpty(parms.Order))
            {
                terms.Add($"order={HttpUtility.UrlEncode(parms.Order)}");
            }
            if (!string.IsNullOrEmpty(parms.View))
            {
                terms.Add($"view={HttpUtility.UrlEncode(parms.View)}");
            }
            if (parms.Page.HasValue)
            {
                terms.Add($"page={parms.Page.Value}");
            }

            var urladd = (terms.Any()) ? "?" + string.Join("&", terms) : string.Empty;

            return WhenGetAsync($"{urlroot}/{urladd}");
        }

        protected void ThenResultsAreEqual<TItem>(JsonDocument document, IEnumerable<TItem> items)
        {
            // Then: The expected items are returned
            var actual = JsonSerializer.Deserialize<List<TItem>>(document.RootElement.GetProperty("items"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            Assert.IsTrue(actual.SequenceEqual(items));
        }

        protected async Task ThenIsSpreadsheetContaining<TItem>(HttpContent content, IEnumerable<TItem> items) where TItem : class, new()
        {
            // Then: It's a stream
            Assert.IsInstanceOfType(content, typeof(StreamContent));
            var streamcontent = content as StreamContent;

            // And: The stream contains a spreadsheet
            using var ssr = new SpreadsheetReader();
            ssr.Open(await streamcontent.ReadAsStreamAsync());

            // And: The spreadsheet contains all our items
            var actual = ssr.Deserialize<TItem>();
            Assert.IsTrue(items.SequenceEqual(actual));
        }

        #endregion

        #region Init/Cleanup

        [TestCleanup]
        public void Cleanup()
        {
            // Reset the clock back
            //integrationcontext.clock.Reset();

            // Clean out database
            context.Set<Transaction>().RemoveRange(context.Set<Transaction>());
            context.Set<Payee>().RemoveRange(context.Set<Payee>());
            context.Set<Receipt>().RemoveRange(context.Set<Receipt>());
            context.SaveChanges();
        }

        #endregion

        #region Tests

        [TestMethod]
        public async Task IndexEmpty()
        {
            // Given: No data in database

            // When: Getting "/"
            var document = await WhenGettingIndex(new WireQueryParameters());

            // Then: No items returned
            Assert.AreEqual(0, document.RootElement.GetProperty("items").GetArrayLength());
            var totalitems = document.RootElement.GetProperty("pageInfo").GetProperty("totalItems").GetInt32();
            Assert.AreEqual(0, totalitems);
        }

        [TestMethod]
        public async Task IndexSingle()
        {
            // Given: One item in database
            var items = FakeObjects<T>.Make(1).SaveTo(this);

            // When: Getting "/"
            var document = await WhenGettingIndex(new WireQueryParameters());

            // And: Expected items returned
            ThenResultsAreEqual(document, items);
        }

        [TestMethod]
        public async Task IndexPage1()
        {
            // Given: A long set of items, which is longer than one page, but not as long as two pages 
            var pagesize = 25; // BaseRepository<T>.DefaultPageSize;
            var items = FakeObjects<T>.Make(pagesize).Add(pagesize / 2).SaveTo(this);

            // When: Getting the Index
            var document = await WhenGettingIndex(new WireQueryParameters());

            // Then: Only one page of items returned, which are the LAST group, cuz it's sorted by time
            ThenResultsAreEqual(document, items.Group(0));
        }

        [TestMethod]
        public async Task IndexPage2()
        {
            // Given: A long set of items, which is longer than one page, but not as long as two pages 
            var pagesize = 25;
            var items = FakeObjects<T>.Make(pagesize).Add(pagesize / 2).SaveTo(this);

            // When: Getting the Index for page 2
            var document = await WhenGettingIndex(new WireQueryParameters() { Page = 2 });

            // Then: Only 2nd page items returned
            ThenResultsAreEqual(document, items.Group(1));
        }

        [TestMethod]
        public async Task IndexSearch()
        {
            // Given: There are 5 items in the database, one of which we care about
            var chosen = FakeObjects<T>.Make(5).SaveTo(this).Take(1);

            // When: Searching the index for the focused item's testkey
            var q = (string)TestKey<T>.Order()(chosen.Single());
            var document = await WhenGettingIndex(new WireQueryParameters() { Query = q });

            // Then: The expected items are returned
            ThenResultsAreEqual(document, chosen);
        }

        [TestMethod]
        public async Task Details()
        {
            // Given: There are 5 items in the database, one of which we care about
            var expected = FakeObjects<T>.Make(5).SaveTo(this).Last();
            var id = expected.ID;

            // When: Getting details for the chosen item
            var document = await WhenGetAsync($"{urlroot}/{id}");

            // Then: That item is shown
            var actual = JsonSerializer.Deserialize<T>(document, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task DetailsNotFound()
        {
            // Given: There are 5 items in the database
            var items = FakeObjects<T>.Make(5).SaveTo(this);

            // When: Getting details for an ID which is not in the set
            // Then: Not Found
            var id = items.Max(x => x.ID) + 1;
            await WhenGetAsync($"{urlroot}/{id}", HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task Edit()
        {
            // Given: There are 5 items in the database, one of which we care about, plus an additional item to be use as edit values
            var data = FakeObjects<T>.Make(4).SaveTo(this).Add(1);
            var id = data.Group(0).Last().ID;
            var newvalues = data.Group(1).Single();

            // When: Editing the chosen item with the new values
            var response = await WhenSendAsync($"{urlroot}/{id}", newvalues, HttpMethod.Put);

            // Then: Succeeds
            response.EnsureSuccessStatusCode();

            // And: Then item was updated
            var actual = context.Set<T>().Where(x => x.ID == id).AsNoTracking().Single();
            Assert.AreEqual(newvalues, actual);
        }

        [TestMethod]
        public async Task EditNotFound()
        {
            // Given: There are 5 items in the database
            var data = FakeObjects<T>.Make(4).SaveTo(this).Add(1);
            var newvalues = data.Group(1).Single();

            // When: Editing an ID which doesn't exist the new values
            var id = data.Group(0).Max(x => x.ID) + 1;
            var response = await WhenSendAsync($"{urlroot}/{id}", newvalues, HttpMethod.Put);

            // Then: NotFound
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Create()
        {
            // Given: There is one item in the database, and another one waiting to be created
            var items = FakeObjects<T>.Make(1).SaveTo(this).Add(1);
            var expected = items.Last();

            // When: Creating a new item
            var response = await WhenSendAsync($"{urlroot}/", expected);

            // Then: Succeeds
            response.EnsureSuccessStatusCode();

            // And: Received a route

            // And: Now are two items in database
            Assert.AreEqual(items.Count, context.Set<T>().Count());

            // And: The last one is the one we just added
            var actual = context.Set<T>().OrderBy(x => x.ID).AsNoTracking().Last();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Delete()
        {
            // Given: There are two items in the database, one of which we care about
            var items = FakeObjects<T>.Make(5).Add(1).SaveTo(this);
            var id = items.Group(1).Single().ID;

            // When: Deleting the selected item
            var response = await client.DeleteAsync($"{urlroot}/{id}");

            // Then: Succeeds
            response.EnsureSuccessStatusCode();

            // And: Now is only first group of items in database
            Assert.AreEqual(items.Group(0).Count, context.Set<T>().Count());

            // And: The deleted item cannot be found;
            Assert.IsFalse(context.Set<T>().Any(x => x.ID == id));
        }

        [TestMethod]
        public async Task DeleteNotFound()
        {
            // Given: There are 5 items in the database
            var data = FakeObjects<T>.Make(4).SaveTo(this).Add(1);
            var newvalues = data.Group(1).Single();

            // When: Deleting an ID which doesn't exist
            var id = data.Group(0).Max(x => x.ID) + 1;
            var response = await client.DeleteAsync($"{urlroot}/{id}");

            // Then: NotFound
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public virtual async Task Download()
        {
            // Given: Many items in the database
            var items = FakeObjects<T>.Make(20).SaveTo(this);

            // When: Downloading them
            var response = await client.GetAsync($"{urlroot}/Download/2000?allyears=true");

            // Then: Response is OK
            response.EnsureSuccessStatusCode();

            // And: It's a spreadsheet containing our items
            await ThenIsSpreadsheetContaining(response.Content, items);
        }

        #endregion
    }
}
