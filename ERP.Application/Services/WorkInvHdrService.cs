using ERP.Application.Interfaces.Services;
using ERP.Contracts.Orders;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Application.Services
{
    public class WorkInvHdrService : IWorkInvHdrService
    {
        private readonly IWorkInvHdrRepository _repository;

        public WorkInvHdrService(IWorkInvHdrRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<WorkInvHdr>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<(WorkInvHdr Header, List<WorkInvDet> Details)> GetByIdAsync(string fran, string brch, string workshop, string workInvType, string workInvNo) =>
            await _repository.GetByIdAsync(fran, brch, workshop, workInvType, workInvNo);

        public async Task AddAsync(WorkInvoiceCreateDto dto) => await _repository.AddAsync(dto);

        public async Task UpdateAsync(WorkInvoiceUpdateDto dto) => await _repository.UpdateAsync(dto);

        public async Task DeleteAsync(string fran, string brch, string workshop, string workInvType, string workInvNo) => 
            await _repository.DeleteAsync(fran, brch, workshop, workInvType, workInvNo);
    }
}

