using Common.DotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using YoFi.Core.Models;
using YoFi.Core.Repositories;
using YoFi.Core.Repositories.Wire;

namespace YoFi.WireApi.Controllers;

/// <summary>
/// Super thin API controller for Transaction Repository
/// </summary>
/// <remarks>
/// This is meant to be so thin it could be auto-generated
/// </remarks>
[ApiController]
[Route("wireapi/[controller]")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ILogger<TransactionsController> _logger;
    private readonly ITransactionRepository _repository;
    private readonly IClock _clock;

    public TransactionsController(ILogger<TransactionsController> logger, ITransactionRepository repository, IClock clock)
    {
        _logger = logger;
        _repository = repository;
        _clock = clock;
    }

    [HttpGet(Name = "ListTransactions")]
    [ProducesResponseType(typeof(WireQueryResult<Transaction>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] WireQueryParameters parameters)
    {
        var result = await _repository.GetByQueryAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetTransaction")]
    [ProducesResponseType(typeof(Transaction), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        // TODO: Use a filter
        if (!await _repository.TestExistsByIdAsync(id))
            return NotFound();

        var result = await _repository.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpDelete("{id}", Name = "DeleteTransaction")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        // TODO: Use a filter
        if (!await _repository.TestExistsByIdAsync(id))
            return NotFound();

        var item = await _repository.GetByIdAsync(id);
        await _repository.RemoveAsync(item);
        return Ok();
    }

    [HttpPost(Name = "CreateTransaction")]
    [ProducesResponseType(typeof(Transaction),StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([Bind("Timestamp,Amount,Memo,Payee,Category,BankReference")] Transaction transaction )
    {
        await _repository.AddAsync(transaction);
        var requesturl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
        return Created( $"{requesturl}/{transaction.ID}", transaction);
    }

    [HttpPut("{id}", Name = "UpdateTransaction")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(int id, [Bind("Timestamp,Amount,Memo,Payee,Category,BankReference")] Transaction transaction )
    {
        // TODO: Use a filter
        if (!await _repository.TestExistsByIdAsync(id))
            return NotFound();

        var item = await _repository.GetByIdAsync(id);
        transaction.ID = id;
        await _repository.UpdateAsync(transaction);
        return base. Ok();
    }

    [HttpGet("Download/{year}", Name = "DownloadTransactions")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Type = typeof(FileStream))]
    public async Task<IActionResult> Download(int year, bool allyears, string query)
    {
        var stream = await _repository.AsSpreadsheetAsync(year, true, query);

        IActionResult file = File(stream, contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: "Transactions.xlsx");

        return file;
    }
}
