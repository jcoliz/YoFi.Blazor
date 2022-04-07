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
public class Portfolio: PageTest
{
    #region Overrides

    public override BrowserNewContextOptions ContextOptions => 
        new BrowserNewContextOptions
        {
            AcceptDownloads = true,
            ViewportSize = new ViewportSize() { Width = 1080, Height = 810 }
        };

    #endregion

    #region Helpers

    protected async Task WhenNavigatingToPage(string title)
    {
        // When: Navigating to the root of the site
        await Page.GotoAsync(TestContext.Properties["webAppUrl"] as string);

        // And: Clicking "{title}" on the navbar
        await Page.ClickAsync($"data-test-id=NavBar >> text={title}");
        await Page.WaitForLoadStateAsync();

        // Then: {title} View is visible
        var visible = await Page.IsVisibleAsync($"data-test-id={title}View");
        Assert.IsTrue(visible);
    }

    protected async Task ThenContainsItemsAsync(int from, int to)
    {
        Assert.AreEqual(from.ToString(), await Page.TextContentAsync("data-test-id=firstitem"));
        Assert.AreEqual(to.ToString(), await Page.TextContentAsync("data-test-id=lastitem"));
    }

    protected async Task<int> GetTotalItemsAsync() => await GetNumberAsync("data-test-id=totalitems");

    protected async Task<int> GetNumberAsync(string selector)
    {
        var totalitems = await Page.TextContentAsync(selector);

        if (!Int32.TryParse(totalitems, out int result))
            result = 0;

        return result;
    }

    protected async Task SaveScreenshotAsync(string moment = null)
    {
        var testname = $"{TestContext.FullyQualifiedTestClassName.Split(".").Last()}/{TestContext.TestName}";

        var displaymoment = string.IsNullOrEmpty(moment) ? string.Empty : $"-{moment.Replace('/','-')}";

        var filename = $"Screenshot/{testname}{displaymoment}.png";
        await Page.ScreenshotAsync(new PageScreenshotOptions() { Path = filename, OmitBackground = true, FullPage = true });
        TestContext.AddResultFile(filename);
    }

    #endregion

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
    /// [User Can] View the newest (placeholder) Transactions
    /// </summary>
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
        Assert.AreEqual(25,count);

        await SaveScreenshotAsync();
    }

    /// <summary>
    /// [User Can] View the current list of active payee matching rules
    /// </summary>
    [TestMethod]
    public async Task Initial()
    {
        // When: Navigating to the Transactions page
        await WhenNavigatingToPage("Transactions");

        // And: All expected items are here
        var totalitems = await GetTotalItemsAsync();
        Assert.AreEqual(785,totalitems);

        // And: This page covers items 1-25
        await ThenContainsItemsAsync(from: 1, to: 25);
    }

    /// <summary>
    /// [User Can] Page through the current list of active payee matching rules, without
    /// having to wait the entire list to load.
    /// [Scenario] Clicking on Page 2
    /// </summary>
    [TestMethod]
    public async Task Page2()
    {
        // When: Navigating to the Transactions page
        await WhenNavigatingToPage("Transactions");

        // When: Clicking on the next page on the pagination control
        await Page.ClickAsync("[aria-label=\"Page 2\"]");

        // And: Awaiting results to become visible
        var locator = Page.Locator("data-test-id=results");
        await locator.WaitForAsync();

        // Then: This page covers items 26-50
        await ThenContainsItemsAsync(from: 26, to: 50);
    }

    /// <summary>
    /// [User Can] Page through the current list of active payee matching rules, without
    /// having to wait the entire list to load.
    /// [Scenario] Clicking on Last Page
    /// </summary>
    [TestMethod]
    public async Task LastPage()
    {
        // When: Navigating to the Transactions page
        await WhenNavigatingToPage("Transactions");

        // When: Clicking on the next page on the pagination control
        await Page.ClickAsync("[aria-label=\"Last Page\"]");

        // And: Awaiting results to become visible
        var locator = Page.Locator("data-test-id=results");
        await locator.WaitForAsync();

        // Then: This page covers items 26-50
        await ThenContainsItemsAsync(from: 776, to: 785);
    }

    /// <summary>
    /// [User Can] Navigate to the Reports Page
    /// </summary>
    [TestMethod]
    public async Task _02_Reports()
    {
        // When: Navigating to the Reports page
        await WhenNavigatingToPage("Reports");

        // Then: Reports View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=ReportsView");
        Assert.IsTrue(visible);

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