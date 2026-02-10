using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Orders;
using ERP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WorkInvHdrController : ControllerBase
    {
        private readonly IWorkInvHdrService _service;

        public WorkInvHdrController(IWorkInvHdrService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{fran}/{brch}/{workshop}/{workInvType}/{workInvNo}")]
        public async Task<IActionResult> GetByIdAsync(string fran, string brch, string workshop, string workInvType, string workInvNo)
        {
            var result = await _service.GetByIdAsync(fran, brch, workshop, workInvType, workInvNo);

            if (result.Header == null)
                return NotFound(new { message = "Repair order not found" });

            return Ok(new
            {
                header = result.Header,
                details = result.Details
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkInvoiceCreateDto dto)
        {
            await _service.AddAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Repair order saved successfully"
            });
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WorkInvoiceUpdateDto dto)
        {
            await _service.UpdateAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Repair order updated successfully"
            });
        }


        [HttpDelete("{fran}/{brch}/{workshop}/{workInvType}/{workInvNo}")]
        public async Task<IActionResult> Delete(string fran, string brch, string workshop, string workInvType, string workInvNo)
        {
            await _service.DeleteAsync(fran, brch, workshop, workInvType, workInvNo);

            return Ok(new
            {
                success = true,
                message = "Repair order deleted successfully"
            });
        }
    }
}

