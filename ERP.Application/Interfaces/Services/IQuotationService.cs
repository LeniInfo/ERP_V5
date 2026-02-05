using ERP.Contracts.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.Interfaces.Services
{
    public interface IQuotationService
    {
        Task<List<QuoteRes>> SaveQuotation(QuotationReq request);
    }
}
