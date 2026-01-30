namespace ERP.Domain.Entities;

public sealed class Branch
{
    // DB: dbo.BRCH
    // PK per DB: BRCH (nvarchar(10))
    public string BranchCode { get; set; } = string.Empty; // BRCH

    public string Fran { get; set; } = string.Empty; // FRAN (varchar 10)

    // REFNO is not required as per latest requirement
    // public string RefNo { get; set; } = string.Empty; // REFNO (varchar 10)

    public string Name { get; set; } = string.Empty; // NAME (varchar 100)
    public string NameAr { get; set; } = string.Empty; // NAMEAR (nvarchar 100)

    // Audit
    public DateOnly CreateDt { get; set; } // CREATEDT (date)
    public DateTime CreateTm { get; set; } // CREATETM (datetime)
    public string CreateBy { get; set; } = string.Empty; // CREATEBY (varchar 10)
    public string CreateRemarks { get; set; } = string.Empty; // CREATEREMARKS (varchar 200)
    public DateOnly UpdateDt { get; set; } // UPDATEDT (date)
    public DateTime UpdateTm { get; set; } // UPDATETM (datetime)
    public string UpdateBy { get; set; } = string.Empty; // UPDATEBY (varchar 10)
    public string UpdateRemarks { get; set; } = string.Empty; // UPDATEMARKS (varchar 200)

    // Navigations intentionally omitted
}
