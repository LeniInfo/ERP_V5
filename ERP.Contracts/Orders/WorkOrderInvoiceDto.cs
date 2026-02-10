using ERP.Domain.Entities;

namespace ERP.Contracts.Orders
{
    public sealed class WorkInvoiceCreateDto
    {
        public WorkInvHdr Header { get; set; } = new WorkInvHdr();
        public List<WorkInvDet> Details { get; set; } = new List<WorkInvDet>();
    }

    public class WorkInvoiceUpdateDto
    {
        public WorkInvoiceUpdateHeaderDto Header { get; set; } = new();
        public List<WorkInvoiceUpdateDetailDto> Details { get; set; } = new();
    }

    public class WorkInvoiceUpdateHeaderDto
    {
        public string Fran { get; set; } = string.Empty;
        public string Brch { get; set; } = string.Empty;
        public string Workshop { get; set; } = string.Empty;

        public string WorkInvType { get; set; } = string.Empty;
        public string WorkInvNo { get; set; } = string.Empty;
        public DateTime WorkInvDt { get; set; }

        public string Customer { get; set; } = string.Empty;
        public decimal VehicleId { get; set; }

        public string Currency { get; set; } = string.Empty;

        public decimal NoOfParts { get; set; }
        public decimal NoOfJobs { get; set; }
        public decimal Discount { get; set; }
        public decimal VatValue { get; set; }
        public decimal TotalValue { get; set; }

        public decimal SeqNo { get; set; }
        public string SeqPrefix { get; set; } = string.Empty;

        public string UpdateBy { get; set; } = string.Empty;
        public string UpdateRemarks { get; set; } = string.Empty;
    }


    public class WorkInvoiceUpdateDetailDto
    {
        public string Fran { get; set; } = string.Empty;
        public string Brch { get; set; } = string.Empty;
        public string Workshop { get; set; } = string.Empty;

        public string WorkInvType { get; set; } = string.Empty;
        public string WorkInvNo { get; set; } = string.Empty;
        public decimal WorkInvSrl { get; set; }

        public DateTime WorkInvDt { get; set; }

        public string BillType { get; set; } = string.Empty;
        public string WorkType { get; set; } = string.Empty;
        public decimal WorkId { get; set; }

        public string Make { get; set; } = string.Empty;
        public decimal Part { get; set; }
        public decimal Qty { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }

        public decimal VatPercentage { get; set; }
        public decimal VatValue { get; set; }
        public decimal TotalValue { get; set; }

        public string ReapairType { get; set; } = string.Empty;
        public string ReapairNo { get; set; } = string.Empty;
        public decimal RepairSrl { get; set; }

        public string SaleType { get; set; } = string.Empty;
        public string SaleNo { get; set; } = string.Empty;

        public string UpdateBy { get; set; } = string.Empty;
        public string UpdateRemarks { get; set; } = string.Empty;
    }
}
