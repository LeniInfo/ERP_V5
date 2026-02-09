using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

public interface IQuotationHeaderRepository
{
    Task<QuotationHeader?> GetByKeyAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct);
    Task<IReadOnlyList<QuotationHeader>> GetAllAsync(CancellationToken ct);
    Task<QuotationHeader> AddAsync(QuotationHeader entity, CancellationToken ct);
    Task<QuotationHeader?> UpdateAsync(QuotationHeader entity, CancellationToken ct);
    Task<bool> DeleteAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct);
    Task<string> GetNextQuotationNumberAsync(CancellationToken ct);
}
