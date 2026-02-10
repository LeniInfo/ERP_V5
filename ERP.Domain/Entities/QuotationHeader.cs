namespace ERP.Domain.Entities;

public sealed class QuotationHeader
{
    // PK: FRAN, BRCH, WHSE, QUOTTYPE, QUOTATIONNO
    public string Fran { get; set; } = null!;
    public string Branch { get; set; } = null!;
    public string Warehouse { get; set; } = null!;
    public string QuotType { get; set; } = null!;
    public string QuotationNo { get; set; } = null!;
    
    public DateOnly QuotationDate { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string QuotationSource { get; set; } = string.Empty;
    public string RefNo { get; set; } = string.Empty;
    public DateOnly RefDate { get; set; }
    public decimal SeqNo { get; set; }
    public string SeqPrefix { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal NoOfItems { get; set; }
    public decimal Discount { get; set; }
    public decimal VatValue { get; set; }
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
}
