using ERP.Contracts.Master;
using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<IReadOnlyList<UserAccount>> GetAllAsync(CancellationToken ct);
    Task<UserAccount?> GetByKeyAsync(string fran, string userId, CancellationToken ct);
    Task CreateAsync(UserAccount entity, CancellationToken ct);
    Task UpdateAsync(UserAccount entity, CancellationToken ct);
    Task DeleteAsync(string fran, string userId, CancellationToken ct);

    //added by: Vaishnavi
    //added on: 29-12-2025
    Task<LoginResponseDto?> LoginAsync(
        string userId,
        string password,
        CancellationToken ct);
}
