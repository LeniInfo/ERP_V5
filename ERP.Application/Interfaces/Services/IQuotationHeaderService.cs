using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Services;

public interface IQuotationHeaderService
{
    Task<QuotationHeader?> GetByKeyAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct);
    Task<IReadOnlyList<QuotationHeader>> GetAllAsync(CancellationToken ct);
    Task<QuotationHeader> CreateAsync(QuotationHeader entity, CancellationToken ct);
    Task<QuotationHeader?> UpdateAsync(QuotationHeader entity, CancellationToken ct);
    Task<bool> DeleteAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct);
    Task<string> GetNextQuotationNumberAsync(CancellationToken ct);
}
