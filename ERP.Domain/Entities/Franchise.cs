namespace ERP.Domain.Entities;

public sealed class Franchise
{
    // DB: dbo.FRAN
    public string Fran { get; set; } = string.Empty; // PK (varchar 10)
    public string Name { get; set; } = string.Empty; // varchar 100
    public string NameAr { get; set; } = string.Empty; // nvarchar 100
    //added by: Vaishnavi
    //added on: 27-12-2025

     // Added by: Nishanth
    // Added on: 04-02-2026
    public string SaasCustomerId { get; set; } = string.Empty;

    // 12-01-2026 Changes Jegan
    public string VatEnabled { get; set; }  = string.Empty;
    public string NatureOfBusiness { get; set; } = string.Empty;
    public string CustomerCurrency { get; set; } = string.Empty;

    // 12-01-2026 Changes Ends Jegan

    // Audit
    public DateOnly CreateDt { get; set; }
    public DateTime CreateTm { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public string CreateRemarks { get; set; } = string.Empty;
    public DateOnly UpdateDt { get; set; }
    public DateTime UpdateTm { get; set; }
    public string UpdateBy { get; set; } = string.Empty;
    public string UpdateRemarks { get; set; } = string.Empty; // maps to UPDATEMARKS
}
