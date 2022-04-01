using jcoliz.FakeObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using YoFi.AspNet.Data;
using YoFi.Tests.Integration.Helpers;
using YoFi.Core.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
    public async Task GetEmpty()
    {
        // Given: No data in database

        // When: Getting "/"
        var response = await client.GetAsync(urlroot);

        // Then: Success
        response.EnsureSuccessStatusCode();

        // And: No items returned
        var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.AreEqual(0, document.RootElement.GetProperty("items").GetArrayLength());

        var totalitems = document.RootElement.GetProperty("pageInfo").GetProperty("totalItems").GetInt32();
        Assert.AreEqual(0, totalitems);
    }

    [TestMethod]
    public async Task GetSome()
    {
        // Given: Some items in database
        var items = FakeObjects<Transaction>.Make(7).SaveTo(this);

        // When: Getting "/"
        var response = await client.GetAsync(urlroot);

        // Then: Success
        response.EnsureSuccessStatusCode();

        // And: Expected items returned
        var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var actual = JsonSerializer.Deserialize<List<Transaction>>(document.RootElement.GetProperty("items"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        Assert.IsTrue(actual.SequenceEqual(items));
    }

    [TestMethod]
    public async Task GetPage2()
    {
        // Given: A long set of items, which is longer than one page, but not as long as two pages 
        var pagesize = 25; // BaseRepository<BudgetTx>.DefaultPageSize;
        var items = FakeObjects<Transaction>.Make(pagesize).Add(pagesize / 2).SaveTo(this);

        // When: Getting the Index for page 2
        var response = await client.GetAsync(urlroot + "?page=2");

        // Then: Success
        response.EnsureSuccessStatusCode();

        // And: Only 2nd page items returned
        var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var actual = JsonSerializer.Deserialize<List<Transaction>>(document.RootElement.GetProperty("items"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        Assert.IsTrue(actual.SequenceEqual(items.Group(1)));
    }

    [TestMethod]
    public async Task GetSwagger()
    {
        // When: Getting the swagger file
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        // Then: Success
        response.EnsureSuccessStatusCode();
    }
}