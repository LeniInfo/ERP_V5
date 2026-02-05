using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ERP.Contracts.Master;

namespace ERP.Application.Interfaces.Services
{
    /// <summary>Service abstraction for SaaS Customer operations.</summary>
    public interface ISaasCustomerService
    {
        Task<SaasCustomerDto?> GetByIdAsync(string saasCustomerId, CancellationToken ct);

        Task<IReadOnlyList<SaasCustomerDto>> GetAllAsync(CancellationToken ct);

        Task<SaasCustomerDto> CreateAsync(SaasCustomerDto dto, CancellationToken ct);

        Task<SaasCustomerDto?> UpdateAsync(string saasCustomerId, SaasCustomerDto dto, CancellationToken ct);

        Task<bool> DeleteAsync(string saasCustomerId, CancellationToken ct);
    }
}
