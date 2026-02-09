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
    public sealed class SaasCustomer
    {
        public string SaasCustomerId { get; set; } = string.Empty;

        public string SaasCustomerName { get; set; } = string.Empty;

        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }

        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime? CreateDt { get; set; }
        public DateTime? CreateTm { get; set; }

        public string CreateBy { get; set; } = string.Empty;
        public string CreateRemarks { get; set; } = string.Empty;

        public DateTime? UpdateDt { get; set; }
        public DateTime? UpdateTm { get; set; }

        public string UpdateBy { get; set; } = string.Empty;
        public string UpdateMarks { get; set; } = string.Empty;
    }

}
