using System.Threading.Tasks;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YoFi.Vue.Tests.Functional;

[TestClass]
public class Portfolio: PageTest
{
    [TestMethod]
    public async Task _01_Transactions()
    {
        // When: Navigating to the root of the site
        await Page.GotoAsync("http://localhost:5003");

        // And: Clicking "Transactions" on the navbar
        await Page.ClickAsync("#navbarNav >> text=Transactions");
        await Page.WaitForLoadStateAsync();

        // Then: Transactions View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=TransactionsView");
        Assert.IsTrue(visible);
    }

    [TestMethod]
    public async Task _02_Reports()
    {
        // When: Navigating to the root of the site
        await Page.GotoAsync("http://localhost:5003");

        // And: Clicking "Transactions" on the navbar
        await Page.ClickAsync("#navbarNav >> text=Reports");
        await Page.WaitForLoadStateAsync();

        // Then: Transactions View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=ReportsView");
        Assert.IsTrue(visible);
    }

    [TestMethod]
    public async Task _03_AboutVue()
    {
        // When: Navigating to the root of the site
        await Page.GotoAsync("http://localhost:5003");

        // And: Clicking "Transactions" on the navbar
        await Page.ClickAsync("#navbarNav >> text=Vue");
        await Page.WaitForLoadStateAsync();

        // Then: Transactions View is visible
        var visible = await Page.IsVisibleAsync("data-test-id=HomeView");
        Assert.IsTrue(visible);
    }

}