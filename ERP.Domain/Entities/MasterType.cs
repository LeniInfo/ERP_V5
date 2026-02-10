using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Entities;

public sealed class MasterType
{
    // Composite Primary Key
    public string Fran { get; set; } = null!;
    public string MasterTypeCode { get; set; } = null!;
    public string MasterCode { get; set; } = null!;

    // Master Details
    public string Name { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string VatNo { get; set; } = null!;

    // Sequence
    public string SeqNo { get; set; }
    public string SeqPrefix { get; set; } = null!;

    // Audit Fields
    public DateTime? CreateDate { get; set; }
    public DateTime? CreateTime { get; set; }
    public string? CreateBy { get; set; } = null!;
    public string? CreateRemarks { get; set; } = null!;

    public DateTime? UpdateDate { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? UpdateBy { get; set; } = null!;
    public string? UpdateRemarks { get; set; } = null!;
}

