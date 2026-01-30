using ERP.Application.Interfaces.Repositories;
using ERP.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace ERP.Infrastructure.Persistence.Repositories;

/// <summary>Purchase Orders repository (SP driven for ADD / UPDATE / DELETE).</summary>
public sealed class PurchaseOrdersRepository : IPurchaseOrdersRepository
{
    private readonly ErpDbContext _db;
    private readonly ILogger<PurchaseOrdersRepository> _log;
    private readonly string _conn;

    //warning changes(02-01-2026)
    public PurchaseOrdersRepository(
    ErpDbContext db,
    IConfiguration config,
    ILogger<PurchaseOrdersRepository> log)
    {
        _db = db;
        _log = log;

        _conn = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured.");
    }


    //public PurchaseOrdersRepository(
    //    ErpDbContext db,
    //    IConfiguration config,
    //    ILogger<PurchaseOrdersRepository> log)
    //{
    //    _db = db;
    //    _log = log;
    //    _conn = config.GetConnectionString("DefaultConnection");
    //}

    public async Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken ct)
        => await _db.PurchaseOrders
            .AsNoTracking()
            .OrderByDescending(h => h.PoDate)
            .ToListAsync(ct);

    public async Task<PurchaseOrder?> GetByKeyAsync(
        string fran,
        string branch,
        string warehouseCode,
        string poType,
        string poNumber,
        CancellationToken ct)
        => await _db.PurchaseOrders
            .AsNoTracking()
            .Include(h => h.Lines)
            .FirstOrDefaultAsync(h =>
                h.Fran == fran &&
                h.Branch == branch &&
                h.WarehouseCode == warehouseCode &&
                h.PoType == poType &&
                h.PoNumber == poNumber, ct);

    public async Task<bool> AddAsync(
        PurchaseOrder header,
        IEnumerable<PurchaseOrderLine> lines,
        CancellationToken ct)
    {
        var jsonPayload = new
        {
            header = new
            {
                FRAN = header.Fran,
                BRCH = header.Branch,
                WHSE = header.WarehouseCode,
                POTYPE = header.PoType,
                VENDOR = header.SupplierCode,
                CREATEBY = header.CreateBy ?? "SYSTEM",
                CREATEREMARKS = header.CreateRemarks ?? string.Empty,
                CURRENCY = header.Currency ?? "INR",
                NOOFITEMS = header.NoOfItems,
                DISCOUNT = header.Discount,
                TOTALVALUE = header.TotalValue
            },
            details = lines.Select(l => new
            {
                POSRL = l.PoLineNumber.ToString(),
                MAKE = l.Make,
                PART = l.PartCode,
                QTY = l.Qty,
                UNITPRICE = l.UnitPrice,
                DISCOUNT = l.Discount,
                VATPERCENTAGE = l.VatPercentage,
                PLANTYPE = l.PlanType,
                PLANNO = l.PlanNo,
                PLANSRL = l.PlanSerial ?? 0,
                VATVALUE = l.VatValue,
                DISCOUNTVALUE = l.DiscountValue,
                TOTALVALUE = l.TotalValue,
                CREATEBY = l.CreateBy ?? "SYSTEM",
                CREATEREMARKS = l.CreateRemarks ?? string.Empty
            })
        };

        var json = JsonSerializer.Serialize(jsonPayload);

        var parameters = new[]
        {
            new SqlParameter("@psFran", header.Fran),
            new SqlParameter("@psDOCPrefix", "PO"),
            new SqlParameter("@Mode", "ADD"),
            new SqlParameter("@JSONData", json)
        };

        _log.LogInformation("Creating PO via SP_Save_PO");

        var result = await _db.Database.ExecuteSqlRawAsync(
            "EXEC dbo.SP_Save_PO @psFran, @psDOCPrefix, @Mode, @JSONData",
            parameters,
            ct);

        return result > 0;
    }

    public async Task<bool> UpdateUsingSpAsync(
    string fran,
    string pono,
    string supplier,
    string jsonData,
    CancellationToken ct)
    {
        using var con = new SqlConnection(_conn);
        using var cmd = new SqlCommand("SP_GetAndUpdatePO", con)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@Action", "UPDATE");
        cmd.Parameters.AddWithValue("@PONO", pono);
        cmd.Parameters.AddWithValue("@FRAN", fran);
        cmd.Parameters.AddWithValue("@SUPPLIER", supplier);
        cmd.Parameters.AddWithValue("@JSONData", jsonData);

        await con.OpenAsync(ct);

        //warning changes(02-01-2026)
        var result = await cmd.ExecuteScalarAsync(ct);

        if (result is null || result == DBNull.Value)
            return false;

        var jsonResult = Convert.ToString(result);

        return jsonResult?.Contains("success", StringComparison.OrdinalIgnoreCase) == true;


        //var result = await cmd.ExecuteScalarAsync(ct);
        //if (result == null) return false;

        //var jsonResult = result.ToString();
        //return jsonResult.Contains("success");
    }

    public async Task<bool> DeleteAsync(
    string fran,
    string pono,
    CancellationToken ct)
    {
        using var con = new SqlConnection(_conn);
        using var cmd = new SqlCommand("SP_GetAndUpdatePO", con)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@Action", "DELETE");
        cmd.Parameters.AddWithValue("@PONO", pono);
        cmd.Parameters.AddWithValue("@FRAN", fran);

        await con.OpenAsync(ct);
        await cmd.ExecuteNonQueryAsync(ct);

        return true;
    }

}
