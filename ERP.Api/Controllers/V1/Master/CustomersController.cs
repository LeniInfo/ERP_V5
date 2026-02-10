using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Master;
using ERP.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Api.Controllers.V1.Master;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/master/[controller]")]
#if DEBUG
[AllowAnonymous]
#endif
public sealed class CustomersController : ControllerBase
{
    private readonly ICustomersService _svc;
    private readonly ErpDbContext _context;

    public CustomersController(
        ICustomersService svc,
        ErpDbContext context)
    {
        _svc = svc;
        _context = context;
    }

    [HttpGet]
    [Authorize(Policy = "erp.api.read")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> Get(CancellationToken ct)
        => Ok(await _svc.GetAllAsync(ct));

    [HttpGet("next-code")]
    public async Task<ActionResult<string>> GetNextCustomerCode()
    {
        var lastCode = await _context.Customers
            .OrderByDescending(x => x.CustomerCode)
            .Select(x => x.CustomerCode)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(lastCode))
            return "C1";

        int numberPart = int.Parse(lastCode.Substring(1));
        return $"C{numberPart + 1}";
    }

    [HttpGet("{customerCode}")]
    [Authorize(Policy = "erp.api.read")]
    public async Task<ActionResult<CustomerDto>> GetByCode(string customerCode, CancellationToken ct)
    {
        var item = await _svc.GetByCodeAsync(customerCode, ct);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [Authorize(Policy = "erp.api.write")]
    public async Task<ActionResult<CustomerDto>> Create(
        [FromBody] CreateCustomerRequest req,
        CancellationToken ct)
    {
        var created = await _svc.CreateAsync(req, ct);
        return CreatedAtAction(
            nameof(GetByCode),
            new { version = "1.0", customerCode = created.CustomerCode },
            created);
    }

    [HttpPut("{customerCode}")]
    [Authorize(Policy = "erp.api.write")]
    public async Task<IActionResult> Update(
        string customerCode,
        [FromBody] UpdateCustomerRequest req,
        CancellationToken ct)
    {
        var ok = await _svc.UpdateAsync(customerCode, req, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{customerCode}")]
    [Authorize(Policy = "erp.api.write")]
    public async Task<IActionResult> Delete(string customerCode, CancellationToken ct)
    {
        var ok = await _svc.DeleteAsync(customerCode, ct);
        return ok ? NoContent() : NotFound();
    }
}
