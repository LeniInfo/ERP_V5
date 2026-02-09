namespace ERP.Contracts.Master;

public sealed record CreateFranchiseRequest(string Fran, string Name, string NameAr, bool VatEnabled, String CustomerCurrency, string NatureOfBusiness);
public sealed record UpdateFranchiseRequest(string? Name, string? NameAr, bool VatEnabled, String CustomerCurrency, string NatureOfBusiness);
