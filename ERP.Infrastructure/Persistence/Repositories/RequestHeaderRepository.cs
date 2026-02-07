using System;
using Microsoft.Extensions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Persistence.Repositories;

public sealed class RequestHeaderRepository(ErpDbContext db, ILogger<RequestHeaderRepository> log) : IRequestHeaderRepository
{
    public async Task<RequestHeader?> GetByKeyAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct)
    {
        return await db.RequestHeaders.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.RequestType == requestType && x.RequestNo == requestNo, ct);
    }

    public async Task<IReadOnlyList<RequestHeader>> GetAllAsync(CancellationToken ct)
    {
        return await db.RequestHeaders.AsNoTracking().OrderBy(x => x.Fran).ThenBy(x => x.RequestNo).ToListAsync(ct);
    }

    public async Task<RequestHeader> AddAsync(RequestHeader entity, CancellationToken ct)
    {
        await db.RequestHeaders.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        log.LogInformation("Inserted RequestHeader {Fran}/{Branch}/{Warehouse}/{RequestType}/{RequestNo}", entity.Fran, entity.Branch, entity.Warehouse, entity.RequestType, entity.RequestNo);
        return entity;
    }

    public async Task<RequestHeader?> UpdateAsync(RequestHeader entity, CancellationToken ct)
    {
        var existing = await db.RequestHeaders.FirstOrDefaultAsync(
            x => x.Fran == entity.Fran && x.Branch == entity.Branch && x.Warehouse == entity.Warehouse && x.RequestType == entity.RequestType && x.RequestNo == entity.RequestNo, ct);
        if (existing is null) return null;

        existing.RequestDate = entity.RequestDate;
        existing.Customer = entity.Customer;
        existing.RequestSource = entity.RequestSource;
        existing.RefNo = entity.RefNo;
        existing.RefDate = entity.RefDate;
        existing.SeqNo = entity.SeqNo;
        existing.SeqPrefix = entity.SeqPrefix;
        existing.Currency = entity.Currency;
        existing.NoOfItems = entity.NoOfItems;
        existing.Discount = entity.Discount;
        existing.VatValue = entity.VatValue;
        existing.TotalValue = entity.TotalValue;
        existing.UpdateDt = entity.UpdateDt;
        existing.UpdateTm = entity.UpdateTm;
        existing.UpdateBy = entity.UpdateBy;
        existing.UpdateRemarks = entity.UpdateRemarks;

        await db.SaveChangesAsync(ct);
        return existing;
    }

    public async Task<bool> DeleteAsync(string fran, string branch, string warehouse, string requestType, string requestNo, CancellationToken ct)
    {
        var existing = await db.RequestHeaders.FirstOrDefaultAsync(
            x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.RequestType == requestType && x.RequestNo == requestNo, ct);
        if (existing is null) return false;

        db.RequestHeaders.Remove(existing);
        await db.SaveChangesAsync(ct);
        log.LogInformation("Deleted RequestHeader {Fran}/{Branch}/{Warehouse}/{RequestType}/{RequestNo}", fran, branch, warehouse, requestType, requestNo);
        return true;
    }

    public async Task<string> GetNextRequestNumberAsync(CancellationToken ct)
    {
        // Get all request numbers (filter in memory to avoid EF translation issues)
        var allRequestNos = await db.RequestHeaders
            .AsNoTracking()
            .Select(h => h.RequestNo)
            .ToListAsync(ct);

        // Find the highest 4-digit number from existing request numbers
        int maxNumber = 0;
        foreach (var requestNo in allRequestNos)
        {
            if (!string.IsNullOrEmpty(requestNo) && 
                requestNo.Length == 4 && 
                requestNo.All(char.IsDigit) &&
                int.TryParse(requestNo, out int num) && 
                num > maxNumber)
            {
                maxNumber = num;
            }
        }

        // Generate next 4-digit request number (0001 to 9999)
        int nextNumber = maxNumber + 1;
        if (nextNumber > 9999)
        {
            throw new InvalidOperationException("Maximum request number limit reached (9999)");
        }

        return nextNumber.ToString("D4"); // Format as 4-digit string with leading zeros
    }
}
