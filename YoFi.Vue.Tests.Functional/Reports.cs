using System.Threading.Tasks;
using System.Linq;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Playwright;
using System;

namespace YoFi.Vue.Tests.Functional;

[TestClass]
public class ReportsTests: FunctionalTest
{
    #region Helpers

    private async Task GivenOnReportsPage()
    {
        // When: Navigating to the Reports page
        await WhenNavigatingToPage("Reports");
        
        // And: Waiting for reports to actually be loaded
        await Page.WaitForSelectorAsync("data-test-id=reports-loaded");
    }

    #endregion

    #region Tests

    /// <summary>
    /// [User Can] View a summary of their entire financial picture on a single page
    /// </summary>
    /// <remarks>
    /// Feature AB#1114: Report Cover Page: One page summary view of the whole picture
    /// </remarks>
    [DataRow("income")]
    [DataRow("taxes")]
    [DataRow("expenses")]
    [DataRow("savings")]
    [DataTestMethod]
    public async Task Summary(string report)
    {
        // Given: We are logged in and on the reports page
        await GivenOnReportsPage();

        // When: Checking the total of the {report} summary
        var total = await Page.Locator($"data-test-id=report-{report} >> tr.report-row-total >> td.report-col-total").TextContentAsync();
        var totaltext = total.Trim();

        // And: Clicking on the detailed report link for the {report} report
        await Page.ClickAsync($"data-test-id={report}-detail");

        // And: Waiting for the page to fully load
        await Page.WaitForSelectorAsync("data-test-id=ReportDisplay");
        await SaveScreenshotAsync(report);

        // Then: The total on the detailed report is the same as in the summary
        var summarytotal = await Page.Locator("tr.report-row-total >> td.report-col-total").TextContentAsync();
        var summarytotaltext = summarytotal.Trim();

        Assert.AreEqual(summarytotaltext, totaltext);
    }

    /// <summary>
    /// [User Can] Select and view a report of their choice
    /// </summary>
    /// <param name="report">Name of the report</param>
    [DataRow("Income")]
    [DataRow("Expenses")]
    [DataRow("Taxes")]
    [DataRow("Savings")]
    [DataRow("Income Detail")]
    [DataRow("Expenses Detail")]
    [DataRow("Taxes Detail")]
    [DataRow("Savings Detail")]
    [DataRow("Expenses Budget")]
    [DataRow("All Transactions")]
    [DataRow("Full Budget")]
    [DataRow("All vs. Budget")]
    [DataRow("Expenses vs. Budget")]
    [DataRow("Managed Budget")]
    [DataRow("Transaction Export")]
    [DataRow("Year over Year")]
    [DataTestMethod]
    public async Task AllReports(string report)
    {
        // Given: We are logged in and on the reports page
        await GivenOnReportsPage();

        // When: Selecting the "all" report from the dropdown
        await Page.ClickAsync("text=Choose a Report");
        await Page.ClickAsync($"text={report}");

        // And: Waiting for the page to fully load
        await Page.WaitForSelectorAsync("data-test-id=ReportDisplay");
        await Page.WaitForLoadStateAsync(state:LoadState.NetworkIdle);
        await SaveScreenshotAsync($"{report}-loaded");

        // Then: The expected report is generated
        //await Page.ThenIsOnPageAsync(expected);
        await ThenH2Is(report);
    }
    #endregion
}