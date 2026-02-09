using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Contracts.Master
{
    public class QuotationDtos

    {
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

    }
}
