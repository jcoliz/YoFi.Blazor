using System.Threading.Tasks;
using System.Linq;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Playwright;
using System;

namespace YoFi.Vue.Tests.Functional;

/// <summary>
/// Wide view of the whole breadth of the site
/// </summary>
/// <remarks>
/// "Portfolio" tests intend to cover the entire horizontal space of the project, but
/// very shallowly. Individual area tests will go deep in each area.
/// </remarks>
[TestClass]
public class Portfolio: FunctionalTest
{
    #region Tests

    /// <summary>
    /// [User Can] Navigate to the Transaction Page
    /// </summary>
    [TestMethod]
    public async Task _01_Transactions()
    {
        // When: Navigating to the Transactions page
        await WhenNavigatingToPage("Transactions");

        // Then: Transactions View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=TransactionsView");
        Assert.IsTrue(visible);

        await SaveScreenshotAsync();
    }

    /// <summary>
    /// [User Can] Navigate to the Reports Page
    /// </summary>
    [TestMethod]
    public async Task _02_Reports()
    {
        // When: Navigating to the Reports page
        await WhenNavigatingToPage("Reports");
        await Page.WaitForLoadStateAsync(state:LoadState.NetworkIdle);

        // Then: Reports View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=ReportsView");
        Assert.IsTrue(visible);

        // And: Reports get loaded
        await Page.WaitForSelectorAsync("data-test-id=reports-loaded");
        await SaveScreenshotAsync();
    }

    /// <summary>
    /// [User Can] Navigate to the About Page
    /// </summary>
    [TestMethod]
    public async Task _03_AboutVue()
    {
        // When: Navigating to the About page
        await WhenNavigatingToPage("About");

        // Then: About View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=AboutView");
        Assert.IsTrue(visible);

        await SaveScreenshotAsync();
   }

    #endregion
}