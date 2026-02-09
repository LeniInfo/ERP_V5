using ERP.Contracts.Finance;
using ERP.Contracts.Master;

namespace ERP.Application.Interfaces.Services;

public interface IFranchisesService
{
    Task<IReadOnlyList<FranchiseDto>> GetAllAsync(CancellationToken ct);
    Task<FranchiseDto?> GetByKeyAsync(string fran, CancellationToken ct);
    Task<FranchiseDto> CreateAsync(CreateFranchiseRequest request, CancellationToken ct);
    Task<FranchiseDto?> UpdateAsync(string fran, UpdateFranchiseRequest request, CancellationToken ct);
    Task<bool> DeleteAsync(string fran, CancellationToken ct);


    //// Added: Added method to call the storedprocedure
    //// Added by: Nishanth
    //// Added on: 04-02-2026

    //Task<List<SaasCustomerDropdownDto>> GetSaasCustomerDropdownAsync();

    //// Added by: Nishanth
    //// Added on: 05-02-2026
    //Task<List<CustomerCurrencyDrpDto>> GetCustomerCurrencyDropdownAsync();

    //// Added: Added method to loadparam
    //// Added by: Nishanth
    //// Added on: 06-02-2026
    //Task<IReadOnlyList<LoadParam>> LoadByParamAsync(string fran, string paramType, CancellationToken ct);


}
