using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

public interface IRequestDetailRepository
{
    Task<RequestDetail?> GetDetailByKeyAsync(string fran, string branch, string warehouse, string requestType, string requestNo, string requestSrl, CancellationToken ct);
    Task<IReadOnlyList<RequestDetail>> GetAllDetailsAsync(CancellationToken ct);
    Task<IReadOnlyList<RequestDetail>> GetDetailsByHeaderAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct);
    Task<RequestDetail> AddDetailAsync(RequestDetail entity, CancellationToken ct);
    Task<RequestDetail?> UpdateDetailAsync(RequestDetail entity, CancellationToken ct);
    Task<bool> DeleteDetailAsync(string fran, string branch, string warehouse, string requestType, string requestNo, string requestSrl, CancellationToken ct);
}
