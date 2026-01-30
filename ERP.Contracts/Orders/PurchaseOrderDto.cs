namespace ERP.Contracts.Orders
{
    /// <summary>Represents a purchase order header.</summary>
    public sealed class PurchaseOrderDto
    {
        public string Fran { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public string WarehouseCode { get; set; } = null!;
        public string PoType { get; set; } = null!;
        public string PoNumber { get; set; } = null!;
        public string PoDate { get; set; } = null!;
        public string SupplierCode { get; set; } = null!;
        public string? SupplierRefNo { get; set; }
        public string Currency { get; set; } = null!;
        public int NoOfItems { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalValue { get; set; }
        public List<PurchaseOrderLineDto> Lines { get; set; } = new();
    }
}