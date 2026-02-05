using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Orders;
using ERP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ERP.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RepairOrderController : ControllerBase
    {
        private readonly IRepairOrderService _service;

        public RepairOrderController(IRepairOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{fran}/{brch}/{workshop}/{repairType}/{repairNo}/{customer}")]
        public async Task<IActionResult> GetHdrDet(
    string fran,
    string brch,
    string workshop,
    string repairType,
    string repairNo,
    string customer)
        {
            var result = await _service.GetHdrDetAsync(
                fran,
                brch,
                workshop,
                repairType,
                repairNo,
                customer
            );

            if (result.Header == null)
                return NotFound(new { message = "Repair order not found" });

            return Ok(new
            {
                header = result.Header,
                details = result.Details
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RepairOrderCreateDto dto)
        {
            await _service.AddAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Repair order saved successfully"
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RepairOrderUpdateDto dto)
        {
            await _service.UpdateAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Repair order updated successfully"
            });
        }

        [HttpDelete("{fran}/{customer}/{repairNo}")]
        public async Task<IActionResult> Delete(string fran, string customer, string repairNo)
        {
            await _service.DeleteAsync(fran, customer, repairNo);

            return Ok(new
            {
                success = true,
                message = "Repair order deleted successfully"
            });
        }

        //arabic conversion 
        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslateDto dto)
        {
            try
            {
                using var http = new HttpClient();

                var url =
                    "https://translate.googleapis.com/translate_a/single" +
                    "?client=gtx" +
                    "&sl=" + dto.From +
                    "&tl=" + dto.To +
                    "&dt=t" +
                    "&q=" + Uri.EscapeDataString(dto.Text);

                var response = await http.GetStringAsync(url);

                using var doc = JsonDocument.Parse(response);
                var translatedText = doc
                    .RootElement[0][0][0]
                    .GetString();

                return Ok(new { translatedText });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}

