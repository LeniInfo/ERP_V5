using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Infrastructure.Persistence.Configurations;

public sealed class MasterTypeConfiguration : IEntityTypeConfiguration<MasterType>
{
    public void Configure(EntityTypeBuilder<MasterType> b)
    {
        b.ToTable("MASTERTYPE", "dbo");

        // Composite Primary Key
        b.HasKey(x => new
        {
            x.Fran,
            x.MasterTypeCode,
            x.MasterCode
        });

        // Key Columns
        b.Property(x => x.Fran)
            .HasColumnName("FRAN")
            .HasMaxLength(10)
            .IsRequired();

        b.Property(x => x.MasterTypeCode)
            .HasColumnName("MASTERTYPE")
            .HasMaxLength(25)
            .IsRequired();

        b.Property(x => x.MasterCode)
            .HasColumnName("MASTERCODE")
            .HasMaxLength(10)
            .IsRequired();

        // Master Details
        b.Property(x => x.Name)
            .HasColumnName("NAME")
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.NameAr)
            .HasColumnName("NAMEAR")
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.Phone)
            .HasColumnName("PHONE")
            .HasMaxLength(50)
            .IsRequired();

        b.Property(x => x.Email)
            .HasColumnName("EMAIL")
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.Address)
            .HasColumnName("ADDRESS")
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.VatNo)
            .HasColumnName("VATNO")
            .HasMaxLength(50)
            .IsRequired();

        // Sequence
        b.Property(x => x.SeqNo)
            .HasColumnName("SEQNO")
           .HasMaxLength(10)
            .IsRequired();

        b.Property(x => x.SeqPrefix)
            .HasColumnName("SEQPREFIX")
            .HasMaxLength(50)
            .IsRequired();

        // Audit Fields
        b.Property(x => x.CreateDate)
            .HasColumnName("CREATEDT")
            .HasColumnType("date")
            .IsRequired();

        b.Property(x => x.CreateTime)
            .HasColumnName("CREATETM")
            .HasColumnType("datetime2(7)")
            .IsRequired();

        b.Property(x => x.CreateBy)
            .HasColumnName("CREATEBY")
            .HasMaxLength(10)
            .IsRequired();

        b.Property(x => x.CreateRemarks)
            .HasColumnName("CREATEREMARKS")
            .HasMaxLength(200)
            .IsRequired();

        b.Property(x => x.UpdateDate)
            .HasColumnName("UPDATEDT")
            .HasColumnType("date")
            .IsRequired();

        b.Property(x => x.UpdateTime)
            .HasColumnName("UPDATETM")
            .HasColumnType("datetime2(7)")
            .IsRequired();

        b.Property(x => x.UpdateBy)
            .HasColumnName("UPDATEBY")
            .HasMaxLength(10)
            .IsRequired();

        b.Property(x => x.UpdateRemarks)
            .HasColumnName("UPDATEMARKS")
            .HasMaxLength(200)
            .IsRequired();
    }
}
