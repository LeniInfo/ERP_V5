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
    /// <summary>Application service for MasterType operations.</summary>
    public sealed class MasterTypesService : IMasterTypesService
    {
        private readonly IMasterTypesRepository _repo;
        private readonly IAppLogger<MasterTypesService> _log;

        public MasterTypesService(
            IMasterTypesRepository repo,
            IAppLogger<MasterTypesService> log)
        {
            _repo = repo;
            _log = log;
        }

        public async Task<MasterTypeDto?> GetByKeyAsync(
            string fran,
            string masterType,
            string masterCode,
            CancellationToken ct)
        {
            var entity = await _repo.GetByKeyAsync(fran, masterType, masterCode, ct);
            return entity is null ? null : ToDto(entity);
        }

        public async Task<IReadOnlyList<MasterTypeDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(ToDto).ToList();
        }

        public async Task<MasterTypeDto> CreateAsync(
            MasterTypeDto dto,
            CancellationToken ct)
        {
            //SEQNO
            var maxSeq = await _repo.GetNextSeqAsync(dto.Fran, dto.MasterType, dto.SeqPrefix!, ct);
            var nextMasterCode = await _repo.GetNextMasterCodeAsync(dto.Fran, dto.MasterType, dto.SeqPrefix!, ct);


            var entity = new MasterType
            {
                Fran = dto.Fran,
                MasterTypeCode = dto.MasterType,
                MasterCode = nextMasterCode,

                Name = dto.Name ?? string.Empty,
                NameAr = dto.NameAr ?? string.Empty,
                Phone = dto.Phone ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                Address = dto.Address ?? string.Empty,
                VatNo = dto.VatNo ?? string.Empty,

                SeqNo = maxSeq,
                SeqPrefix = "CT",

                CreateDate = dto.CreateDate ?? DateTime.Now,
                CreateTime = dto.CreateTime ?? DateTime.Now,
                CreateBy = dto.CreateBy ?? string.Empty,
                CreateRemarks = dto.CreateRemarks ?? string.Empty,

                UpdateDate = dto.UpdateDate ?? DateTime.Now,
                UpdateTime = dto.UpdateTime ?? DateTime.Now,
                UpdateBy = dto.UpdateBy ?? string.Empty,
                UpdateRemarks = dto.UpdateRemarks ?? string.Empty
            };

            var created = await _repo.AddAsync(entity, ct);
            _log.Info(
                "Created MasterType {Fran}-{MasterType}-{MasterCode}",
                created.Fran,
                created.MasterTypeCode,
                created.MasterCode);

            return ToDto(created);
        }

        public async Task<MasterTypeDto?> UpdateAsync(
            string fran,
            string masterType,
            string masterCode,
            MasterTypeDto dto,
            CancellationToken ct)
        {
            var toUpdate = new MasterType
            {
                Fran = fran,
                MasterTypeCode = masterType,
                MasterCode = masterCode,

                Name = dto.Name ?? string.Empty,
                NameAr = dto.NameAr ?? string.Empty,
                Phone = dto.Phone ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                Address = dto.Address ?? string.Empty,
                VatNo = dto.VatNo ?? string.Empty,

                SeqNo = dto.SeqNo,
                SeqPrefix = dto.SeqPrefix ?? string.Empty,

                UpdateDate = DateTime.Now,          // ✅ always set current date
                UpdateTime = DateTime.Now,          // ✅ always set current time
                UpdateBy = dto.UpdateBy ?? "SYSTEM",
                UpdateRemarks = dto.UpdateRemarks ?? string.Empty

            };

            var updated = await _repo.UpdateAsync(toUpdate, ct);
            if (updated is null)
            {
                _log.Warn(
                    "MasterType not found for update {Fran}-{MasterType}-{MasterCode}",
                    fran, masterType, masterCode);
                return null;
            }

            _log.Info(
                "Updated MasterType {Fran}-{MasterType}-{MasterCode}",
                fran, masterType, masterCode);

            return ToDto(updated);
        }


        public async Task<bool> DeleteAsync(
            string fran,
            string masterType,
            string masterCode,
            CancellationToken ct)
        {
            var ok = await _repo.DeleteAsync(fran, masterType, masterCode, ct);

            if (!ok)
            {
                _log.Warn(
                    "MasterType not found for delete {Fran}-{MasterType}-{MasterCode}",
                    fran, masterType, masterCode);
            }
            else
            {
                _log.Info(
                    "Deleted MasterType {Fran}-{MasterType}-{MasterCode}",
                    fran, masterType, masterCode);
            }

            return ok;
        }

        private static MasterTypeDto ToDto(MasterType e) => new()
        {
            Fran = e.Fran,
            MasterType = e.MasterTypeCode,
            MasterCode = e.MasterCode,

            Name = e.Name,
            NameAr = e.NameAr,
            Phone = e.Phone,
            Email = e.Email,
            Address = e.Address,
            VatNo = e.VatNo,

            SeqNo = e.SeqNo,
            SeqPrefix = e.SeqPrefix,

            CreateDate = e.CreateDate,
            CreateTime = e.CreateTime,
            CreateBy = e.CreateBy,
            CreateRemarks = e.CreateRemarks,

            UpdateDate = e.UpdateDate,
            UpdateTime = e.UpdateTime,
            UpdateBy = e.UpdateBy,
            UpdateRemarks = e.UpdateRemarks
        };
    }
}

