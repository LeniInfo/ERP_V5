using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

public interface IRequestHeaderRepository
{
    Task<RequestHeader?> GetByKeyAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct);
    Task<IReadOnlyList<RequestHeader>> GetAllAsync(CancellationToken ct);
    Task<RequestHeader> AddAsync(RequestHeader entity, CancellationToken ct);
    Task<RequestHeader?> UpdateAsync(RequestHeader entity, CancellationToken ct);
    Task<bool> DeleteAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct);
    Task<string> GetNextRequestNumberAsync(CancellationToken ct);
}
