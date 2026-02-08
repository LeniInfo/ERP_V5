using System;
using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class QuotationHeaderController : ControllerBase
{
    private readonly IQuotationHeaderService _service;

    public QuotationHeaderController(IQuotationHeaderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<QuotationHeader>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{fran}/{branch}/{warehouse}/{quotType}/{quotationNo}")]
    public async Task<ActionResult<QuotationHeader>> Get(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct)
    {
        var entity = await _service.GetByKeyAsync(fran, branch, warehouse, quotType, quotationNo, ct);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<QuotationHeader>> Create([FromBody] QuotationHeader entity, CancellationToken ct)
    {
        var created = await _service.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(Get), new { fran = created.Fran, branch = created.Branch, warehouse = created.Warehouse, quotType = created.QuotType, quotationNo = created.QuotationNo }, created);
    }

    [HttpPut("{fran}/{branch}/{warehouse}/{quotType}/{quotationNo}")]
    public async Task<ActionResult<QuotationHeader>> Update(string fran, string branch, string warehouse, string quotType, string quotationNo, [FromBody] QuotationHeader entity, CancellationToken ct)
    {
        // Ensure the key matches
        entity.Fran = fran;
        entity.Branch = branch;
        entity.Warehouse = warehouse;
        entity.QuotType = quotType;
        entity.QuotationNo = quotationNo;

        var updated = await _service.UpdateAsync(entity, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{fran}/{branch}/{warehouse}/{quotType}/{quotationNo}")]
    public async Task<IActionResult> Delete(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct)
        => await _service.DeleteAsync(fran, branch, warehouse, quotType, quotationNo, ct) ? NoContent() : NotFound();

    [HttpGet("next-quotation-number")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<ActionResult<string>> GetNextQuotationNumber(CancellationToken ct)
    {
        try
        {
            var quotationNo = await _service.GetNextQuotationNumberAsync(ct);
            return Ok(quotationNo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message, error = ex.GetType().Name });
        }
    }
}
