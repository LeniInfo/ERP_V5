namespace ERP.Domain.Entities;

public sealed class RequestDetail
{
    // PK: FRAN, BRCH, WHSE, REQUESTTYPE, REQUESTNO, REQUESTSRL
    public string Fran { get; set; } = null!;
    public string Branch { get; set; } = null!;
    public string Warehouse { get; set; } = null!;
    public string RequestType { get; set; } = null!;
    public string RequestNo { get; set; } = null!;
    public string RequestSrl { get; set; } = null!;
    
    public DateOnly RequestDate { get; set; }
    public decimal WorkId { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Part { get; set; } = string.Empty;
    public decimal Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal VatPercentage { get; set; }
    public decimal VatValue { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal TotalValue { get; set; }
    
    // Audit
    public DateOnly CreateDt { get; set; }
    public DateTime CreateTm { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public string CreateRemarks { get; set; } = string.Empty;
    public DateOnly UpdateDt { get; set; }
    public DateTime UpdateTm { get; set; }
    public string UpdateBy { get; set; } = string.Empty;
    public string UpdateRemarks { get; set; } = string.Empty;
    
    public RequestHeader? Header { get; set; }
}
