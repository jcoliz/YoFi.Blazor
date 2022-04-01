using Common.DotNet;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YoFi.Core.Models;
using YoFi.Core.Repositories;
using YoFi.Core.Repositories.Wire;

namespace YoFi.Experiments.WebApi.Controllers;

/// <summary>
/// Super thin API controller for Transaction Repository
/// </summary>
/// <remarks>
/// This is meant to be so thin it could be auto-generated
/// </remarks>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ITransactionRepository _repository;
    private readonly IClock _clock;

    public TransactionsController(ILogger<WeatherForecastController> logger, ITransactionRepository repository, IClock clock)
    {
        _logger = logger;
        _repository = repository;
        _clock = clock;
    }

    [HttpGet]
    [ProducesResponseType(typeof(WireQueryResult<Transaction>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get([FromQuery] WireQueryParameters parameters)
    {
        var result = await _repository.GetByQueryAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Transaction), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(int id)
    {
        // TODO: Use a filter
        if (!await _repository.TestExistsByIdAsync(id))
            return NotFound();

        var result = await _repository.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // TODO: Use a filter
        if (!await _repository.TestExistsByIdAsync(id))
            return NotFound();

        var item = await _repository.GetByIdAsync(id);
        await _repository.RemoveAsync(item);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Timestamp,Amount,Memo,Payee,Category,BankReference")] Transaction transaction )
    {
        await _repository.AddAsync(transaction);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit(int id, [Bind("Timestamp,Amount,Memo,Payee,Category,BankReference")] Transaction transaction )
    {
        // TODO: Use a filter
        if (!await _repository.TestExistsByIdAsync(id))
            return NotFound();

        var item = await _repository.GetByIdAsync(id);
        transaction.ID = id;
        await _repository.UpdateAsync(transaction);
        return Ok();
    }

    [HttpGet("Download/{year}")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",Type = typeof(FileStreamResult))]
    public async Task<IActionResult> Download(int year, bool allyears, string query)
    {
        var stream = await _repository.AsSpreadsheetAsync(year, true, query);

        IActionResult file = File(stream, contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: "Transactions.xlsx");

        return file;
    }
}
