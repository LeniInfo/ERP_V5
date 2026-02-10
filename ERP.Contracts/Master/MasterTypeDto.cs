namespace ERP.Contracts.Master
{
    /// <summary>Represents a MasterType master record.</summary>
    public sealed class MasterTypeDto
    {
        /// <summary>Franchise / Company code.</summary>
        public string Fran { get; set; } = null!;

        /// <summary>Master category (e.g. SUPPLIER, CUSTOMER, ITEM).</summary>
        public string MasterType { get; set; } = null!;

        /// <summary>Business master code.</summary>
        public string MasterCode { get; set; } = null!;

        /// <summary>English name.</summary>
        public string? Name { get; set; }

        /// <summary>Arabic name.</summary>
        public string? NameAr { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? VatNo { get; set; }

        /// <summary>Sequence number for code generation.</summary>
        public string SeqNo { get; set; } = null!;

        /// <summary>Sequence prefix.</summary>
        public string? SeqPrefix { get; set; }

        // Audit (kept optional for client simplicity)
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
