using ERP.Application.Interfaces.Repositories;
using ERP.Application.Interfaces.Services;
using ERP.Domain.Entities;

namespace ERP.Application.Services;

public sealed class RequestHeaderService(IRequestHeaderRepository repo) : IRequestHeaderService
{
    public async Task<RequestHeader?> GetByKeyAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct)
        => await repo.GetByKeyAsync(fran, branch, warehouse, requestType, requestNo, ct);

    public async Task<IReadOnlyList<RequestHeader>> GetAllAsync(CancellationToken ct)
        => await repo.GetAllAsync(ct);

    public async Task<RequestHeader> CreateAsync(RequestHeader entity, CancellationToken ct)
        => await repo.AddAsync(entity, ct);

    public async Task<RequestHeader?> UpdateAsync(RequestHeader entity, CancellationToken ct)
        => await repo.UpdateAsync(entity, ct);

    public async Task<bool> DeleteAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct)
        => await repo.DeleteAsync(fran, branch, warehouse, requestType, requestNo, ct);

    public async Task<string> GetNextRequestNumberAsync(CancellationToken ct)
        => await repo.GetNextRequestNumberAsync(ct);
}
