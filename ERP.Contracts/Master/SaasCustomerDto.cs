namespace ERP.Contracts.Master
{
    /// <summary>Represents a SaaS Customer master record.</summary>
    public sealed class SaasCustomerDto
    {
        /// <summary>SaaS Customer ID (Primary Key)</summary>
        public string? SaasCustomerId { get; set; } = null!;

        public string? SaasCustomerName { get; set; }

        public string? Phone1 { get; set; }

        public string? Phone2 { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? CreateTime { get; set; }

        public string? CreateBy { get; set; }

        public string? CreateRemarks { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string? UpdateBy { get; set; }

        public string? UpdateRemarks { get; set; }
    }
}
