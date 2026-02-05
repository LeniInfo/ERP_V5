using ERP.Contracts.Orders;
using ERP.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Application.Interfaces.Services
{
    public interface IRepairOrderService
    {
        Task<List<RepairHdr>> GetAllAsync();

        Task<(RepairHdr Header, List<RepairOrder> Details)> GetHdrDetAsync(
    string fran,
    string brch,
    string workshop,
    string repairType,
    string repairNo,
    string customer
);

        Task AddAsync(RepairOrderCreateDto dto);

        Task UpdateAsync(RepairOrderUpdateDto dto);

        Task DeleteAsync(string fran, string customer, string repairNo);
    }
}

