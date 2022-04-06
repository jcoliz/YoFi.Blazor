using System.Threading.Tasks;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YoFi.Vue.Tests.Functional;

[TestClass]
public class Portfolio: PageTest
{
    #region Helpers

    protected async Task WhenNavigatingToPage(string title)
    {
        // When: Navigating to the root of the site
        await Page.GotoAsync(TestContext.Properties["webAppUrl"] as string);

        // And: Clicking "{title}" on the navbar
        await Page.ClickAsync($"#navbarNav >> text={title}");
        await Page.WaitForLoadStateAsync();

        // Then: {title} View is visible
        var visible = await Page.IsVisibleAsync($"data-test-id={title}View");
        Assert.IsTrue(visible);
    }

    #endregion

    #region Tests

    [TestMethod]
    public async Task _01_Transactions()
    {
        // When: Navigating to the Transactions page
        await WhenNavigatingToPage("Transactions");

        // Then: Transactions View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=TransactionsView");
        Assert.IsTrue(visible);
    }

    [TestMethod]
    public async Task _01A_Transactions_Loaded()
    {
        // When: Navigating to the Transactions page
        await WhenNavigatingToPage("Transactions");

        // And: Awaiting results to become visible
        var locator = Page.Locator("data-test-id=results");
        await locator.WaitForAsync();

        // Then: Five rows of results were loaded
        var rows = locator.Locator("tbody tr");
        var count = await rows.CountAsync();
        Assert.AreEqual(5,count);
    }

    [TestMethod]
    public async Task _02_Reports()
    {
        // When: Navigating to the Reports page
        await WhenNavigatingToPage("Reports");

        // Then: Reports View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=ReportsView");
        Assert.IsTrue(visible);
    }

    [TestMethod]
    public async Task _03_AboutVue()
    {
        // When: Navigating to the About page
        await WhenNavigatingToPage("About");

        // Then: About View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=AboutView");
        Assert.IsTrue(visible);
    }

    #endregion
}