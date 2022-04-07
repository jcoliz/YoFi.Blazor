using System.Threading.Tasks;
using System.Linq;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Playwright;
using System;

namespace YoFi.Vue.Tests.Functional;

[TestClass]
public class TransactionsTests: FunctionalTest
{
    /// <summary>
    /// [User Can] View the current list of transactions
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
    /// [User Can] Page through the current list of transactions, without
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
    /// [User Can] Page through the current list of transactions, without
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
}