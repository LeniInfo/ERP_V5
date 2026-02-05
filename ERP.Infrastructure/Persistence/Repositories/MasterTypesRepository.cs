using Microsoft.Extensions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Persistence.Repositories;

/// <summary>EF Core repository for MasterTypes.</summary>
public sealed class MasterTypesRepository(
    ErpDbContext db,
    ILogger<MasterTypesRepository> log)
    : IMasterTypesRepository
{
    private readonly ErpDbContext _db = db;
    private readonly ILogger<MasterTypesRepository> _log = log;

    public async Task<MasterType?> GetByKeyAsync(
        string fran,
        string masterType,
        string masterCode,
        CancellationToken ct)
    {
        return await _db.MasterTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Fran == fran &&
                x.MasterTypeCode == masterType &&
                x.MasterCode == masterCode,
                ct);
    }

    public async Task<IReadOnlyList<MasterType>> GetAllAsync(CancellationToken ct)
    {
        return await _db.MasterTypes
            .AsNoTracking()
            .OrderBy(x => x.Fran)
            .ThenBy(x => x.MasterTypeCode)
            .ThenBy(x => x.MasterCode)
            .ToListAsync(ct);
    }

    public async Task<MasterType> AddAsync(MasterType entity, CancellationToken ct)
    {
        await _db.MasterTypes.AddAsync(entity, ct);
        await _db.SaveChangesAsync(ct);

        _log.LogInformation(
           "Inserted MasterType {Fran}-{MasterType}-{MasterCode}",
           entity.Fran,
           entity.MasterTypeCode,
           entity.MasterCode);

        return entity;
    }

    public async Task<MasterType?> UpdateAsync(MasterType entity, CancellationToken ct)
    {
        var existing = await _db.MasterTypes.FirstOrDefaultAsync(x =>
            x.Fran == entity.Fran &&
            x.MasterTypeCode == entity.MasterTypeCode &&
            x.MasterCode == entity.MasterCode,
            ct);

        if (existing is null)
        {
            _log.LogWarning(
                "Attempted update of non-existent MasterType {Fran}-{MasterType}-{MasterCode}",
                entity.Fran,
                entity.MasterTypeCode,
                entity.MasterCode);

            return null;
        }

        existing.Name = entity.Name;
        existing.NameAr = entity.NameAr;
        existing.Phone = entity.Phone;
        existing.Email = entity.Email;
        existing.Address = entity.Address;
        existing.VatNo = entity.VatNo;

        existing.SeqNo = entity.SeqNo;
        existing.SeqPrefix = entity.SeqPrefix;

        existing.UpdateDate = entity.UpdateDate;
        existing.UpdateTime = entity.UpdateTime;
        existing.UpdateBy = entity.UpdateBy;
        existing.UpdateRemarks = entity.UpdateRemarks;

        await _db.SaveChangesAsync(ct);

        _log.LogInformation(
            "Updated MasterType {Fran}-{MasterType}-{MasterCode}",
            existing.Fran,
            existing.MasterTypeCode,
            existing.MasterCode);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        string fran,
        string masterType,
        string masterCode,
        CancellationToken ct)
    {
        var existing = await _db.MasterTypes.FirstOrDefaultAsync(x =>
            x.Fran == fran &&
            x.MasterTypeCode == masterType &&
            x.MasterCode == masterCode,
            ct);

        if (existing is null)
        {
            _log.LogWarning(
                "Attempted delete of non-existent MasterType {Fran}-{MasterType}-{MasterCode}",
                fran,
                masterType,
                masterCode);

            return false;
        }

        _db.MasterTypes.Remove(existing);
        await _db.SaveChangesAsync(ct);

        _log.LogInformation(
            "Deleted MasterType {Fran}-{MasterType}-{MasterCode}",
            fran,
            masterType,
            masterCode);

        return true;
    }

    public async Task<string> GetNextSeqAsync(
     string fran,
     string masterType,
     string prefix,
     CancellationToken ct)
    {
        var max = await _db.MasterTypes
            .Where(x => x.Fran == fran
                     && x.MasterTypeCode == masterType
                     && x.SeqNo.StartsWith(prefix))
            .Select(x => x.SeqNo)
            .OrderByDescending(x => x)
            .FirstOrDefaultAsync(ct);

        if (string.IsNullOrEmpty(max))
            return $"{prefix}0000001";

        var number = int.Parse(max.Substring(prefix.Length));
        var next = number + 1;

        return $"{prefix}{next:0000000}";
    }


    public async Task<string> GetNextMasterCodeAsync(
    string fran,
    string masterType,
    string prefix,
    CancellationToken ct)
    {
        // Get the highest matching master code like CT*, ordered descending
        var max = await _db.MasterTypes
            .Where(x => x.Fran == fran
                     && x.MasterTypeCode == masterType
                     && x.MasterCode.StartsWith(prefix))
            .Select(x => x.MasterCode)
            .OrderByDescending(x => x)
            .FirstOrDefaultAsync(ct);

        // First entry
        if (string.IsNullOrEmpty(max))
            return $"{prefix}1";

        // Extract just the number part after prefix
        var numberPart = max.Substring(prefix.Length);

        if (!int.TryParse(numberPart, out int current))
            current = 0;

        // Increment
        int next = current + 1;

        return $"{prefix}{next}";
    }



}
