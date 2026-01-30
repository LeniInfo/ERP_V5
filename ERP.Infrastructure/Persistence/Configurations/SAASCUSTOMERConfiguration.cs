using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Persistence.Configurations
{ 
    public sealed class SAASCUSTOMERConfiguration : IEntityTypeConfiguration<SAASCUSTOMER>
    {
        //added by: Vaishnavi
        //added on: 27-12-2025
    public void Configure(EntityTypeBuilder<SAASCUSTOMER> builder)
    {
        // Table name
        builder.ToTable("SAASCUSTOMER");

        // Primary Key + Identity
        builder.HasKey(x => x.SaasCustomerId);
        builder.Property(x => x.SaasCustomerId).HasColumnName("SAASCUSTOMERID").HasMaxLength(10).IsRequired();
        //builder.Property(x => x.SaasCustomerId).HasColumnName("SAASCUSTOMERID").HasColumnType("numeric(22,0)").ValueGeneratedOnAdd(); 
        builder.Property(x => x.SaasCustomerName).HasColumnName("SAASCUSTOMERNAME").HasMaxLength(50).HasDefaultValue(string.Empty).IsRequired();
        builder.Property(x => x.Phone1).HasColumnName("PHONE1").HasColumnType("numeric(22,0)").HasDefaultValue(0);
        builder.Property(x => x.Phone2).HasColumnName("PHONE2").HasColumnType("numeric(22,0)").HasDefaultValue(0);
        builder.Property(x => x.Email).HasColumnName("EMAIL").HasMaxLength(50).HasDefaultValue(string.Empty);
        builder.Property(x => x.Address).HasColumnName("ADDRESS").HasMaxLength(200).HasDefaultValue(string.Empty);
        builder.Property(x => x.CreateDt).HasColumnName("CREATEDT").HasColumnType("date").HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.CreateTm).HasColumnName("CREATETM").HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
        builder.Property(x => x.CreateBy).HasColumnName("CREATEBY").HasMaxLength(10).HasDefaultValue(string.Empty);
        builder.Property(x => x.CreateRemarks).HasColumnName("CREATEREMARKS").HasMaxLength(200).HasDefaultValue(string.Empty);
        builder.Property(x => x.UpdateDt).HasColumnName("UPDATEDT").HasColumnType("date").HasDefaultValueSql("'1900-01-01'");
        builder.Property(x => x.UpdateTm).HasColumnName("UPDATETM").HasColumnType("date").HasDefaultValueSql("'1900-01-01'");
        builder.Property(x => x.UpdateBy).HasColumnName("UPDATEBY").HasMaxLength(10).HasDefaultValue(string.Empty);
        builder.Property(x => x.UpdateMarks).HasColumnName("UPDATEMARKS").HasMaxLength(200).HasDefaultValue(string.Empty);
    }
    }
}