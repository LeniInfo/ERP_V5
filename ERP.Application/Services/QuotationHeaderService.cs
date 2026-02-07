using ERP.Application.Interfaces.Repositories;
using ERP.Application.Interfaces.Services;
using ERP.Domain.Entities;

namespace ERP.Application.Services;

public sealed class QuotationHeaderService(IQuotationHeaderRepository repo) : IQuotationHeaderService
{
    public async Task<QuotationHeader?> GetByKeyAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct)
        => await repo.GetByKeyAsync(fran, branch, warehouse, quotType, quotationNo, ct);

    public async Task<IReadOnlyList<QuotationHeader>> GetAllAsync(CancellationToken ct)
        => await repo.GetAllAsync(ct);

    public async Task<QuotationHeader> CreateAsync(QuotationHeader entity, CancellationToken ct)
        => await repo.AddAsync(entity, ct);

    public async Task<QuotationHeader?> UpdateAsync(QuotationHeader entity, CancellationToken ct)
        => await repo.UpdateAsync(entity, ct);

    public async Task<bool> DeleteAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct)
        => await repo.DeleteAsync(fran, branch, warehouse, quotType, quotationNo, ct);

    public async Task<string> GetNextQuotationNumberAsync(CancellationToken ct)
        => await repo.GetNextQuotationNumberAsync(ct);
}
