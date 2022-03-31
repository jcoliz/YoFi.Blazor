using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using YoFi.AspNet.Data;
using YoFi.Tests.Integration.Helpers;
using YoFi.Core.Models;

namespace YoFi.Experiments.WebApi.Tests;

[TestClass]
public class TransactionApiTests
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
}