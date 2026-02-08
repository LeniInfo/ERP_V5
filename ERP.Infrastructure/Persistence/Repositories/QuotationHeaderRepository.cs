using System;
using Microsoft.Extensions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Persistence.Repositories;

public sealed class QuotationHeaderRepository(ErpDbContext db, ILogger<QuotationHeaderRepository> log) : IQuotationHeaderRepository
{
    public async Task<QuotationHeader?> GetByKeyAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct)
    {
        return await db.QuotationHeaders.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.QuotType == quotType && x.QuotationNo == quotationNo, ct);
    }

    public async Task<IReadOnlyList<QuotationHeader>> GetAllAsync(CancellationToken ct)
    {
        return await db.QuotationHeaders.AsNoTracking().OrderBy(x => x.Fran).ThenBy(x => x.QuotationNo).ToListAsync(ct);
    }

    public async Task<QuotationHeader> AddAsync(QuotationHeader entity, CancellationToken ct)
    {
        await db.QuotationHeaders.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        log.LogInformation("Inserted QuotationHeader {Fran}/{Branch}/{Warehouse}/{QuotType}/{QuotationNo}", entity.Fran, entity.Branch, entity.Warehouse, entity.QuotType, entity.QuotationNo);
        return entity;
    }

    public async Task<QuotationHeader?> UpdateAsync(QuotationHeader entity, CancellationToken ct)
    {
        var existing = await db.QuotationHeaders.FirstOrDefaultAsync(
            x => x.Fran == entity.Fran && x.Branch == entity.Branch && x.Warehouse == entity.Warehouse && x.QuotType == entity.QuotType && x.QuotationNo == entity.QuotationNo, ct);
        if (existing is null) return null;

        existing.QuotationDate = entity.QuotationDate;
        existing.Customer = entity.Customer;
        existing.QuotationSource = entity.QuotationSource;
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

    public async Task<bool> DeleteAsync(string fran, string branch, string warehouse, string quotType, string quotationNo, CancellationToken ct)
    {
        var existing = await db.QuotationHeaders.FirstOrDefaultAsync(
            x => x.Fran == fran && x.Branch == branch && x.Warehouse == warehouse && x.QuotType == quotType && x.QuotationNo == quotationNo, ct);
        if (existing is null) return false;

        db.QuotationHeaders.Remove(existing);
        await db.SaveChangesAsync(ct);
        log.LogInformation("Deleted QuotationHeader {Fran}/{Branch}/{Warehouse}/{QuotType}/{QuotationNo}", fran, branch, warehouse, quotType, quotationNo);
        return true;
    }

    public async Task<string> GetNextQuotationNumberAsync(CancellationToken ct)
    {
        // Get all quotation numbers (filter in memory to avoid EF translation issues)
        var allQuotationNos = await db.QuotationHeaders
            .AsNoTracking()
            .Select(h => h.QuotationNo)
            .ToListAsync(ct);

        // Find the highest 4-digit number from existing quotation numbers
        int maxNumber = 0;
        foreach (var quotationNo in allQuotationNos)
        {
            if (!string.IsNullOrEmpty(quotationNo) && 
                quotationNo.Length == 4 && 
                quotationNo.All(char.IsDigit) &&
                int.TryParse(quotationNo, out int num) && 
                num > maxNumber)
            {
                maxNumber = num;
            }
        }

        // Generate next 4-digit quotation number (0001 to 9999)
        int nextNumber = maxNumber + 1;
        if (nextNumber > 9999)
        {
            throw new InvalidOperationException("Maximum quotation number limit reached (9999)");
        }

        return nextNumber.ToString("D4"); // Format as 4-digit string with leading zeros
    }
}
