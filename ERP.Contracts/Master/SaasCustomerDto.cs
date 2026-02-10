namespace ERP.Contracts.Master
{
    /// <summary>Represents a SaaS Customer master record.</summary>
    public sealed class SaasCustomerDto
    {
        /// <summary>SaaS Customer ID (Primary Key)</summary>
        public string SaasCustomerId { get; set; } = string.Empty;

        public string? SaasCustomerName { get; set; }

        public string? Phone1 { get; set; }

        public string? Phone2 { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public DateTime? CreateDt { get; set; }

        public DateTime? CreateTm { get; set; }

        public string? CreateBy { get; set; }

        public string? CreateRemarks { get; set; }

        public DateTime? UpdateDt { get; set; }

        public DateTime? UpdateTm { get; set; }

        public string? UpdateBy { get; set; }

        public string? UpdateRemarks { get; set; }
    }
}
