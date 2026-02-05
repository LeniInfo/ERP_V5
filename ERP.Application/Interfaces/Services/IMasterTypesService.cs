using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ERP.Contracts.Master;

namespace ERP.Application.Interfaces.Services
{
    /// <summary>Service abstraction for MasterType operations.</summary>
    public interface IMasterTypesService
    {
        Task<MasterTypeDto?> GetByKeyAsync(
            string fran,
            string masterType,
            string masterCode,
            CancellationToken ct);

        Task<IReadOnlyList<MasterTypeDto>> GetAllAsync(CancellationToken ct);


        Task<MasterTypeDto> CreateAsync(
            MasterTypeDto dto,
            CancellationToken ct);

        Task<MasterTypeDto?> UpdateAsync(
            string fran,
            string masterType,
            string masterCode,
            MasterTypeDto dto,
            CancellationToken ct);

        Task<bool> DeleteAsync(
            string fran,
            string masterType,
            string masterCode,
            CancellationToken ct);
    }
}

