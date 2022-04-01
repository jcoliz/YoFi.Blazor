using jcoliz.FakeObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using YoFi.AspNet.Data;
using YoFi.Tests.Integration.Helpers;
using YoFi.Core.Models;
using YoFi.Core.Repositories.Wire;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace YoFi.Experiments.WebApi.Tests;

[TestClass]
public class TransactionApiTests: IFakeObjectsSaveTarget
{
    #region Fields

    protected static IntegrationContext integrationcontext;
    protected static HttpClient client => integrationcontext.client;
    protected static ApplicationDbContext context => integrationcontext.context;

    protected string urlroot => "/Transactions/";

    #endregion

    #region Properties

    public TestContext TestContext { get; set; }

    #endregion

    #region Helpers

    public void AddRange(IEnumerable objects)
    {
        if (objects is IEnumerable<Transaction> txs)
        {
            context.AddRange(txs);
            context.SaveChanges();
        }
        else
            throw new System.NotImplementedException();
    }

    protected async Task<JsonDocument> WhenGetAsync(string url, HttpStatusCode expectedresult = HttpStatusCode.OK)
    {
        // When: Getting {url}
        var response = await client.GetAsync(url);

        // Then: Result as expected
        Assert.AreEqual(response.StatusCode, expectedresult);

        // And: Response is valid JSON, if valid
        JsonDocument document = null;
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

        return document;
    }

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

        return WhenGetAsync($"{urlroot}{urladd}");
    }

    protected void ThenResultsAreEqual<T>(JsonDocument document, IEnumerable<T> items)
    {
        // Then: The expected items are returned
        var actual = JsonSerializer.Deserialize<List<T>>(document.RootElement.GetProperty("items"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        Assert.IsTrue(actual.SequenceEqual(items));
    }

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

    [TestMethod]
    public async Task GetSwagger()
    {
        // When: Getting the swagger file
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        // Then: Success
        response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    public async Task IndexEmpty()
    {
        // Given: No data in database

        // When: Getting "/"
        var document = await WhenGettingIndex(new WireQueryParameters());

        // And: No items returned
        Assert.AreEqual(0, document.RootElement.GetProperty("items").GetArrayLength());
        var totalitems = document.RootElement.GetProperty("pageInfo").GetProperty("totalItems").GetInt32();
        Assert.AreEqual(0, totalitems);
    }

    [TestMethod]
    public async Task IndexSingle()
    {
        // Given: One item in database
        var items = FakeObjects<Transaction>.Make(1).SaveTo(this);

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
        var items = FakeObjects<Transaction>.Make(pagesize).Add(pagesize / 2).SaveTo(this);

        // When: Getting the Index
        var document = await WhenGettingIndex(new WireQueryParameters());

        // Then: Only one page of items returned, which are the LAST group, cuz it's sorted by time
        ThenResultsAreEqual(document, items.Group(0));
    }


    [TestMethod]
    public async Task IndexPage2()
    {
        // Given: A long set of items, which is longer than one page, but not as long as two pages 
        var pagesize = 25; // BaseRepository<BudgetTx>.DefaultPageSize;
        var items = FakeObjects<Transaction>.Make(pagesize).Add(pagesize / 2).SaveTo(this);

        // When: Getting the Index for page 2
        var document = await WhenGettingIndex(new WireQueryParameters() { Page = 2 } );

        // And: Only 2nd page items returned
        ThenResultsAreEqual(document, items.Group(1));
    }

    [TestMethod]
    public async Task IndexSearch()
    {
        // Given: There are 5 items in the database, one of which we care about
        var chosen = FakeObjects<Transaction>.Make(5).SaveTo(this).Take(1);

        // When: Searching the index for the focused item's testkey
        var q = (string)TestKey<Transaction>.Order()(chosen.Single());
        var document = await WhenGettingIndex(new WireQueryParameters() { Query = q });

        // Then: The expected items are returned
        ThenResultsAreEqual(document, chosen);
    }

    [TestMethod]
    public async Task Details()
    {
        // Given: There are 5 items in the database, one of which we care about
        var expected = FakeObjects<Transaction>.Make(5).SaveTo(this).Last();
        var id = expected.ID;

        // When: Getting details for the chosen item
        var document = await WhenGetAsync($"{urlroot}{id}");

        // Then: That item is shown
        var actual = JsonSerializer.Deserialize<Transaction>(document, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task DetailsNotFound()
    {
        // Given: There are 5 items in the database
        var items = FakeObjects<Transaction>.Make(5).SaveTo(this);

        // When: Getting details for an ID which is not in the set
        // Then: Not Found
        var id = items.Max(x=>x.ID) + 1;
        await WhenGetAsync($"{urlroot}{id}",HttpStatusCode.NotFound);
    }

}