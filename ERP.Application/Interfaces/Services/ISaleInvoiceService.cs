using ERP.Contracts.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.Interfaces.Services
{
    public interface ISaleInvoiceService
    {
        Task<int> SaveSaleInvoiceAsync(SaleInvoiceDtos request);
        Task<List<ParamResponse>> GetParams(ParamRequest request);
        Task<List<workmasRes>> getworkmas(WorkmasReq request);
        //warning changes (02-01-2026)
        //Task<List<paramres>> GetParams(paramreq request);
    }
}
