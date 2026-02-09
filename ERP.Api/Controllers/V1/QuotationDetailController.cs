using System;
using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class QuotationDetailController : ControllerBase
{
    private readonly IQuotationDetailService _service;

    public QuotationDetailController(IQuotationDetailService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<QuotationDetail>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllDetailsAsync(ct));

    [HttpGet("{fran}/{branch}/{warehouse}/{quotType}/{quotationNo}/{quotSrl}")]
    public async Task<ActionResult<QuotationDetail>> Get(string fran, string branch, string warehouse, string quotType, string quotationNo, string quotSrl, CancellationToken ct)
    {
        var entity = await _service.GetDetailByKeyAsync(fran, branch, warehouse, quotType, quotationNo, quotSrl, ct);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpGet("by-header/{fran}/{branch}/{warehouse}/{quotType}/{quotationNo}")]
    public async Task<ActionResult<IReadOnlyList<QuotationDetail>>> GetByHeader(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct)
        => Ok(await _service.GetDetailsByHeaderAsync(fran, branch, warehouse, quotType, quotationNo, ct));

    [HttpPost]
    public async Task<ActionResult<QuotationDetail>> Create([FromBody] QuotationDetail entity, CancellationToken ct)
    {
        var created = await _service.CreateDetailAsync(entity, ct);
        return CreatedAtAction(nameof(Get), new { fran = created.Fran, branch = created.Branch, warehouse = created.Warehouse, quotType = created.QuotType, quotationNo = created.QuotationNo, quotSrl = created.QuotSrl }, created);
    }

    [HttpPut("{fran}/{branch}/{warehouse}/{quotType}/{quotationNo}/{quotSrl}")]
    public async Task<ActionResult<QuotationDetail>> Update(string fran, string branch, string warehouse, string quotType, string quotationNo, string quotSrl, [FromBody] QuotationDetail entity, CancellationToken ct)
    {
        // Ensure the key matches
        entity.Fran = fran;
        entity.Branch = branch;
        entity.Warehouse = warehouse;
        entity.QuotType = quotType;
        entity.QuotationNo = quotationNo;
        entity.QuotSrl = quotSrl;

        var updated = await _service.UpdateDetailAsync(entity, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{fran}/{branch}/{warehouse}/{quotType}/{quotationNo}/{quotSrl}")]
    public async Task<IActionResult> Delete(string fran, string branch, string warehouse, string quotType, string quotationNo, string quotSrl, CancellationToken ct)
        => await _service.DeleteDetailAsync(fran, branch, warehouse, quotType, quotationNo, quotSrl, ct) ? NoContent() : NotFound();
}
