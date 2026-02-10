using ERP.Domain.Entities;

namespace ERP.Contracts.Orders
{
    public sealed class RepairOrderCreateDto
    {
        public RepairHdr Header { get; set; } = new RepairHdr();
        public List<RepairOrder> Details { get; set; } = new List<RepairOrder>();
    }

    public class RepairOrderUpdateDto
    {
        public RepairOrderUpdateHeaderDto Header { get; set; } = new();
        public List<RepairOrderUpdateDetailDto> Details { get; set; } = new();
    }

    public class RepairOrderUpdateHeaderDto
    {
        public string Fran { get; set; } = null!;
        public string Brch { get; set; } = null!;
        public string Workshop { get; set; } = null!;
        public string Customer { get; set; } = null!;      
        public string RepairNo { get; set; } = null!;
        public string RepairType { get; set; } = string.Empty;
        public string Currency { get; set; } = null!;      
        public int NoOfParts { get; set; }                 
        public int NoOfJobs { get; set; }                  
        public decimal Discount { get; set; }             
        public decimal TotalValue { get; set; }            
    }

    public class RepairOrderUpdateDetailDto
    {
        public string Fran { get; set; } = null!;          
        public string Brch { get; set; } = null!;           
        public string Workshop { get; set; } = null!;
        public string RepairType { get; set; } = string.Empty;

        public int RepairSrl { get; set; }
        public decimal VehicleId { get; set; } = 0;
        public decimal WorkId { get; set; } = 0;      
        public string WorkType { get; set; } = null!;       
        public DateTime WorkDt { get; set; }                

        public int NoOfWorks { get; set; }                  
        public decimal UnitPrice { get; set; }             
        public decimal Discount { get; set; }              
        public decimal TotalValue { get; set; }             

        public string CreateBy { get; set; } = null!;       
        public string? CreateRemarks { get; set; }          
        public string UpdateBy { get; set; } = string.Empty;
        public string? UpdateRemarks { get; set; }          

        public int Qty { get; set; }
        public string WorkDesc { get; set; } = string.Empty;
        public string WorkDescAr { get; set; } = string.Empty;
    }

    public class TranslateDto
    {
        public string Text { get; set; } = string.Empty; 
        public string From { get; set; } = "en"; 
        public string To { get; set; } = "ar"; 
    }

    public class TranslateResponse
    {
        public string translatedText { get; set; } = string.Empty;
    }
}
