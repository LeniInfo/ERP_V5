using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

/// <summary>Repository abstraction for Purchase Orders (header + lines).</summary>
public interface IPurchaseOrdersRepository
{
    //Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken ct);
    //Task<PurchaseOrder?> GetByKeyAsync(string fran, string branch, string warehouseCode, string poType, string poNumber, CancellationToken ct);
    //Task<bool> AddAsync(PurchaseOrder header, IEnumerable<PurchaseOrderLine> lines, CancellationToken ct);
    //Task<PurchaseOrder?> UpdateAsync(PurchaseOrder header, IEnumerable<PurchaseOrderLine> lines, CancellationToken ct);
    //Task<bool> DeleteAsync(string fran, string branch, string warehouseCode, string poType, string poNumber, CancellationToken ct);

    Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken ct);

    Task<PurchaseOrder?> GetByKeyAsync(
        string fran,
        string branch,
        string warehouseCode,
        string poType,
        string poNumber,
        CancellationToken ct);

    Task<bool> AddAsync(
        PurchaseOrder header,
        IEnumerable<PurchaseOrderLine> lines,
        CancellationToken ct);

    Task<bool> UpdateUsingSpAsync(
        string fran,
        string pono,
        string supplier,
        string jsonData,
        CancellationToken ct);

    Task<bool> DeleteAsync(
        string fran,
        string pono,
        CancellationToken ct);
}