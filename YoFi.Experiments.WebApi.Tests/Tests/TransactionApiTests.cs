using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using YoFi.Core.Models;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.Experiments.WebApi.Tests;

[TestClass]
public class TransactionApiTests: BaseObjectApiTests<Transaction>
{
    #region Properties

    protected override string urlroot { get; set; } = "/Transactions";

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

    #endregion

    [TestMethod]
    public async Task GetSwagger()
    {
        // When: Getting the swagger file
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        // Then: Success
        response.EnsureSuccessStatusCode();
    }

}