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
public class QuotationController(IQuotationService svc) : ControllerBase
{


    // Author: Sripriya, Date:1-1-2026, Purpose: Quotation save 
    [HttpPost("SavwQuotation")]
    public async Task<IActionResult> SaveQuote(QuotationReq input)
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
        var result = await svc.SaveQuotation(input);

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
        return Ok(new
        {
            code = 1,
            message = "Success",
            data = new
            {
                QuoteStatus = result
            }
        });
    }


}
