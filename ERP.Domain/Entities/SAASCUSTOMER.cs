using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entities
{
    //added by: Vaishnavi
    //added on: 27-12-2025
    public sealed class SAASCUSTOMER

    {
        // PK: SAASCUSTOMERID 
    public string? SaasCustomerId { get; set; }
    public string SaasCustomerName { get; set; } = string.Empty;
    public decimal Phone1 { get; set; } = 0;
    public decimal Phone2 { get; set; } = 0;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateOnly CreateDt { get; set; }
    public DateTime CreateTm { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public string CreateRemarks { get; set; } = string.Empty;
    public DateOnly UpdateDt { get; set; }
    public DateOnly UpdateTm { get; set; }
    public string UpdateBy { get; set; } = string.Empty;
    public string UpdateMarks { get; set; } = string.Empty;
}
}
