using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Contracts.Master
{
    public class QuotationDtos

    {
        public List<QuotationReq>? QuotationReq { get; set; }

    }
    public class QuotationReq
    {
        public string FRAN { get; set; }
        public string BRCH { get; set; }
        public string WHSE { get; set; }
        public string PREFIX { get; set; }
        public string QUOTATIONTYPE { get; set; }
        public string QUOTATIONNO { get; set; }
        public int QUOTATIONSRL { get; set; }
        public DateTime? QUOTATIONDT { get; set; }

        public string MAKE { get; set; }
        public string PART { get; set; }
        public string WORKID { get; set; }

        public decimal QTY { get; set; }
        public decimal ACCPQTY { get; set; }
        public decimal NOTAVLQTY { get; set; }

        public decimal UNITPRICE { get; set; }
        public decimal DISCOUNT { get; set; }
        public decimal VATPERCENTAGE { get; set; }
        public decimal VATVALUE { get; set; }
        public decimal DISCOUNTVALUE { get; set; }
        public decimal TOTALVALUE { get; set; }

        public DateTime? CREATEDT { get; set; }
        public TimeSpan? CREATETM { get; set; }
        public string CREATEBY { get; set; }
        public string CREATEREMARKS { get; set; }
        public string CUSTOMER { get; set; }
        public string CURRENCY { get; set; }
        public string REFTYPE { get; set; }

        public DateTime? UPDATEDT { get; set; }
        public TimeSpan? UPDATETM { get; set; }
        public string UPDATEBY { get; set; }
        public string UPDATEREMARKS { get; set; }
    }
    public class QuoteRes
    {
        public string FRAN { get; set; }
        public string BRCH { get; set; }
        public string WHSE { get; set; }
        public string QUOTATIONTYPE { get; set; }
        public string QUOTATIONNO { get; set; }
        public string QUOTATIONSRL { get; set; }
        public string QUOTATIONDT { get; set; }

        public string MAKE { get; set; }
        public string PART { get; set; }
        public string WORKID { get; set; }

        public string QTY { get; set; }
        public string ACCPQTY { get; set; }
        public string NOTAVLQTY { get; set; }
        public string UNITPRICE { get; set; }
        public string DISCOUNT { get; set; }
        public string VATPERCENTAGE { get; set; }
        public string VATVALUE { get; set; }
        public string DISCOUNTVALUE { get; set; }
        public string TOTALVALUE { get; set; }

        public string CREATEDT { get; set; }
        public string CREATETM { get; set; }
        public string CREATEBY { get; set; }
        public string CREATEREMARKS { get; set; }

        public string UPDATEDT { get; set; }
        public string UPDATETM { get; set; }
        public string UPDATEBY { get; set; }
        public string UPDATEREMARKS { get; set; }
    }

}
