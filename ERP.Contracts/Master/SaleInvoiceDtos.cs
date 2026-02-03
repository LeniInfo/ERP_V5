namespace ERP.Contracts.Master;

public sealed class SaleInvoiceDtos
{
    public List<SaleInvoiceRequest>? saleinvoicerequest { get; set; }
}
public class SaleInvoiceRequest
{
    public string? FRAN { get; set; }
    public string? BRCH { get; set; }
    public string? WHSE { get; set; }
    public string? SALETYPE { get; set; }
    public string? CUSTOMER { get; set; }
    public string? CURRENCY { get; set; }
    public string? SALESCHANNEL { get; set; }
    public DateTime? SALEDT { get; set; }
    public string? MAKE { get; set; }
    public string? PART { get; set; }

    public decimal? QTY { get; set; }
    public decimal? UNITPRICE { get; set; }
    public decimal? DISCOUNT { get; set; }

    public decimal? VATPERCENTAGE { get; set; }
    public decimal? VATVALUE { get; set; }
    public decimal? DISCOUNTVALUE { get; set; }
    public decimal? TOTALVALUE { get; set; }
    public DateTime? CREATEDT { get; set; }
    public DateTime? CREATETM { get; set; }
    public string? CREATEBY { get; set; }
    public string? CREATEREMARKS { get; set; }

    public DateTime? UPDATEDT { get; set; }
    public DateTime? UPDATETM { get; set; }
    public string? UPDATEBY { get; set; }
    public string? UPDATEREMARKS { get; set; }
    public string? PREFIX { get; set; }
}


//warning changes on (02-01-2026)
public class ParamRequest
{
    public string? Type { get; set; }
    public string? Mode { get; set; }
    public string? Fran { get; set; }
}

public class ParamResponse
{
    public string? PARAMVALUE { get; set; }
    public string? PARAMDESC { get; set; }
}

public class WorkmasReq
{
    public string? Type { get; set; }
    public string? Mode { get; set; }
    public string? Fran { get; set; }
}
public class workmasRes
{
    public string? WORKID { get; set; }
    public string? Name { get; set; }
    public string? WORKTYPE { get; set; }
    public string? UNITPRICE { get; set; }
    public string? ESTIMATED { get; set; }
}
//public class paramreq
//{
//    public string? Type { get; set; }
//    public string? Mode { get; set; }
//    public string? Fran { get; set; }
//}

//public class paramres
//{
//    public string? PARAMVALUE { get; set; }
//    public string? PARAMDESC { get; set; }
//}