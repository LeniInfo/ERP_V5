using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;

namespace ERP.Api.Controllers.V1.Orders
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/orders/[controller]")]
#if DEBUG
    [AllowAnonymous]
#endif
    public sealed class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrdersService _svc;
        public PurchaseOrdersController(IPurchaseOrdersService svc) => _svc = svc;

        /// <summary>List all purchase order headers.</summary>
        [HttpGet]
        //[Authorize(Policy = "erp.orders.read")]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> Get(CancellationToken ct)
        {
            var items = await _svc.GetAllAsync(ct);
            return Ok(items);
        }

        /// <summary>Get a purchase order (with lines) by composite key.</summary>
        [HttpGet("{fran}/{branch}/{warehouseCode}/{poType}/{poNumber}")]
        public async Task<ActionResult<PurchaseOrderDto>> GetByKey(string fran, string branch, string warehouseCode, string poType, string poNumber, CancellationToken ct)
        {
            var item = await _svc.GetByKeyAsync(fran, branch, warehouseCode, poType, poNumber, ct);
            if (item is null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseOrderDto>> Create([FromBody] PurchaseOrderDto dto, CancellationToken ct)
        {
            var success = await _svc.CreateAsync(dto, ct);
            if (!success)
                return BadRequest("Failed to create Purchase Order");

            return Ok(new { message = "Purchase Order created successfully", poNumber = dto.PoNumber });
        }

        [HttpPut("{fran}/{pono}/{supplier}")]
        public async Task<IActionResult> UpdateAsync(
     string fran,
     string pono,
     string supplier,
     [FromBody] PurchaseOrderDto dto,
     CancellationToken ct)
        {
            var updated = await _svc.UpdateAsync(fran, pono, supplier, dto, ct);

            if (!updated)
                return BadRequest(new { result = "Failed", message = "Update failed" });

            return Ok(new { result = "Success", message = "Updated Successfully" });
        }


        [HttpDelete("{fran}/{pono}")]
        public async Task<IActionResult> Delete(
            string fran,
            string pono,
            CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(fran, pono, ct);
            return ok ? NoContent() : NotFound();
        }

    }
}