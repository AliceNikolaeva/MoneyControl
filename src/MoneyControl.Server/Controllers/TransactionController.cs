using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyControl.Application.CSV;
using MoneyControl.Shared.Models;
using MoneyControl.Shared.Queries.Transaction.CreateTransaction;
using MoneyControl.Shared.Queries.Transaction.DeleteTransaction;
using MoneyControl.Shared.Queries.Transaction.GetTransactions;
using MoneyControl.Shared.Queries.Transaction.SearchTransactions;
using MoneyControl.Shared.Queries.Transaction.UpdateTransaction;

namespace MoneyControl.Server.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CsvReport _csvReport;

    public TransactionController(IMediator mediator, CsvReport csvReport)
    {
        _mediator = mediator;
        _csvReport = csvReport;
    }

    [HttpPost("create")]
    public async Task<int> Create(CreateTransactionCommand command)
    {
        var id = await _mediator.Send(command);
        return id;
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateTransactionCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteTransactionQuery query)
    {
        await _mediator.Send(query);
        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<TransactionModel>> GetAll()
    {
        var transactions = await _mediator.Send(new GetTransactionsCommand());
        return transactions;
    }

    [HttpGet("search")]
    public async Task<TransactionsModel> Search([FromQuery] SearchTransactionsQuery query)
    {
        var transactions = await _mediator.Send(query);
        return transactions;
    }

    [HttpGet("csv")]
    public async Task<byte[]> GetCsvReportAsync([FromQuery] CsvParameters query)
    {
        var result = await _csvReport.CreateCsvReportAsync(query, CancellationToken.None);
        return result;
    }
}