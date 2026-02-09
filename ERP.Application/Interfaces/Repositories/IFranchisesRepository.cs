using ERP.Contracts.Finance;
using ERP.Contracts.Master;
using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

public interface IFranchisesRepository
{
    Task<IReadOnlyList<FranchiseDto>> GetAllAsync(CancellationToken ct);
    Task<FranchiseDto?> GetByKeyAsync(string fran, CancellationToken ct);
    Task<Franchise> AddAsync(Franchise entity, CancellationToken ct);
    Task<Franchise?> UpdateAsync(Franchise entity, CancellationToken ct);
    Task<bool> DeleteAsync(string fran, CancellationToken ct);


    // Added: Added method to call the storedprocedure
    // Added by: Nishanth
    // Added on: 04-02-2026

    //Task<List<SaasCustomerDropdownDto>> GetSaasCustomerDropdownAsync();

    //// Added: Added method to call the storedprocedure
    //// Added by: Nishanth
    //// Added on: 05-02-2026
    //Task<List<CustomerCurrencyDrpDto>> GetCustomerCurrencyDropdownAsync();

    //// Added: Added method to loadparam
    //// Added by: Nishanth
    //// Added on: 06-02-2026
    //Task<IReadOnlyList<LoadParam>> LoadByParamAsync(string fran, string paramType, CancellationToken ct);


}
