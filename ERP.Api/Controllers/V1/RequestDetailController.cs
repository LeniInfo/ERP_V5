using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class RequestDetailController : ControllerBase
{
    private readonly IRequestDetailService _service;

    public RequestDetailController(IRequestDetailService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RequestDetail>>> GetAll(CancellationToken ct)
        => Ok(await _service.GetAllDetailsAsync(ct));

    [HttpGet("{fran}/{branch}/{warehouse}/{requestType}/{requestNo}/{requestSrl}")]
    public async Task<ActionResult<RequestDetail>> Get(string fran, string branch, string warehouse, string requestType, string requestNo, string requestSrl, CancellationToken ct)
    {
        var entity = await _service.GetDetailByKeyAsync(fran, branch, warehouse, requestType, requestNo, requestSrl, ct);
        return entity == null ? NotFound() : Ok(entity);
    }

    [HttpGet("by-header/{fran}/{branch}/{warehouse}/{requestType}/{requestNo}")]
    public async Task<ActionResult<IReadOnlyList<RequestDetail>>> GetByHeader(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct)
        => Ok(await _service.GetDetailsByHeaderAsync(fran, branch, warehouse, requestType, requestNo, ct));

    [HttpPost]
    public async Task<ActionResult<RequestDetail>> Create([FromBody] RequestDetail entity, CancellationToken ct)
    {
        var created = await _service.CreateDetailAsync(entity, ct);
        return CreatedAtAction(nameof(Get), new { fran = created.Fran, branch = created.Branch, warehouse = created.Warehouse, requestType = created.RequestType, requestNo = created.RequestNo, requestSrl = created.RequestSrl }, created);
    }

    [HttpPut("{fran}/{branch}/{warehouse}/{requestType}/{requestNo}/{requestSrl}")]
    public async Task<ActionResult<RequestDetail>> Update(string fran, string branch, string warehouse, string requestType, string requestNo, string requestSrl, [FromBody] RequestDetail entity, CancellationToken ct)
    {
        // Ensure the key matches
        entity.Fran = fran;
        entity.Branch = branch;
        entity.Warehouse = warehouse;
        entity.RequestType = requestType;
        entity.RequestNo = requestNo;
        entity.RequestSrl = requestSrl;

        var updated = await _service.UpdateDetailAsync(entity, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{fran}/{branch}/{warehouse}/{requestType}/{requestNo}/{requestSrl}")]
    public async Task<IActionResult> Delete(string fran, string branch, string warehouse, string requestType, string requestNo, string requestSrl, CancellationToken ct)
        => await _service.DeleteDetailAsync(fran, branch, warehouse, requestType, requestNo, requestSrl, ct) ? NoContent() : NotFound();
}
