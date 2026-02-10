using ERP.Contracts.Orders;
using ERP.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Application.Interfaces.Services
{
    public interface IWorkInvHdrService
    {
        Task<IEnumerable<WorkInvHdr>> GetAllAsync();
        Task<(WorkInvHdr Header, List<WorkInvDet> Details)> GetByIdAsync(string fran, string brch, string workshop, string workInvType, string workInvNo);
        Task AddAsync(WorkInvoiceCreateDto dto);
        Task UpdateAsync(WorkInvoiceUpdateDto dto);
        Task DeleteAsync(string fran, string brch, string workshop, string workInvType, string workInvNo);
    }
}

