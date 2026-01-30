using System.Text.Json.Serialization;

namespace ERP.Contracts.Orders;

public record CustomerOrderHeaderDto(
    string Fran,
    string Branch,
    string Warehouse,
    string CordType,
    string CordNo,
    DateOnly CordDate,
    string Customer,
    decimal SeqNo,
    string SeqPrefix,
    string Currency,
    decimal NoOfItems,
    decimal DiscountValue,
    decimal GrossValue,
    decimal NetValue,
    decimal VatValue,
    decimal TotalValue,
    DateOnly CreateDt,
    DateTime CreateTm,
    string CreateBy
);

public record CreateCustomerOrderHeaderRequest(
    [property: JsonPropertyName("fran")] string Fran,
    [property: JsonPropertyName("branch")] string Branch,
    [property: JsonPropertyName("warehouse")] string Warehouse,
    [property: JsonPropertyName("cordType")] string CordType,
    [property: JsonPropertyName("cordNo")] string CordNo,
    [property: JsonPropertyName("cordDate")] DateOnly? CordDate = null,
    [property: JsonPropertyName("customer")] string? Customer = null,
    [property: JsonPropertyName("seqNo")] decimal? SeqNo = null,
    [property: JsonPropertyName("seqPrefix")] string? SeqPrefix = null,
    [property: JsonPropertyName("currency")] string? Currency = null,
    [property: JsonPropertyName("noOfItems")] decimal? NoOfItems = null,
    [property: JsonPropertyName("discountValue")] decimal? DiscountValue = null,
    [property: JsonPropertyName("grossValue")] decimal? GrossValue = null,
    [property: JsonPropertyName("netValue")] decimal? NetValue = null,
    [property: JsonPropertyName("vatValue")] decimal? VatValue = null,
    [property: JsonPropertyName("totalValue")] decimal? TotalValue = null
);

public record UpdateCustomerOrderHeaderRequest(
    [property: JsonPropertyName("cordDate")] DateOnly? CordDate = null,
    [property: JsonPropertyName("customer")] string? Customer = null,
    [property: JsonPropertyName("seqNo")] decimal? SeqNo = null,
    [property: JsonPropertyName("seqPrefix")] string? SeqPrefix = null,
    [property: JsonPropertyName("currency")] string? Currency = null,
    [property: JsonPropertyName("noOfItems")] decimal? NoOfItems = null,
    [property: JsonPropertyName("discountValue")] decimal? DiscountValue = null,
    [property: JsonPropertyName("grossValue")] decimal? GrossValue = null,
    [property: JsonPropertyName("netValue")] decimal? NetValue = null,
    [property: JsonPropertyName("vatValue")] decimal? VatValue = null,
    [property: JsonPropertyName("totalValue")] decimal? TotalValue = null
);

public record CustomerOrderDetailDto(
    string Fran,
    string Branch,
    string Warehouse,
    string CordType,
    string CordNo,
    string CordSrl,
    DateOnly CordDate,
    string Make,
    decimal Part,
    decimal Qty,
    decimal AccpQty,
    decimal NotAvlQty,
    decimal Price,
    decimal Discount,
    decimal VatPercentage,
    decimal VatValue,
    decimal DiscountValue,
    decimal TotalValue,
    DateOnly CreateDt,
    DateTime CreateTm,
    string CreateBy
);

public record CreateCustomerOrderDetailRequest(
    [property: JsonPropertyName("fran")] string Fran,
    [property: JsonPropertyName("branch")] string Branch,
    [property: JsonPropertyName("warehouse")] string Warehouse,
    [property: JsonPropertyName("cordType")] string CordType,
    [property: JsonPropertyName("cordNo")] string CordNo,
    [property: JsonPropertyName("cordSrl")] string CordSrl,
    [property: JsonPropertyName("cordDate")] DateOnly? CordDate = null,
    [property: JsonPropertyName("make")] string? Make = null,
    [property: JsonPropertyName("part")] decimal? Part = null,
    [property: JsonPropertyName("qty")] decimal? Qty = null,
    [property: JsonPropertyName("accpQty")] decimal? AccpQty = null,
    [property: JsonPropertyName("notAvlQty")] decimal? NotAvlQty = null,
    [property: JsonPropertyName("price")] decimal? Price = null,
    [property: JsonPropertyName("discount")] decimal? Discount = null,
    [property: JsonPropertyName("vatPercentage")] decimal? VatPercentage = null,
    [property: JsonPropertyName("vatValue")] decimal? VatValue = null,
    [property: JsonPropertyName("discountValue")] decimal? DiscountValue = null,
    [property: JsonPropertyName("totalValue")] decimal? TotalValue = null
);

public record UpdateCustomerOrderDetailRequest(
    [property: JsonPropertyName("cordDate")] DateOnly? CordDate = null,
    [property: JsonPropertyName("make")] string? Make = null,
    [property: JsonPropertyName("part")] decimal? Part = null,
    [property: JsonPropertyName("qty")] decimal? Qty = null,
    [property: JsonPropertyName("accpQty")] decimal? AccpQty = null,
    [property: JsonPropertyName("notAvlQty")] decimal? NotAvlQty = null,
    [property: JsonPropertyName("price")] decimal? Price = null,
    [property: JsonPropertyName("discount")] decimal? Discount = null,
    [property: JsonPropertyName("vatPercentage")] decimal? VatPercentage = null,
    [property: JsonPropertyName("vatValue")] decimal? VatValue = null,
    [property: JsonPropertyName("discountValue")] decimal? DiscountValue = null,
    [property: JsonPropertyName("totalValue")] decimal? TotalValue = null
);
