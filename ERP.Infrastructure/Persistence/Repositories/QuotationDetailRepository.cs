using Microsoft.Extensions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ERP.Infrastructure.Persistence;

namespace ERP.Infrastructure.Persistence.Repositories;

public sealed class QuotationDetailRepository(ErpDbContext db, ILogger<QuotationDetailRepository> log) : IQuotationDetailRepository
{
    public async Task<QuotationDetail?> GetDetailByKeyAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, string quotSrl, CancellationToken ct)
    {
        return await db.QuotationDetails.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.QuotType == quotType && x.QuotationNo == quotationNo && x.QuotSrl == quotSrl, ct);
    }

    public async Task<IReadOnlyList<QuotationDetail>> GetAllDetailsAsync(CancellationToken ct)
    {
        return await db.QuotationDetails.AsNoTracking().OrderBy(x => x.Fran).ThenBy(x => x.QuotationNo).ThenBy(x => x.QuotSrl).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<QuotationDetail>> GetDetailsByHeaderAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct)
    {
        return await db.QuotationDetails.AsNoTracking()
            .Where(x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.QuotType == quotType && x.QuotationNo == quotationNo)
            .OrderBy(x => x.QuotSrl).ToListAsync(ct);
    }

    public async Task<QuotationDetail> AddDetailAsync(QuotationDetail entity, CancellationToken ct)
    {
        await db.QuotationDetails.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        log.LogInformation("Inserted QuotationDetail {Fran}/{Branch}/{Warehouse}/{QuotType}/{QuotationNo}/{QuotSrl}", entity.Fran, entity.Branch, entity.Warehouse, entity.QuotType, entity.QuotationNo, entity.QuotSrl);
        return entity;
    }

    public async Task<QuotationDetail?> UpdateDetailAsync(QuotationDetail entity, CancellationToken ct)
    {
        var existing = await db.QuotationDetails.FirstOrDefaultAsync(
            x => x.Fran == entity.Fran && x.Branch == entity.Branch && x.Warehouse == entity.Warehouse && x.QuotType == entity.QuotType && x.QuotationNo == entity.QuotationNo && x.QuotSrl == entity.QuotSrl, ct);
        if (existing is null) return null;

        existing.QuotationDate = entity.QuotationDate;
        // WorkId is ignored (stored in QUOTATIONSRL instead)
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

    public async Task<bool> DeleteDetailAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, string quotSrl, CancellationToken ct)
    {
        var existing = await db.QuotationDetails.FirstOrDefaultAsync(
            x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.QuotType == quotType && x.QuotationNo == quotationNo && x.QuotSrl == quotSrl, ct);
        if (existing is null) return false;

        db.QuotationDetails.Remove(existing);
        await db.SaveChangesAsync(ct);
        log.LogInformation("Deleted QuotationDetail {Fran}/{Branch}/{Warehouse}/{QuotType}/{QuotationNo}/{QuotSrl}", fran, branch, warehouse, quotType, quotationNo, quotSrl);
        return true;
    }
}
