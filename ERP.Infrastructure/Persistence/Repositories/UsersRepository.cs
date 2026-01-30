using ERP.Application.Interfaces.Repositories;
using ERP.Contracts.Master;
using ERP.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ERP.Infrastructure.Persistence.Repositories;

public sealed class UsersRepository : IUsersRepository
{
    private readonly ErpDbContext _db;
    public UsersRepository(ErpDbContext db) => _db = db;

    public async Task<IReadOnlyList<UserAccount>> GetAllAsync(CancellationToken ct)
        => await _db.Users.AsNoTracking().OrderBy(x => x.Fran).ThenBy(x => x.UserId).ToListAsync(ct);

    public async Task<UserAccount?> GetByKeyAsync(string fran, string userId, CancellationToken ct)
        => await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Fran == fran && x.UserId == userId, ct);

    public async Task CreateAsync(UserAccount entity, CancellationToken ct)
    {
        _db.Users.Add(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(UserAccount entity, CancellationToken ct)
    {
        _db.Users.Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string fran, string userId, CancellationToken ct)
    {
        var tracked = await _db.Users.FirstOrDefaultAsync(x => x.Fran == fran && x.UserId == userId, ct);
        if (tracked is null) return;
        _db.Users.Remove(tracked);
        await _db.SaveChangesAsync(ct);
    }

    //added by: Vaishnavi
    //added on: 29-12-2025
    public async Task<LoginResponseDto?> LoginAsync(
        string userId,
        string password,
        CancellationToken ct)
    {
        var userIdParam = new SqlParameter("@UserId", userId);
        var passwordParam = new SqlParameter("@Password", password);

        await using var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync(ct);

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "dbo.SP_Login";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(userIdParam);
        cmd.Parameters.Add(passwordParam);

        await using var reader = await cmd.ExecuteReaderAsync(ct);

        if (await reader.ReadAsync(ct))
        {
            return new LoginResponseDto
            {
                Fran = reader["FRAN"] == DBNull.Value
                    ? null
                    : reader["FRAN"].ToString(),

                Flag = reader["FLAG"]?.ToString() ?? "0"
            };
        }

        return null;
    }

}
