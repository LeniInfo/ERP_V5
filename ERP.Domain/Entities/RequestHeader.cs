namespace ERP.Domain.Entities;

public sealed class RequestHeader
{
    // PK: FRAN, BRCH, WHSE, REQUESTTYPE, REQUESTNO
    public string Fran { get; set; } = null!;
    public string Branch { get; set; } = null!;
    public string Warehouse { get; set; } = null!;
    public string RequestType { get; set; } = null!;
    public string RequestNo { get; set; } = null!;
    
    public DateOnly RequestDate { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string RequestSource { get; set; } = string.Empty;
    public string RefNo { get; set; } = string.Empty;
    public DateOnly RefDate { get; set; }
    public decimal SeqNo { get; set; }
    public string SeqPrefix { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal NoOfItems { get; set; }
    public decimal VatValue { get; set; }
    public decimal TotalValue { get; set; }
    public string DescEn { get; set; } = string.Empty;
    public string DescArabic { get; set; } = string.Empty;
    
    // Audit
    public DateOnly CreateDt { get; set; }
    public DateTime CreateTm { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public string CreateRemarks { get; set; } = string.Empty;
    public DateOnly UpdateDt { get; set; }
    public DateTime UpdateTm { get; set; }
    public string UpdateBy { get; set; } = string.Empty;
    public string UpdateRemarks { get; set; } = string.Empty;
}
