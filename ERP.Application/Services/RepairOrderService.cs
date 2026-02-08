using ERP.Application.Interfaces.Services;
using ERP.Contracts.Orders;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Application.Services
{
    public class RepairOrderService : IRepairOrderService
    {
        private readonly IRepairOrderRepository _repository;

        public RepairOrderService(IRepairOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RepairHdr>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<(RepairHdr Header, List<RepairOrder> Details)> GetHdrDetAsync(
    string fran,
    string brch,
    string workshop,
    string repairType,
    string repairNo,
    string customer)
        {
            return await _repository.GetHdrDetAsync(
                fran,
                brch,
                workshop,
                repairType,
                repairNo,
                customer
            );
        }


        public async Task UpdateAsync(RepairOrderUpdateDto dto)
        {
            await _repository.UpdateAsync(dto);
        }

        public async Task DeleteAsync(string fran, string customer, string repairNo)
        {
            await _repository.DeleteAsync(fran, customer, repairNo);
        }
        public Task AddAsync(RepairOrderCreateDto dto) => _repository.AddAsync(dto);

    }
}

