using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoFi.Core.Models;
using YoFi.Tests.Integration.Helpers;

namespace YoFi.WireApi.Tests.Integration;

[TestClass]
public class TransactionApiTests: BaseObjectApiTests<Transaction>
{
    #region Properties

    protected override string urlroot { get; set; } = "wireapi/Transactions";

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
}