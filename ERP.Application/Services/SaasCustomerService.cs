using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ERP.Application.Abstractions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Master;
using ERP.Domain.Entities;

namespace ERP.Application.Services
{
    /// <summary>Application service for SaaS Customer operations.</summary>
    public sealed class SaasCustomerService : ISaasCustomerService
    {
        private readonly ISaasCustomerRepository _repo;
        private readonly IAppLogger<SaasCustomerService> _log;

        public SaasCustomerService(
            ISaasCustomerRepository repo,
            IAppLogger<SaasCustomerService> log)
        {
            _repo = repo;
            _log = log;
        }

        public async Task<SaasCustomerDto?> GetByIdAsync(string saasCustomerId, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(saasCustomerId, ct);
            return entity is null ? null : ToDto(entity);
        }

        public async Task<IReadOnlyList<SaasCustomerDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(ToDto).ToList();
        }

        public async Task<SaasCustomerDto> CreateAsync(SaasCustomerDto dto, CancellationToken ct)
        {
            var entity = new SaasCustomer
            {
                SaasCustomerId = dto.SaasCustomerId,
                SaasCustomerName = dto.SaasCustomerName ?? string.Empty,
                Phone1 = dto.Phone1,
                Phone2 = dto.Phone2,
                Email = dto.Email ?? string.Empty,
                Address = dto.Address ?? string.Empty,
                CreateDt = System.DateTime.UtcNow,
                CreateTm = System.DateTime.UtcNow,
                CreateBy = dto.CreateBy ?? string.Empty,
                CreateRemarks = dto.CreateRemarks ?? string.Empty
            };

            var created = await _repo.AddAsync(entity, ct);
            _log.Info("Created SaaS Customer {SaasCustomerId}", created.SaasCustomerId);

            return ToDto(created);
        }

        public async Task<SaasCustomerDto?> UpdateAsync(string saasCustomerId, SaasCustomerDto dto, CancellationToken ct)
        {
            var entity = new SaasCustomer
            {
                SaasCustomerId = saasCustomerId,
                SaasCustomerName = dto.SaasCustomerName ?? string.Empty,
                Phone1 = dto.Phone1,
                Phone2 = dto.Phone2,
                Email = dto.Email ?? string.Empty,
                Address = dto.Address ?? string.Empty,
                UpdateDt = System.DateTime.UtcNow,
                UpdateTm = System.DateTime.UtcNow,
                UpdateBy = dto.UpdateBy ?? string.Empty,
                UpdateMarks = dto.UpdateRemarks ?? string.Empty
            };

            var updated = await _repo.UpdateAsync(entity, ct);
            if (updated is null)
            {
                _log.Warn("SaaS Customer not found for update {SaasCustomerId}", saasCustomerId);
                return null;
            }

            _log.Info("Updated SaaS Customer {SaasCustomerId}", saasCustomerId);
            return ToDto(updated);
        }

        public async Task<bool> DeleteAsync(string saasCustomerId, CancellationToken ct)
        {
            var ok = await _repo.DeleteAsync(saasCustomerId, ct);

            if (!ok)
                _log.Warn("SaaS Customer not found for delete {SaasCustomerId}", saasCustomerId);
            else
                _log.Info("Deleted SaaS Customer {SaasCustomerId}", saasCustomerId);

            return ok;
        }

        // =========================
        // Mapping: Entity → DTO
        // =========================
        private static SaasCustomerDto ToDto(SaasCustomer e) => new()
        {
            SaasCustomerId = e.SaasCustomerId,
            SaasCustomerName = e.SaasCustomerName,
            Phone1 = e.Phone1,
            Phone2 = e.Phone2,
            Email = e.Email,
            Address = e.Address,
            CreateDate = e.CreateDt,
            CreateTime = e.CreateTm,
            CreateBy = e.CreateBy,
            CreateRemarks = e.CreateRemarks,
            UpdateDate = e.UpdateDt,
            UpdateTime = e.UpdateTm,
            UpdateBy = e.UpdateBy,
            UpdateRemarks = e.UpdateMarks
        };
    }
}
