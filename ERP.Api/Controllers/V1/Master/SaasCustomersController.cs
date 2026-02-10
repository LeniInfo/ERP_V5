using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Master;
using ERP.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ERP.Api.Controllers.V1.Master
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/master/[controller]")]
#if DEBUG
    [AllowAnonymous] // Dev-only
#endif
    public sealed class SaasCustomersController : ControllerBase
    {
        private readonly ISaasCustomerService _svc;
        private readonly ErpDbContext _context;

        public SaasCustomersController(ISaasCustomerService svc, ErpDbContext context)
        {
            _svc = svc;
            _context = context;
        }

        /// <summary>List all SaaS customers.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaasCustomerDto>>> Get(CancellationToken ct)
        {
            var items = await _svc.GetAllAsync(ct);
            return Ok(items);
        }

        /// <summary>Get a SaaS customer by ID.</summary>
        [HttpGet("{saasCustomerId}")]
        public async Task<ActionResult<SaasCustomerDto>> GetById(string saasCustomerId, CancellationToken ct)
        {
            var item = await _svc.GetByIdAsync(saasCustomerId, ct);
            if (item is null)
                return NotFound();

            return Ok(item);
        }

        /// <summary>Create a new SaaS customer.</summary>
        /// 
        [HttpGet("next-id")]
        public async Task<ActionResult<string>> GetNextSaasCustomerId()
        {
            var lastId = await _context.SaasCustomer
                .OrderByDescending(x => x.SaasCustomerId)
                .Select(x => x.SaasCustomerId)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(lastId))
                return "A";

            char current = char.ToUpperInvariant(lastId[0]);
            char nextChar = (char)(current + 1);

            return nextChar.ToString();
        }

        [HttpPost]
        public async Task<ActionResult<SaasCustomerDto>> Create(
            [FromBody] SaasCustomerDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _svc.CreateAsync(dto, ct);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    saasCustomerId = created.SaasCustomerId,
                    version = HttpContext.GetRequestedApiVersion()!.ToString()
                },
                created
            );
        }

        /// <summary>Update an existing SaaS customer.</summary>
        [HttpPut("{saasCustomerId}")]
        public async Task<ActionResult<SaasCustomerDto>> Update(
            string saasCustomerId,
            [FromBody] SaasCustomerDto dto,
            CancellationToken ct)
        {
            var updated = await _svc.UpdateAsync(saasCustomerId, dto, ct);
            if (updated is null)
                return NotFound();

            return Ok(updated);
        }

        /// <summary>Delete a SaaS customer.</summary>
        [HttpDelete("{saasCustomerId}")]
        public async Task<IActionResult> Delete(string saasCustomerId, CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(saasCustomerId, ct);
            if (!ok)
                return NotFound();

            return NoContent();
        }
    }
}
