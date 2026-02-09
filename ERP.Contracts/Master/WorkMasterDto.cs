namespace ERP.Contracts.Master
{
    /// <summary>Represents a Work master record.</summary>
    public sealed class WorkMasterDto
    {
        /// <summary>Franchise or branch code.</summary>
        public string Fran { get; set; } = null!;

        /// <summary>Unique Work identifier.</summary>
        public decimal WorkId { get; set; }

        /// <summary>Type or category of work.</summary>
        public string? WorkType { get; set; }

        /// <summary>Name or title of the work.</summary>
        public string? Name { get; set; }

        /// <summary>Additional remarks about the work.</summary>
        public string? Remarks { get; set; }

        /// <summary>Unit price for the work.</summary>
        public decimal UnitPrice { get; set; }

        /// <summary>Estimated cost or quantity.</summary>
        public decimal Estimated { get; set; }

        /// <summary>Record creation timestamp.</summary>
        public DateTime CreateTm { get; set; }

        /// <summary>User who created the record.</summary>
        public string? CreateBy { get; set; }

        /// <summary>Creation remarks.</summary>
        public string? CreateRemarks { get; set; }

        /// <summary>Record last update timestamp.</summary>
        public DateTime UpdateTm { get; set; }

        /// <summary>User who last updated the record.</summary>
        public string? UpdateBy { get; set; }

        /// <summary>Update remarks or notes.</summary>
        public string? UpdateMarks { get; set; }
    }
}
