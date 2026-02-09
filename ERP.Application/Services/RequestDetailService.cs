using ERP.Application.Interfaces.Repositories;
using ERP.Application.Interfaces.Services;
using ERP.Domain.Entities;

namespace ERP.Application.Services;

public sealed class RequestDetailService(IRequestDetailRepository repo) : IRequestDetailService
{
    public async Task<RequestDetail?> GetDetailByKeyAsync(string fran, string branch, string warehouse, string requestType, string requestNo, string requestSrl, CancellationToken ct)
        => await repo.GetDetailByKeyAsync(fran, branch, warehouse, requestType, requestNo, requestSrl, ct);

    public async Task<IReadOnlyList<RequestDetail>> GetAllDetailsAsync(CancellationToken ct)
        => await repo.GetAllDetailsAsync(ct);

    public async Task<IReadOnlyList<RequestDetail>> GetDetailsByHeaderAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct)
        => await repo.GetDetailsByHeaderAsync(fran, branch, warehouse, requestType, requestNo, ct);

    public async Task<RequestDetail> CreateDetailAsync(RequestDetail entity, CancellationToken ct)
        => await repo.AddDetailAsync(entity, ct);

    public async Task<RequestDetail?> UpdateDetailAsync(RequestDetail entity, CancellationToken ct)
        => await repo.UpdateDetailAsync(entity, ct);

    public async Task<bool> DeleteDetailAsync(string fran, string branch, string warehouse, string requestType, string requestNo, string requestSrl, CancellationToken ct)
        => await repo.DeleteDetailAsync(fran, branch, warehouse, requestType, requestNo, requestSrl, ct);
}
