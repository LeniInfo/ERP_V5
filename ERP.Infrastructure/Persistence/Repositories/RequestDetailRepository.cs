using Microsoft.Extensions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ERP.Infrastructure.Persistence;

namespace ERP.Infrastructure.Persistence.Repositories;

public sealed class RequestDetailRepository(ErpDbContext db, ILogger<RequestDetailRepository> log) : IRequestDetailRepository
{
    public async Task<RequestDetail?> GetDetailByKeyAsync(string fran, string branch, string warehouse, string requestType, string requestNo, string requestSrl, CancellationToken ct)
    {
        return await db.RequestDetails.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.RequestType == requestType && x.RequestNo == requestNo && x.RequestSrl == requestSrl, ct);
    }

    public async Task<IReadOnlyList<RequestDetail>> GetAllDetailsAsync(CancellationToken ct)
    {
        return await db.RequestDetails.AsNoTracking().OrderBy(x => x.Fran).ThenBy(x => x.RequestNo).ThenBy(x => x.RequestSrl).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<RequestDetail>> GetDetailsByHeaderAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct)
    {
        return await db.RequestDetails.AsNoTracking()
            .Where(x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.RequestType == requestType && x.RequestNo == requestNo)
            .OrderBy(x => x.RequestSrl).ToListAsync(ct);
    }

    public async Task<RequestDetail> AddDetailAsync(RequestDetail entity, CancellationToken ct)
    {
        await db.RequestDetails.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        log.LogInformation("Inserted RequestDetail {Fran}/{Branch}/{Warehouse}/{RequestType}/{RequestNo}/{RequestSrl}", entity.Fran, entity.Branch, entity.Warehouse, entity.RequestType, entity.RequestNo, entity.RequestSrl);
        return entity;
    }

    public async Task<RequestDetail?> UpdateDetailAsync(RequestDetail entity, CancellationToken ct)
    {
        var existing = await db.RequestDetails.FirstOrDefaultAsync(
            x => x.Fran == entity.Fran && x.Branch == entity.Branch && x.Warehouse == entity.Warehouse && x.RequestType == entity.RequestType && x.RequestNo == entity.RequestNo && x.RequestSrl == entity.RequestSrl, ct);
        if (existing is null) return null;

        existing.RequestDate = entity.RequestDate;
        // WorkId is ignored (stored in REQUESTSRL instead)
        existing.Make = entity.Make;
        existing.Part = entity.Part;
        existing.Qty = entity.Qty;
        existing.UnitPrice = entity.UnitPrice;
        existing.Discount = entity.Discount;
        existing.VatPercentage = entity.VatPercentage;
        existing.VatValue = entity.VatValue;
        existing.DiscountValue = entity.DiscountValue;
        existing.TotalValue = entity.TotalValue;
        existing.UpdateDt = entity.UpdateDt;
        existing.UpdateTm = entity.UpdateTm;
        existing.UpdateBy = entity.UpdateBy;
        existing.UpdateRemarks = entity.UpdateRemarks;

        await db.SaveChangesAsync(ct);
        return existing;
    }

    public async Task<bool> DeleteDetailAsync(string fran, string branch, string warehouse, string requestType, string requestNo, string requestSrl, CancellationToken ct)
    {
        var existing = await db.RequestDetails.FirstOrDefaultAsync(
            x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.RequestType == requestType && x.RequestNo == requestNo && x.RequestSrl == requestSrl, ct);
        if (existing is null) return false;

        db.RequestDetails.Remove(existing);
        await db.SaveChangesAsync(ct);
        log.LogInformation("Deleted RequestDetail {Fran}/{Branch}/{Warehouse}/{RequestType}/{RequestNo}/{RequestSrl}", fran, branch, warehouse, requestType, requestNo, requestSrl);
        return true;
    }
}
