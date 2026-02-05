using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

/// <summary>Repository abstraction for SaaS Customer persistence.</summary>
public interface ISaasCustomerRepository
{
    Task<SaasCustomer?> GetByIdAsync(string saasCustomerId, CancellationToken ct);

    Task<IReadOnlyList<SaasCustomer>> GetAllAsync(CancellationToken ct);

    Task<SaasCustomer> AddAsync(SaasCustomer customer, CancellationToken ct);

    Task<SaasCustomer?> UpdateAsync(SaasCustomer customer, CancellationToken ct);

    Task<bool> DeleteAsync(string saasCustomerId, CancellationToken ct);
}
