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
    /// <summary>Application service for Part operations.</summary>
    public sealed class PartsService : IPartsService
    {
        private readonly IPartsRepository _repo;
        private readonly IAppLogger<PartsService> _log;

        public PartsService(IPartsRepository repo, IAppLogger<PartsService> log)
        {
            _repo = repo;
            _log = log;
        }

        /// <summary>Get a single part by code.</summary>
        public async Task<PartDto?> GetByCodeAsync(string partCode, CancellationToken ct)
        {
            var entity = await _repo.GetByCodeAsync(partCode, ct);
            return entity is null ? null : ToDto(entity);
        }

        /// <summary>List all parts.</summary>
        public async Task<IReadOnlyList<PartDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(ToDto).ToList();
        }

        /// <summary>Search parts by name.</summary>
        public async Task<IReadOnlyList<PartDto>> SearchByNameAsync(string name, CancellationToken ct)
        {
            var rows = await _repo.SearchByNameAsync(name, ct);
            return rows.Select(ToDto).ToList();
        }

        // Added: Added to store parts
        // Added by: Vaishnavi
        // Added on: 10-12-2025
        public async Task<int> CreatePartAsync(PartRequests request, CancellationToken ct)
        {
            var result = await _repo.AddPartByStoredProcAsync(request, ct);

            _log.Info("Stored procedure SP_ADD_PART executed for Part: {Part}", request.PartCode);

            return result;
        }
        private static PartDto ToDto(Part e) => new()
        {
            PartCode = e.PartCode,
            Description = e.Description,
            Make = e.Make,
            Active = e.Active,
            Fob = e.Fob,
            Category = e.Category,
            SubsPart = e.SubsPart,
            FinalPart = e.FinalPart
        };

        // Added: Added to get parts
        // Added by: Vaishnavi
        // Added on: 12-12-2025
        public async Task<IReadOnlyList<PartDto>> GetAllPartsByStoredProcAsync(CancellationToken ct)
        {
            var list = await _repo.GetAllPartsByStoredProcAsync(ct);
            _log.Info("Stored procedure SP_GET_PART executed: Retrieved {Count} records", list.Count);
            return list.Select(ToDto).ToList();
        }



    }
}