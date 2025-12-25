using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyControl.Shared.Models;
using MoneyControl.Shared.Queries.Account.CreateAccount;
using MoneyControl.Shared.Queries.Account.DeleteAccount;
using MoneyControl.Shared.Queries.Account.GetAccounts;
using MoneyControl.Shared.Queries.Account.UpdateAccount;

namespace MoneyControl.Server.Controllers;

[ApiController]
[Authorize("ApiScope")]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<object> Create(CreateAccountCommand command)
    {
        var id = await _mediator.Send(command);
        return id;
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateAccountCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteAccountCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet]
    public async Task<IEnumerable<AccountModel>> GetAll()
    {
        var accounts = await _mediator.Send(new GetAccountsCommand());
        return accounts;
    }
}