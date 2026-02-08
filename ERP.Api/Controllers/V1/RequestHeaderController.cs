using System;
using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class RequestHeaderController : ControllerBase
{
    private readonly IRequestHeaderService _service;

    public RequestHeaderController(IRequestHeaderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RequestHeader>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllAsync(ct));

    [HttpGet("{fran}/{branch}/{warehouse}/{requestType}/{requestNo}")]
    public async Task<ActionResult<RequestHeader>> Get(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct)
    {
        var entity = await _service.GetByKeyAsync(fran, branch, warehouse, requestType, requestNo, ct);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<RequestHeader>> Create([FromBody] RequestHeader entity, CancellationToken ct)
    {
        // Validate DESCEN and DESARABIC max length
        if (entity.DescEn != null && entity.DescEn.Length > 300)
        {
            return BadRequest(new { message = "DESCEN cannot exceed 300 characters." });
        }
        if (entity.DescArabic != null && entity.DescArabic.Length > 300)
        {
            return BadRequest(new { message = "DESCARABIC cannot exceed 300 characters." });
        }

        var created = await _service.CreateAsync(entity, ct);
        return CreatedAtAction(nameof(Get), new { fran = created.Fran, branch = created.Branch, warehouse = created.Warehouse, requestType = created.RequestType, requestNo = created.RequestNo }, created);
    }

    [HttpPut("{fran}/{branch}/{warehouse}/{requestType}/{requestNo}")]
    public async Task<ActionResult<RequestHeader>> Update(string fran, string branch, string warehouse, string requestType, string requestNo, [FromBody] RequestHeader entity, CancellationToken ct)
    {
        // Ensure the key matches
        entity.Fran = fran;
        entity.Branch = branch;
        entity.Warehouse = warehouse;
        entity.RequestType = requestType;
        entity.RequestNo = requestNo;

        // Validate DESCEN and DESARABIC max length
        if (entity.DescEn != null && entity.DescEn.Length > 300)
        {
            return BadRequest(new { message = "DESCEN cannot exceed 300 characters." });
        }
        if (entity.DescArabic != null && entity.DescArabic.Length > 300)
        {
            return BadRequest(new { message = "DESCARABIC cannot exceed 300 characters." });
        }

        var updated = await _service.UpdateAsync(entity, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{fran}/{branch}/{warehouse}/{requestType}/{requestNo}")]
    public async Task<IActionResult> Delete(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct)
        => await _service.DeleteAsync(fran, branch, warehouse, requestType, requestNo, ct) ? NoContent() : NotFound();

    [HttpGet("next-request-number")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(object), 500)]
    public async Task<ActionResult<string>> GetNextRequestNumber(CancellationToken ct)
    {
        try
        {
            var requestNo = await _service.GetNextRequestNumberAsync(ct);
            return Ok(requestNo);
        }
        catch (Exception ex)
        {
            // Log the full exception details for debugging
            return StatusCode(500, new { 
                message = $"Failed to generate request number: {ex.Message}", 
                error = ex.GetType().Name,
                stackTrace = ex.StackTrace 
            });
        }
    }
}
