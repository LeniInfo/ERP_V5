using Asp.Versioning;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers.V1.Master
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/master/[controller]")]
#if DEBUG
    [AllowAnonymous] // Dev-only: allow anonymous access during development
#endif
    public sealed class MasterTypesController : ControllerBase
    {
        private readonly IMasterTypesService _svc;

        public MasterTypesController(IMasterTypesService svc)
            => _svc = svc;

        /// <summary>List all master records.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MasterTypeDto>>> Get(CancellationToken ct)
        {
            var items = await _svc.GetAllAsync(ct);
            return Ok(items);
        }

        /// <summary>Get a master record by composite key.</summary>
        [HttpGet("{fran}/{masterType}/{masterCode}")]

        public async Task<ActionResult<MasterTypeDto>> GetByKey(
            string fran,
            string masterType,
            string masterCode,
            CancellationToken ct)
        {
            var item = await _svc.GetByKeyAsync(fran, masterType, masterCode, ct);
            if (item is null) return NotFound();

            return Ok(item);
        }

        /// <summary>Create a new master record.</summary>
        [HttpPost]
        public async Task<ActionResult<MasterTypeDto>> Create(
            [FromBody] MasterTypeDto dto,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _svc.CreateAsync(dto, ct);

            return CreatedAtAction(
                nameof(GetByKey),
                new
                {
                    fran = created.Fran,
                    masterType = created.MasterType,
                    masterCode = created.MasterCode,
                    version = HttpContext.GetRequestedApiVersion()!.ToString()
                },
                created);
        }

        /// <summary>Update an existing master record.</summary>
        [HttpPut("{fran}/{masterType}/{masterCode}")]
        public async Task<ActionResult<MasterTypeDto>> Update(
            string fran,
            string masterType,
            string masterCode,
            [FromBody] MasterTypeDto dto,
            CancellationToken ct)
        {
            var updated = await _svc.UpdateAsync(fran, masterType, masterCode, dto, ct);
            if (updated is null) return NotFound();

            return Ok(updated);
        }

        /// <summary>Delete a master record.</summary>
        [HttpDelete("{fran}/{masterType}/{masterCode}")]
        public async Task<IActionResult> Delete(
            string fran,
            string masterType,
            string masterCode,
            CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(fran, masterType, masterCode, ct);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}

