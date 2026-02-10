using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers.V1.Master;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/master/[controller]")]
#if DEBUG
[AllowAnonymous]
#endif
public sealed class CustomersController(ICustomersService svc) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> Get(CancellationToken ct)
        => Ok(await svc.GetAllAsync(ct));

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> Search([FromQuery] string name, CancellationToken ct)
        => Ok(await svc.SearchByNameAsync(name ?? string.Empty, ct));

    [HttpGet("{customerCode}")]
    public async Task<ActionResult<CustomerDto>> GetByCode(string customerCode, CancellationToken ct)
    {
        var item = await svc.GetByCodeAsync(customerCode, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerRequest req, CancellationToken ct)
    {
        var created = await svc.CreateAsync(req, ct);
        return CreatedAtAction(nameof(GetByCode), new { version = "1.0", customerCode = created.CustomerCode }, created);
    }

    [HttpPut("{customerCode}")]
    public async Task<IActionResult> Update(string customerCode, [FromBody] UpdateCustomerRequest req, CancellationToken ct)
    {
        var ok = await svc.UpdateAsync(customerCode, req, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{customerCode}")]
    public async Task<IActionResult> Delete(string customerCode, CancellationToken ct)
    {
        var ok = await svc.DeleteAsync(customerCode, ct);
        return ok ? NoContent() : NotFound();
    }

}
