using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class FranchiseConfiguration : IEntityTypeConfiguration<Franchise>
{
    public void Configure(EntityTypeBuilder<Franchise> builder)
    {
        builder.ToTable("FRAN", "dbo");
        builder.HasKey(x => x.Fran);

        builder.Property(x => x.Fran).HasColumnName("FRAN").HasMaxLength(10).IsRequired();
        builder.Property(x => x.Name).HasColumnName("NAME").HasMaxLength(100).IsRequired();
        builder.Property(x => x.NameAr).HasColumnName("NAMEAR").HasMaxLength(100).IsUnicode().IsRequired();
        //added by: Vaishnavi
        //added on: 27-12-2025
        builder.Property(x => x.SAASCUSTOMERID).HasColumnName("SAASCUSTOMERID").HasMaxLength(10).IsRequired();

        // 12-01-2026 Changes Jegan
        builder.Property(x => x.VATENABLED).HasColumnName("VATENABLED").HasMaxLength(1).IsRequired();
        builder.Property(x => x.NATUREOFBUSINESS).HasColumnName("NATUREOFBUSINESS").HasMaxLength(1).IsRequired();
        builder.Property(x => x.CUSTOMERCURRENCY).HasColumnName("CUSTOMERCURRENCY").HasMaxLength(1).IsRequired();
        // 12-01-2026 Changes Ends Jegan

        builder.Property(x => x.CreateDt).HasColumnName("CREATEDT").IsRequired();
        builder.Property(x => x.CreateTm).HasColumnName("CREATETM").IsRequired();
        builder.Property(x => x.CreateBy).HasColumnName("CREATEBY").HasMaxLength(10).IsRequired();
        builder.Property(x => x.CreateRemarks).HasColumnName("CREATEREMARKS").HasMaxLength(200).IsRequired();
        builder.Property(x => x.UpdateDt).HasColumnName("UPDATEDT").IsRequired();
        builder.Property(x => x.UpdateTm).HasColumnName("UPDATETM").IsRequired();
        builder.Property(x => x.UpdateBy).HasColumnName("UPDATEBY").HasMaxLength(10).IsRequired();
        builder.Property(x => x.UpdateRemarks).HasColumnName("UPDATEMARKS").HasMaxLength(200).IsRequired();
    }
}
