using ERP.Application.Interfaces.Repositories;
using ERP.Contracts.Master;
using ERP.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Persistence.Repositories;

/// <summary>EF Core repository for Parts.</summary>
public sealed class PartsRepository(ErpDbContext db, ILogger<PartsRepository> log) : IPartsRepository
{
    private readonly ErpDbContext _db = db;
    private readonly ILogger<PartsRepository> _log = log;

    public async Task<Part?> GetByCodeAsync(string partCode, CancellationToken ct)
    {
        return await _db.Parts.AsNoTracking().FirstOrDefaultAsync(p => p.PartCode == partCode, ct);
    }

    public async Task<IReadOnlyList<Part>> GetAllAsync(CancellationToken ct)
    {
        return await _db.Parts.AsNoTracking().OrderBy(p => p.PartCode).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Part>> SearchByNameAsync(string name, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return await GetAllAsync(ct);
        }

        var searchTerm = name.Trim().ToLower();

        return await _db.Parts
            .AsNoTracking()
            .Where(p =>
                (p.PartCode != null && p.PartCode.ToLower().Contains(searchTerm)) ||
                (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                (p.Make != null && p.Make.ToLower().Contains(searchTerm)) ||
                (p.Barcode != null && p.Barcode.ToLower().Contains(searchTerm)) ||
                (p.Category != null && p.Category.ToLower().Contains(searchTerm))
            )
            .OrderBy(p => p.PartCode)
            .Take(100) // Limit to 100 results for performance
            .ToListAsync(ct);
    }

    // Added: Added to store parts
    // Added by: Vaishnavi
    // Added on: 10-12-2025

    public async Task<int> AddPartByStoredProcAsync(PartRequests req, CancellationToken ct)
    {
        var sql = @"
        EXEC [dbo].[SP_ADD_PART]
        @FRAN = @FRAN,
        @MAKE = @MAKE,
        @PART = @PART,
        @DESC = @DESC,
        @INVCLASS = @INVCLASS,
        @CATEGORY = @CATEGORY,
        @GROUP = @GROUP,
        @COO = @COO";

        var parameters = new[]
        {
            new SqlParameter("@FRAN", req.Fran),
            new SqlParameter("@MAKE", req.Make),
            new SqlParameter("@PART", req.PartCode),
            new SqlParameter("@DESC", (object?)req.Description ?? DBNull.Value),
            new SqlParameter("@INVCLASS", req.InvClass),
            new SqlParameter("@CATEGORY", req.Category),
            new SqlParameter("@GROUP", req.Group),
            new SqlParameter("@COO", req.Coo)
        };

        await _db.Database.ExecuteSqlRawAsync(sql, parameters, ct);

        _log.LogInformation("SP_ADD_PART executed for Part: {Part}", req.PartCode);

        return 1; // Success indicator
    }

    // Added: Added to get parts
    // Added by: Vaishnavi
    // Added on: 12-12-2025
    public async Task<IReadOnlyList<Part>> GetAllPartsByStoredProcAsync(CancellationToken ct)
    {
        const string sql = "EXEC [dbo].[SP_GET_PART]";

        var parts = await _db.Parts
            .FromSqlRaw(sql)
            .AsNoTracking()
            .ToListAsync(ct);

        _log.LogInformation("SP_GET_PART executed: Retrieved {Count} parts", parts.Count);

        return parts;
    }

}