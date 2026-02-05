using Microsoft.Extensions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Persistence.Repositories;

/// <summary>EF Core repository for Saas Customers.</summary>
public sealed class SaasCustomerRepository(ErpDbContext db, ILogger<SaasCustomerRepository> log) : ISaasCustomerRepository
{
    private readonly ErpDbContext _db = db;
    private readonly ILogger<SaasCustomerRepository> _log = log;

    public async Task<SaasCustomer?> GetByIdAsync(string saasCustomerId, CancellationToken ct)
    {
        return await _db.SaasCustomer
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SaasCustomerId == saasCustomerId, ct);
    }

    public async Task<IReadOnlyList<SaasCustomer>> GetAllAsync(CancellationToken ct)
    {
        return await _db.SaasCustomer
            .AsNoTracking()
            .OrderBy(s => s.SaasCustomerId)
            .ToListAsync(ct);
    }

    public async Task<SaasCustomer> AddAsync(SaasCustomer customer, CancellationToken ct)
    {
        await _db.SaasCustomer.AddAsync(customer, ct);
        await _db.SaveChangesAsync(ct);

        _log.LogInformation("Inserted SaasCustomer {SaasCustomerId}", customer.SaasCustomerId);

        return customer;
    }

    public async Task<SaasCustomer?> UpdateAsync(SaasCustomer customer, CancellationToken ct)
    {
        var existing = await _db.SaasCustomer
            .FirstOrDefaultAsync(s => s.SaasCustomerId == customer.SaasCustomerId, ct);

        if (existing is null)
        {
            _log.LogWarning("Attempted update of non-existent SaasCustomer {SaasCustomerId}", customer.SaasCustomerId);
            return null;
        }

        existing.SaasCustomerName = customer.SaasCustomerName;
        existing.Phone1 = customer.Phone1;
        existing.Phone2 = customer.Phone2;
        existing.Email = customer.Email;
        existing.Address = customer.Address;

        existing.UpdateDt = customer.UpdateDt;
        existing.UpdateTm = customer.UpdateTm;
        existing.UpdateBy = customer.UpdateBy;
        existing.UpdateMarks = customer.UpdateMarks;

        await _db.SaveChangesAsync(ct);

        _log.LogInformation("Updated SaasCustomer {SaasCustomerId}", existing.SaasCustomerId);

        return existing;
    }

    public async Task<bool> DeleteAsync(string saasCustomerId, CancellationToken ct)
    {
        var existing = await _db.SaasCustomer
            .FirstOrDefaultAsync(s => s.SaasCustomerId == saasCustomerId, ct);

        if (existing is null)
        {
            _log.LogWarning("Attempted delete of non-existent SaasCustomer {SaasCustomerId}", saasCustomerId);
            return false;
        }

        _db.SaasCustomer.Remove(existing);
        await _db.SaveChangesAsync(ct);

        _log.LogInformation("Deleted SaasCustomer {SaasCustomerId}", saasCustomerId);

        return true;
    }
}
