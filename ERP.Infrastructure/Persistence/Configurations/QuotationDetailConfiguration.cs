using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class QuotationDetailConfiguration : IEntityTypeConfiguration<QuotationDetail>
{
    public void Configure(EntityTypeBuilder<QuotationDetail> b)
    {
        b.ToTable("QUOTATIONDET", "dbo");
        b.HasKey(x => new { x.Fran, x.Branch, x.Warehouse, x.QuotType, x.QuotationNo, x.QuotSrl });

        b.Property(x => x.Fran).HasColumnName("FRAN").HasMaxLength(10).IsRequired();
        b.Property(x => x.Branch).HasColumnName("BRCH").HasMaxLength(10).IsRequired();
        b.Property(x => x.Warehouse).HasColumnName("WHSE").HasMaxLength(10).IsRequired();
        b.Property(x => x.QuotType).HasColumnName("QUOTATIONTYPE").HasMaxLength(50).IsRequired();
        b.Property(x => x.QuotationNo).HasColumnName("QUOTATIONNO").HasMaxLength(10).IsRequired();
        b.Property(x => x.QuotSrl).HasColumnName("QUOTATIONSRL").HasMaxLength(10).IsRequired();
        b.Property(x => x.QuotationDate).HasColumnName("QUOTATIONDT").HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v)).IsRequired();
        // WORKID column doesn't exist - QUOTATIONSRL stores the workId, so we ignore this property
        b.Ignore(x => x.WorkId);
        b.Property(x => x.Make).HasColumnName("MAKE").HasMaxLength(10).IsRequired();
        b.Property(x => x.Part).HasColumnName("PART").HasMaxLength(10).IsRequired();
        b.Property(x => x.Qty).HasColumnName("QTY").HasPrecision(22, 0).IsRequired();
        b.Property(x => x.UnitPrice).HasColumnName("UNITPRICE").HasPrecision(22, 3).IsRequired();
        b.Property(x => x.Discount).HasColumnName("DISCOUNT").HasPrecision(22, 3).IsRequired();
        b.Property(x => x.VatPercentage).HasColumnName("VATPERCENTAGE").HasPrecision(22, 3).IsRequired();
        b.Property(x => x.VatValue).HasColumnName("VATVALUE").HasPrecision(22, 3).IsRequired();
        b.Property(x => x.DiscountValue).HasColumnName("DISCOUNTVALUE").HasPrecision(22, 3).IsRequired();
        b.Property(x => x.TotalValue).HasColumnName("TOTALVALUE").HasPrecision(22, 3).IsRequired();

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
