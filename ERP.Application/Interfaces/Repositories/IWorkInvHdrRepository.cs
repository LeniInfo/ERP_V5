using ERP.Contracts.Orders;
using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IWorkInvHdrRepository
    {
        Task<IEnumerable<WorkInvHdr>> GetAllAsync();
        Task<(WorkInvHdr Header, List<WorkInvDet> Details)> GetByIdAsync(string fran, string brch, string workshop, string workInvType, string workInvNo);
        Task AddAsync(WorkInvoiceCreateDto entity);
        Task UpdateAsync(WorkInvoiceUpdateDto entity);
        Task DeleteAsync(string fran, string brch, string workshop, string workInvType, string workInvNo);
    }
}

