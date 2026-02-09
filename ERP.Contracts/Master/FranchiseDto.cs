namespace ERP.Contracts.Master;

public sealed class FranchiseDto
{
    public string Fran { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;

    // Added by: Nishanth
    // Added on: 04-02-2026
   
    public string SaasCustomerId { get; set; } = string.Empty;

    // Added: Added method to loadparam
    // Added by: Nishanth
    // Added on: 06-02-2026

    public string CustomerCurrency { get; set; } = "";
    public string ParamValue { get; set; } = "";
    public string ParamDesc { get; set; } = "";
    public bool VatEnabled { get; set; } 

}


