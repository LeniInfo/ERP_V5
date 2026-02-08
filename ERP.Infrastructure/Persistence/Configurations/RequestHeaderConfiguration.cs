using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class RequestHeaderConfiguration : IEntityTypeConfiguration<RequestHeader>
{
    public void Configure(EntityTypeBuilder<RequestHeader> b)
    {
        b.ToTable("REQUESTHDR", "dbo");
        b.HasKey(x => new { x.Fran, x.Branch, x.Warehouse, x.RequestType, x.RequestNo });

        b.Property(x => x.Fran).HasColumnName("FRAN").HasMaxLength(10).IsRequired();
        b.Property(x => x.Branch).HasColumnName("BRCH").HasMaxLength(10).IsRequired();
        b.Property(x => x.Warehouse).HasColumnName("WHSE").HasMaxLength(10).IsRequired();
        b.Property(x => x.RequestType).HasColumnName("REQUESTTYPE").HasMaxLength(50).IsRequired();
        b.Property(x => x.RequestNo).HasColumnName("REQUESTNO").HasMaxLength(10).IsRequired();
        b.Property(x => x.RequestDate).HasColumnName("REQUESTDT").HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v)).IsRequired();
        b.Property(x => x.Customer).HasColumnName("CUSTOMER").HasMaxLength(10).IsRequired();
        b.Property(x => x.RequestSource).HasColumnName("REQUESTSOURCE").HasMaxLength(50).IsRequired();
        b.Property(x => x.RefNo).HasColumnName("REFNO").HasMaxLength(50).IsRequired();
        b.Property(x => x.RefDate).HasColumnName("REFDATE").HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v)).IsRequired();
        b.Property(x => x.SeqNo).HasColumnName("SEQNO").HasPrecision(22, 0).IsRequired();
        b.Property(x => x.SeqPrefix).HasColumnName("SEQPREFIX").HasMaxLength(10).IsRequired();
        b.Property(x => x.Currency).HasColumnName("CURRENCY").HasMaxLength(10).IsRequired();
        b.Property(x => x.NoOfItems).HasColumnName("NOOFITEMS").HasPrecision(22, 0).IsRequired();
        b.Property(x => x.VatValue).HasColumnName("VATVALUE").HasPrecision(22, 3).IsRequired();
        b.Property(x => x.TotalValue).HasColumnName("TOTALVALUE").HasPrecision(22, 3).IsRequired();
        b.Property(x => x.DescEn).HasColumnName("DESCEN").HasMaxLength(300).IsRequired(false);
        b.Property(x => x.DescArabic).HasColumnName("DESCARABIC").HasMaxLength(300).IsRequired(false);

        // Audit
        b.Property(x => x.CreateDt).HasColumnName("CREATEDT").HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v)).IsRequired();
        b.Property(x => x.CreateTm).HasColumnName("CREATETM").HasColumnType("datetime").IsRequired();
        b.Property(x => x.CreateBy).HasColumnName("CREATEBY").HasMaxLength(10).IsRequired();
        b.Property(x => x.CreateRemarks).HasColumnName("CREATEREMARKS").HasMaxLength(200).IsRequired();
        b.Property(x => x.UpdateDt).HasColumnName("UPDATEDT").HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v)).IsRequired();
        b.Property(x => x.UpdateTm).HasColumnName("UPDATETM").HasColumnType("datetime").IsRequired();
        b.Property(x => x.UpdateBy).HasColumnName("UPDATEBY").HasMaxLength(10).IsRequired();
        b.Property(x => x.UpdateRemarks).HasColumnName("UPDATEREMARKS").HasMaxLength(200).IsRequired();
    }
}
