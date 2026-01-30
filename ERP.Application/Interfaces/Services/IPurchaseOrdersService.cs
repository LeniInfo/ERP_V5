using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ERP.Contracts.Orders;

namespace ERP.Application.Interfaces.Services
{
    /// <summary>Service abstraction for Purchase Order operations.</summary>
    public interface IPurchaseOrdersService
    {
        //Task<IReadOnlyList<PurchaseOrderDto>> GetAllAsync(CancellationToken ct);
        //Task<PurchaseOrderDto?> GetByKeyAsync(string fran, string branch, string warehouseCode, string poType, string poNumber, CancellationToken ct);
        //Task<bool> CreateAsync(PurchaseOrderDto dto, CancellationToken ct);
        //Task<PurchaseOrderDto?> UpdateAsync(string fran, string branch, string warehouseCode, string poType, string poNumber, PurchaseOrderDto dto, CancellationToken ct);
        //Task<bool> DeleteAsync(string fran, string branch, string warehouseCode, string poType, string poNumber, CancellationToken ct);
        Task<IReadOnlyList<PurchaseOrderDto>> GetAllAsync(CancellationToken ct);

        Task<PurchaseOrderDto?> GetByKeyAsync(
            string fran,
            string branch,
            string warehouseCode,
            string poType,
            string poNumber,
            CancellationToken ct);

        Task<bool> CreateAsync(PurchaseOrderDto dto, CancellationToken ct);

        Task<bool> UpdateAsync(
            string fran,
            string pono,
            string supplier,
            PurchaseOrderDto dto,
            CancellationToken ct);

        Task<bool> DeleteAsync(
            string fran,
            string pono,
            CancellationToken ct);
    }
}