using ERP.Application.Abstractions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Finance;
using ERP.Contracts.Master;
using ERP.Domain.Entities;

namespace ERP.Application.Services;

public sealed class FranchisesService(IFranchisesRepository repo, IAppLogger<FranchisesService> log) : IFranchisesService
{
    private readonly IFranchisesRepository _repo = repo;
    private readonly IAppLogger<FranchisesService> _log = log;

    public async Task<IReadOnlyList<FranchiseDto>> GetAllAsync(CancellationToken ct)
    => await _repo.GetAllAsync(ct);

    public async Task<FranchiseDto?> GetByKeyAsync(string fran, CancellationToken ct)
    => await _repo.GetByKeyAsync(fran, ct);

    public async Task<FranchiseDto> CreateAsync(CreateFranchiseRequest request, CancellationToken ct)
    {
        Validate(request);
        var now = DateTime.UtcNow;
        var e = new Franchise
        {
            Fran = request.Fran,
            Name = request.Name,
            NameAr = request.NameAr,
            CustomerCurrency = request.CustomerCurrency,
            NatureOfBusiness = request.NatureOfBusiness,
            // ✅ ADD THIS
            VatEnabled = request.VatEnabled ? "Y" : "N",
            CreateDt = DateOnly.FromDateTime(now),
            CreateTm = now,
            CreateBy = string.Empty,
            CreateRemarks = string.Empty,
            UpdateDt = DateOnly.FromDateTime(now),
            UpdateTm = now,
            UpdateBy = string.Empty,
            UpdateRemarks = string.Empty,
        };
        var created = await _repo.AddAsync(e, ct);
        _log.Info("Created Franchise {Fran}", created.Fran);
        return await _repo.GetByKeyAsync(created.Fran, ct)
        ?? throw new Exception("Create failed");
    }

    public async Task<FranchiseDto?> UpdateAsync(string fran, UpdateFranchiseRequest request, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var e = new Franchise
        {
            Fran = fran,
            Name = request.Name ?? string.Empty,
            NameAr = request.NameAr ?? string.Empty,
            CustomerCurrency = request.CustomerCurrency,
            NatureOfBusiness = request.NatureOfBusiness,
            VatEnabled = request.VatEnabled ? "Y" : "N",
            UpdateDt = DateOnly.FromDateTime(now),
            UpdateTm = now,
            UpdateBy = string.Empty,
            UpdateRemarks = string.Empty,
        };
        var updated = await _repo.UpdateAsync(e, ct);
        if (updated is null) return null;
        _log.Info("Updated Franchise {Fran}", fran);
        return await _repo.GetByKeyAsync(fran, ct);
    }

    public Task<bool> DeleteAsync(string fran, CancellationToken ct) => _repo.DeleteAsync(fran, ct);

    private static void Validate(CreateFranchiseRequest r)
    {
        if (string.IsNullOrWhiteSpace(r.Fran)) throw new ArgumentException("Fran is required");
        if (string.IsNullOrWhiteSpace(r.Name)) throw new ArgumentException("Name is required");
        if (string.IsNullOrWhiteSpace(r.NameAr)) throw new ArgumentException("NameAr is required");
    }


    // Added by: Nishanth
    // Added on: 04-02-2026
    //private static FranchiseDto Map(Franchise e) => new()
    //{
    //    Fran = e.Fran,
    //    Name = e.Name,
    //    NameAr = e.NameAr,
    //    SaasCustomerId = e.SaasCustomerId ?? string.Empty
    //};


    //// Added: Added method to call the storedprocedure
    //// Added by: Nishanth
    //// Added on: 04-02-2026

    //public async Task<List<SaasCustomerDropdownDto>> GetSaasCustomerDropdownAsync()
    //{
    //    return await _repo.GetSaasCustomerDropdownAsync();

    //}
    //// Added by: Nishanth
    //// Added on: 05-02-2026
    //public async Task<List<CustomerCurrencyDrpDto>> GetCustomerCurrencyDropdownAsync()
    //{
    //    return await _repo.GetCustomerCurrencyDropdownAsync();
    //}

    //// Added: Added method to loadparam
    //// Added by: Nishanth
    //// Added on: 06-02-2026
    //public async Task<IReadOnlyList<LoadParam>> LoadByParamAsync(string fran, string paramType, CancellationToken ct)
    //{
    //    return await repo.LoadByParamAsync(fran, paramType, ct);
    //}


}
