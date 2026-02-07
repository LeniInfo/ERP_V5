using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ERP.Contracts.Master;

namespace ERP.Application.Interfaces.Services
{
    /// <summary>Service abstraction for Part operations.</summary>
    public interface IPartsService
    {
        Task<PartDto?> GetByCodeAsync(string partCode, CancellationToken ct);
        Task<IReadOnlyList<PartDto>> GetAllAsync(CancellationToken ct);
        Task<IReadOnlyList<PartDto>> SearchByNameAsync(string name, CancellationToken ct);

        // Added: Added to store parts
        // Added by: Vaishnavi
        // Added on: 10-12-2025
        Task<int> CreatePartAsync(PartRequests request, CancellationToken ct);

        // Added: Added to get parts
        // Added by: Vaishnavi
        // Added on: 10-12-2025
        Task<IReadOnlyList<PartDto>> GetAllPartsByStoredProcAsync(CancellationToken ct);
    }
}