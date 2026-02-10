using ERP.Application.Interfaces.Repositories;
using ERP.Application.Interfaces.Services;
using ERP.Domain.Entities;

namespace ERP.Application.Services;

public sealed class QuotationDetailService(IQuotationDetailRepository repo) : IQuotationDetailService
{
    public async Task<QuotationDetail?> GetDetailByKeyAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, string quotSrl, CancellationToken ct)
        => await repo.GetDetailByKeyAsync(fran, branch, warehouse, quotType, quotationNo, quotSrl, ct);

    public async Task<IReadOnlyList<QuotationDetail>> GetAllDetailsAsync(CancellationToken ct)
        => await repo.GetAllDetailsAsync(ct);

    public async Task<IReadOnlyList<QuotationDetail>> GetDetailsByHeaderAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct)
        => await repo.GetDetailsByHeaderAsync(fran, branch, warehouse, quotType, quotationNo, ct);

    public async Task<QuotationDetail> CreateDetailAsync(QuotationDetail entity, CancellationToken ct)
        => await repo.AddDetailAsync(entity, ct);

    public async Task<QuotationDetail?> UpdateDetailAsync(QuotationDetail entity, CancellationToken ct)
        => await repo.UpdateDetailAsync(entity, ct);

    public async Task<bool> DeleteDetailAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, string quotSrl, CancellationToken ct)
        => await repo.DeleteDetailAsync(fran, branch, warehouse, quotType, quotationNo, quotSrl, ct);
}
