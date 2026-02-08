using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

public interface IQuotationDetailRepository
{
    Task<QuotationDetail?> GetDetailByKeyAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, string quotSrl, CancellationToken ct);
    Task<IReadOnlyList<QuotationDetail>> GetAllDetailsAsync(CancellationToken ct);
    Task<IReadOnlyList<QuotationDetail>> GetDetailsByHeaderAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct);
    Task<QuotationDetail> AddDetailAsync(QuotationDetail entity, CancellationToken ct);
    Task<QuotationDetail?> UpdateDetailAsync(QuotationDetail entity, CancellationToken ct);
    Task<bool> DeleteDetailAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, string quotSrl, CancellationToken ct);
}
