using System.Threading.Tasks;
using System.Linq;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Playwright;

namespace YoFi.Vue.Tests.Functional;

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
        await Page.ClickAsync($"#navbarNav >> text={title}");
        await Page.WaitForLoadStateAsync();

        // Then: {title} View is visible
        var visible = await Page.IsVisibleAsync($"data-test-id={title}View");
        Assert.IsTrue(visible);
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

        await SaveScreenshotAsync();
    }

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