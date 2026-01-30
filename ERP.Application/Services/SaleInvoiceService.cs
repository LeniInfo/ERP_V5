using ERP.Application.Abstractions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Master;
using ERP.Domain.Entities;

namespace ERP.Application.Services;

public sealed class SaleInvoiceService : ISaleInvoiceService
{
    //warning changes(02-01-2026)

    private readonly ISaleInvoiceRepository _repo;
    private readonly IAppLogger<SaleInvoiceService> _log;

    public SaleInvoiceService(
        ISaleInvoiceRepository repo,
        IAppLogger<SaleInvoiceService> log)
    {
        _repo = repo;
        _log = log;
    }

    public async Task<int> SaveSaleInvoiceAsync(SaleInvoiceDtos request)
        => await _repo.SaveSaleInvoiceAsync(request);

    //warning changes(02-01-2026)

    //public async Task<List<paramres>> GetParams(paramreq request)
    //=> await _repo.GetParams(request);

    public async Task<List<ParamResponse>> GetParams(ParamRequest request)
        => await _repo.GetParams(request);

    //private readonly ISaleInvoiceRepository _repo = repo;
    //private readonly IAppLogger<SaleInvoiceService> _log = log;

    //public async Task<int> SaveSaleInvoiceAsync(SaleInvoiceDtos request)
    //{
    //    int result = await repo.SaveSaleInvoiceAsync(request);
    //    return result;
    //}
    //public async Task<List<paramres>> GetParams(paramreq request)
    //{
    //    List<paramres> result = await repo.GetParams(request);
    //    return result;
    //}
}
