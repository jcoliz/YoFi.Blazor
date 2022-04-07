using System.Threading.Tasks;
using System.Linq;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Playwright;
using System;

namespace YoFi.Vue.Tests.Functional;

/// <summary>
/// Base test class shared by all functional test classes
/// </summary>
public class FunctionalTest: PageTest
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

    protected async Task ThenH2Is(string expected)
    {
        var content = await Page.TextContentAsync("h2");
        Assert.AreEqual(expected, content);
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

}