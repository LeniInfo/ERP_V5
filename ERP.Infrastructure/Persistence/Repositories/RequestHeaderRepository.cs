using System;
using System.Linq;
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
        try
        {
            // Get all request numbers (filter in memory to avoid EF translation issues)
            var allRequestNos = await db.RequestHeaders
                .AsNoTracking()
                .Select(h => h.RequestNo)
                .ToListAsync(ct);

            log.LogInformation("Retrieved {Count} request numbers from database", allRequestNos.Count);

            // Find the highest numeric number from existing request numbers
            // Try to parse any numeric part of the request number
            int maxNumber = 0;
            foreach (var requestNo in allRequestNos)
            {
                if (string.IsNullOrEmpty(requestNo))
                    continue;

                // Try to extract numeric part (handle formats like "0001", "REQ001", etc.)
                string numericPart = new string(requestNo.Where(char.IsDigit).ToArray());
                
                if (!string.IsNullOrEmpty(numericPart) && 
                    int.TryParse(numericPart, out int num) && 
                    num > maxNumber)
                {
                    maxNumber = num;
                }
            }

            // Generate next request number starting from 0001
            int nextNumber = maxNumber + 1;
            if (nextNumber > 9999)
            {
                log.LogWarning("Request number exceeded 9999, resetting to 0001");
                nextNumber = 1; // Reset to 1 instead of throwing error
            }

            string nextRequestNo = nextNumber.ToString("D4"); // Format as 4-digit string with leading zeros
            log.LogInformation("Generated next request number: {RequestNo}", nextRequestNo);
            
            return nextRequestNo;
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error generating next request number");
            // Return a default number if there's an error (e.g., database connection issue)
            // This allows the system to continue working even if there's a temporary issue
            return "0001";
        }
    }
}
