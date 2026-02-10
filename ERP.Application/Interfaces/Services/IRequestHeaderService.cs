using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Services;

public interface IRequestHeaderService
{
    Task<RequestHeader?> GetByKeyAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct);
    Task<IReadOnlyList<RequestHeader>> GetAllAsync(CancellationToken ct);
    Task<RequestHeader> CreateAsync(RequestHeader entity, CancellationToken ct);
    Task<RequestHeader?> UpdateAsync(RequestHeader entity, CancellationToken ct);
    Task<bool> DeleteAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct);
    Task<string> GetNextRequestNumberAsync(CancellationToken ct);
}
