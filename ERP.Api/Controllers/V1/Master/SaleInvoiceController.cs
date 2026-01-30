using Asp.Versioning;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers.V1.Master;

[ApiController]
[ApiVersion("1.0")]
[Route("api")]
#if DEBUG
#endif
public class SaleInvoiceController(ISaleInvoiceService svc) : ControllerBase
{


    // Author: Sripriya, Date:1-1-2026, Purpose: Saleincoice save and Params get
    [HttpPost("SaveSaleInvoice")]
    public async Task<IActionResult> SaveSaleInvoice([FromBody] SaleInvoiceDtos input)
    {
        // 1. Null Validation
        if (input == null)
        {
            return BadRequest(new
            {
                code = 0,
                message = "Error",
                data = new
                {
                    errorMessage = "Invalid request payload!",
                    errorPath = "SalesCoreAPI/SaveSaleInvoice"
                }

            });
        }

        // 2. Call Repository
        int result = await svc.SaveSaleInvoiceAsync(input);

        // 3. If Failed
        if (result == 0)
        {
            return BadRequest(new
            {
                code = 0,
                message = "Error",
                data = new
                {
                    errorMessage = "Sale invoice save failed!",
                    errorPath = "SalesCoreAPI/SaveSaleInvoice"
                }

            });
        }

        return Ok(new
        {
            code = 1,
            message = "Success",
            data = new
            {
                SaleInvoiceStatus = result
            }
        });
    }


    [HttpPost("PARMSGET")]
    public async Task<IActionResult> getparams([FromBody] ParamRequest input)
    {


        if (input == null)
        {
            return BadRequest(new
            {
                code = 0,
                message = "Error",
                data = new
                {
                    errorMessage = "Invalid request payload!",
                    errorPath = "SalesCoreAPI/PARMSGET"
                }

            });
        }
        var result = await svc.GetParams(input);

        // 3. If Failed
        if (result == null)
        {
            return BadRequest(new
            {
                code = 0,
                message = "Error",
                data = new
                {
                    errorMessage = "Sale invoice save failed!",
                    errorPath = "SalesCoreAPI/SaveSaleInvoice"
                }

            });
        }
        else
        {
            return Ok(new
            {
                code = 1,
                message = "Success",
                data = result

            });
        }
    }

}
