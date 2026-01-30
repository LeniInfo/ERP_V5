using ERP.Contracts.Master;
using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

public interface ISaleInvoiceRepository

{
    public Task<int> SaveSaleInvoiceAsync(SaleInvoiceDtos request);
    //warning changes(02-01-2026)
    Task<List<ParamResponse>> GetParams(ParamRequest request);

    //Task<List<paramres>> GetParams(paramreq request);
}
