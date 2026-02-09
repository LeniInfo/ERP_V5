using ERP.Application.Interfaces.Repositories;
using ERP.Contracts.Finance;
using ERP.Contracts.Master;
using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;



namespace ERP.Infrastructure.Persistence.Repositories;

public sealed class FranchisesRepository(ErpDbContext db) : IFranchisesRepository
{
    private readonly ErpDbContext _db = db;

    public async Task<IReadOnlyList<FranchiseDto>> GetAllAsync(CancellationToken ct)
    {
        return await (
            from f in _db.Franchises.AsNoTracking()
            join p in _db.Params.AsNoTracking()
                on new { Val = f.NatureOfBusiness, Type = "FRAN_NATUREOFBUSINESS" }
                equals new { Val = p.ParamValue, Type = p.ParamType }
                into pg
            from p in pg.DefaultIfEmpty()   // 👈 LEFT JOIN
            orderby f.Fran
            select new FranchiseDto
            {
                Fran = f.Fran,
                Name = f.Name,
                NameAr = f.NameAr,

                CustomerCurrency = f.CustomerCurrency,

                ParamValue = f.NatureOfBusiness,
                ParamDesc = p != null ? p.ParamDesc : "",

                VatEnabled = f.VatEnabled == "Y"
            }
        ).ToListAsync(ct);
    }


    public async Task<FranchiseDto?> GetByKeyAsync(string fran, CancellationToken ct)
    {
        return await (
            from f in _db.Franchises.AsNoTracking()
            join p in _db.Params.AsNoTracking()
                on new { Val = f.NatureOfBusiness, Type = "FRAN_NATUREOFBUSINESS" }
                equals new { Val = p.ParamValue, Type = p.ParamType }
                into pg
            from p in pg.DefaultIfEmpty()
            where f.Fran == fran
            select new FranchiseDto
            {
                Fran = f.Fran,
                Name = f.Name,
                NameAr = f.NameAr,

                CustomerCurrency = f.CustomerCurrency,

                ParamValue = f.NatureOfBusiness,
                ParamDesc = p != null ? p.ParamDesc : "",

                VatEnabled = f.VatEnabled == "Y"
            }
        ).FirstOrDefaultAsync(ct);
    }

    public async Task<Franchise> AddAsync(Franchise entity, CancellationToken ct)
    {
        entity.VatEnabled = entity.VatEnabled == "Y" ? "Y" : "N";
        await _db.Franchises.AddAsync(entity, ct);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<Franchise?> UpdateAsync(Franchise entity, CancellationToken ct)
    {
        var existing = await _db.Franchises.FirstOrDefaultAsync(f => f.Fran == entity.Fran, ct);
        if (existing is null) return null;
        existing.Name = string.IsNullOrWhiteSpace(entity.Name) ? existing.Name : entity.Name;
        existing.NameAr = string.IsNullOrWhiteSpace(entity.NameAr) ? existing.NameAr : entity.NameAr;
        existing.CustomerCurrency = entity.CustomerCurrency;
        existing.NatureOfBusiness = entity.NatureOfBusiness;
        // ✅ ADD THIS LINE
        existing.VatEnabled = entity.VatEnabled == "Y" ? "Y" : "N";
        existing.UpdateDt = entity.UpdateDt;
        existing.UpdateTm = entity.UpdateTm;
        existing.UpdateBy = entity.UpdateBy;
        existing.UpdateRemarks = entity.UpdateRemarks;
        await _db.SaveChangesAsync(ct);
        return existing;
    }

    public async Task<bool> DeleteAsync(string fran, CancellationToken ct)
    {
        var existing = await _db.Franchises.FirstOrDefaultAsync(f => f.Fran == fran, ct);
        if (existing is null) return false;
        _db.Franchises.Remove(existing);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    // Added by: Nishanth
    // Added on: 04-02-2026

    //public async Task<List<SaasCustomerDropdownDto>> GetSaasCustomerDropdownAsync()
    //{
    //    return await _db.Franchises
    //        .AsNoTracking()
    //        .Select(x => new SaasCustomerDropdownDto
    //        {
    //            SaasCustomerId = x.Fran,   // string
    //            Name = x.Name              // correct property
    //        })
    //        .Distinct()
    //        .OrderBy(x => x.Name)
    //        .ToListAsync();
    //}

    //// Added by: Nishanth
    //// Added on: 05-02-2026
    //public async Task<List<CustomerCurrencyDrpDto>> GetCustomerCurrencyDropdownAsync()
    //{
    //    return await _db.Set<CustomerCurrencyDrpDto>()
    //        .FromSqlRaw("EXEC SP_CUSTOMERCURRENCY_DRP")
    //        .ToListAsync();
    //}

    //// Added: Added method to loadparam
    //// Added by: Nishanth
    //// Added on: 06-02-2026
    //public async Task<IReadOnlyList<LoadParam>> LoadByParamAsync(string fran, string paramType, CancellationToken ct)
    //{
    //    var results = new List<LoadParam>();

    //    using var conn = db.Database.GetDbConnection();
    //    if (conn.State == System.Data.ConnectionState.Closed)
    //        await conn.OpenAsync(ct);

    //    using var cmd = conn.CreateCommand();
    //    cmd.CommandText = "SP_Load_Param";
    //    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    //    var p1 = cmd.CreateParameter();
    //    p1.ParameterName = "@FRAN";
    //    p1.Value = fran;
    //    cmd.Parameters.Add(p1);

    //    var p2 = cmd.CreateParameter();
    //    p2.ParameterName = "@PARAMTYPE";
    //    p2.Value = paramType;
    //    cmd.Parameters.Add(p2);

    //    using var reader = await cmd.ExecuteReaderAsync(ct);
    //    while (await reader.ReadAsync(ct))
    //    {
    //        results.Add(new LoadParam
    //        {
    //            ParamValue = reader["PARAMVALUE"].ToString() ?? "",
    //            ParamDesc = reader["PARAMDESC"].ToString() ?? ""
    //        });
    //    }

    //    return results;
    //}

}
