using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

/// <summary>Repository abstraction for MasterType persistence.</summary>
public interface IMasterTypesRepository
{
    Task<MasterType?> GetByKeyAsync(
        string fran,
        string masterType,
        string masterCode,
        CancellationToken ct);

    Task<IReadOnlyList<MasterType>> GetAllAsync(CancellationToken ct);

    Task<MasterType> AddAsync(MasterType entity, CancellationToken ct);

    Task<string> GetNextSeqAsync(
    string fran,
    string masterType,
    string prefix,
    CancellationToken ct);

    Task<string> GetNextMasterCodeAsync(
    string fran,
    string masterType,
    string prefix,
    CancellationToken ct);


    Task<MasterType?> UpdateAsync(MasterType entity, CancellationToken ct);

    Task<bool> DeleteAsync(
        string fran,
        string masterType,
        string masterCode,
        CancellationToken ct);
}
