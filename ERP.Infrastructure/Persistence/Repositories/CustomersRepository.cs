using ERP.Application.Interfaces.Repositories;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Persistence.Repositories;

public sealed class CustomersRepository : ICustomersRepository
{
    private readonly ErpDbContext _db;
    public CustomersRepository(ErpDbContext db) => _db = db;

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken ct)
        => await _db.Set<Customer>().AsNoTracking().OrderBy(x => x.CustomerCode).ToListAsync(ct);

    public async Task<Customer?> GetByCodeAsync(string customerCode, CancellationToken ct)
        => await _db.Set<Customer>().AsNoTracking().FirstOrDefaultAsync(x => x.CustomerCode == customerCode, ct);

    public async Task<IReadOnlyList<Customer>> SearchByNameAsync(string name, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return await GetAllAsync(ct);
        }

        var searchTerm = name.Trim().ToLower();

        return await _db.Set<Customer>()
            .AsNoTracking()
            .Where(c =>
                (c.CustomerCode != null && c.CustomerCode.ToLower().Contains(searchTerm)) ||
                (c.Name != null && c.Name.ToLower().Contains(searchTerm)) ||
                (c.NameAr != null && c.NameAr.ToLower().Contains(searchTerm)) ||
                (c.Phone != null && c.Phone.Contains(searchTerm)) ||
                (c.Email != null && c.Email.ToLower().Contains(searchTerm)) ||
                (c.Address != null && c.Address.ToLower().Contains(searchTerm)) ||
                (c.VatNo != null && c.VatNo.ToLower().Contains(searchTerm))
            )
            .OrderBy(c => c.CustomerCode)
            .Take(100) // Limit to 100 results for performance
            .ToListAsync(ct);
    }

    public async Task CreateAsync(Customer entity, CancellationToken ct)
    {
        _db.Set<Customer>().Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Customer entity, CancellationToken ct)
    {
        // Fetch the entity with tracking to avoid updating identity columns
        var tracked = await _db.Set<Customer>().FirstOrDefaultAsync(x => x.CustomerCode == entity.CustomerCode, ct);
        if (tracked is null) return;

        // Update only the non-identity properties
        tracked.Name = entity.Name;
        tracked.NameAr = entity.NameAr;
        tracked.Phone = entity.Phone;
        tracked.Email = entity.Email;
        tracked.Address = entity.Address;
        tracked.VatNo = entity.VatNo;
        tracked.UpdateDt = entity.UpdateDt;
        tracked.UpdateTm = entity.UpdateTm;
        tracked.UpdateBy = entity.UpdateBy;
        tracked.UpdateRemarks = entity.UpdateRemarks;

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string customerCode, CancellationToken ct)
    {
        var tracked = await _db.Set<Customer>().FirstOrDefaultAsync(x => x.CustomerCode == customerCode, ct);
        if (tracked is null) return;
        _db.Set<Customer>().Remove(tracked);
        await _db.SaveChangesAsync(ct);
    }
}