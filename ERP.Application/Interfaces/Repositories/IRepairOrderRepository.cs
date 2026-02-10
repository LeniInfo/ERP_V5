using ERP.Contracts.Orders;
using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IRepairOrderRepository
    {
        Task<List<RepairHdr>> GetAllAsync();

        Task<(RepairHdr Header, List<RepairOrder> Details)> GetHdrDetAsync(
    string fran,
    string brch,
    string workshop,
    string repairType,
    string repairNo,
    string customer);

        Task UpdateAsync(RepairOrderUpdateDto dto);

        Task DeleteAsync(string fran, string customer, string repairNo);
        Task AddAsync(RepairOrderCreateDto dto);
    }
}

