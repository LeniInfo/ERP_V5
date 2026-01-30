using ERP.Application.Abstractions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Orders;
using ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ERP.Application.Services
{
    /// <summary>Application service for Purchase Orders.</summary>
    public sealed class PurchaseOrdersService : IPurchaseOrdersService
    {
        private readonly IPurchaseOrdersRepository _repo;
        private readonly IAppLogger<PurchaseOrdersService> _log;

        public PurchaseOrdersService(
            IPurchaseOrdersRepository repo,
            IAppLogger<PurchaseOrdersService> log)
        {
            _repo = repo;
            _log = log;
        }

        // =====================================================
        // ?? GET ALL
        // =====================================================
        public async Task<IReadOnlyList<PurchaseOrderDto>> GetAllAsync(CancellationToken ct)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(ToDtoHeaderOnly).ToList();
        }

        // =====================================================
        // ?? GET BY KEY (EDIT LOAD)
        // =====================================================
        public async Task<PurchaseOrderDto?> GetByKeyAsync(
            string fran,
            string branch,
            string warehouseCode,
            string poType,
            string poNumber,
            CancellationToken ct)
        {
            var entity = await _repo.GetByKeyAsync(
                fran, branch, warehouseCode, poType, poNumber, ct);

            return entity is null ? null : ToDto(entity);
        }

        // =====================================================
        // ?? CREATE (ADD)
        // =====================================================
        public async Task<bool> CreateAsync(
            PurchaseOrderDto dto,
            CancellationToken ct)
        {
            var header = ToEntityHeader(dto);
            var lines = dto.Lines.Select(ToEntityLine).ToList();

            ApplyCreateDefaults(header, lines);

            var success = await _repo.AddAsync(header, lines, ct);

            if (!success)
            {
                _log.Warn("PO creation failed for {Fran}/{Type}", header.Fran, header.PoType);
                return false;
            }

            _log.Info("PO created successfully for {Fran}/{Type}", header.Fran, header.PoType);
            return true;
        }

        public async Task<bool> UpdateAsync(
    string fran,
    string pono,
    string supplier,
    PurchaseOrderDto dto,
    CancellationToken ct)
        {
            // Convert DTO to entity
            var header = ToEntityHeader(dto);
            var lines = dto.Lines.Select(ToEntityLine).ToList();

            // Apply defaults for update (audit, Make, Create fields, etc.)
            ApplyUpdateDefaults(header, lines);

            // Build JSON payload for the stored procedure
            var jsonPayload = new
            {
                header = new
                {
                    CURRENCY = header.Currency,
                    NOOFITEMS = header.NoOfItems,
                    DISCOUNT = header.Discount,
                    TOTALVALUE = header.TotalValue
                },
                details = lines.Select(l => new
                {
                    BRCH = l.Branch,
                    WHSE = l.WarehouseCode,
                    POTYPE = l.PoType,
                    POSRL = l.PoLineNumber,
                    MAKE = l.Make,
                    PART = l.PartCode,
                    QTY = l.Qty,
                    UNITPRICE = l.UnitPrice,
                    DISCOUNT = l.Discount,
                    VATVALUE = l.VatValue,
                    DISCOUNTVALUE = l.DiscountValue,
                    TOTALVALUE = l.TotalValue,
                    CREATEBY = l.CreateBy ?? "SYSTEM",
                    CREATETM = l.CreateTm,
                    UPDATETM = l.UpdateTm,
                    UPDATEBY = l.UpdateBy,
                    CREATEREMARKS = l.CreateRemarks ?? string.Empty,
                    UPDATEREMARKS = l.UpdateRemarks ?? string.Empty,
                    PLANTYPE = l.PlanType,
                    PLANNO = l.PlanNo,
                    PLANSRL = l.PlanSerial ?? 0,
                    VATPERCENTAGE = l.VatPercentage
                })
            };

            var json = JsonSerializer.Serialize(jsonPayload);

            // Call repository SP method
            return await _repo.UpdateUsingSpAsync(
                fran,
                pono,
                supplier,
                json,
                ct);
        }




        public Task<bool> DeleteAsync(
     string fran,
     string pono,
     CancellationToken ct)
        {
            return _repo.DeleteAsync(fran, pono, ct);
        }

        private static void ApplyCreateDefaults(
            PurchaseOrder header,
            List<PurchaseOrderLine> lines)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var now = DateTime.UtcNow;

            header.CreateDt = today;
            header.CreateTm = now;
            header.CreateBy = "SYSTEM";
            header.CreateRemarks = string.Empty;

            header.UpdateDt = today;
            header.UpdateTm = now;
            header.UpdateBy = "SYSTEM";
            header.UpdateRemarks = string.Empty;

            int i = 1;
            foreach (var l in lines)
            {
                l.PoLineNumber = (i++).ToString();
                l.Make = string.IsNullOrWhiteSpace(l.Make) ? "GEN" : l.Make;

                l.CreateDt = today;
                l.CreateTm = now;
                l.CreateBy = "SYSTEM";
                l.CreateRemarks = string.Empty;

                l.UpdateDt = today;
                l.UpdateTm = now;
                l.UpdateBy = "SYSTEM";
                l.UpdateRemarks = string.Empty;
            }
        }

        private static void ApplyUpdateDefaults(
            PurchaseOrder header,
            List<PurchaseOrderLine> lines)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var now = DateTime.UtcNow;

            header.UpdateDt = today;
            header.UpdateTm = now;
            header.UpdateBy = "SYSTEM";
            header.UpdateRemarks = string.Empty;

            foreach (var l in lines)
            {
                l.Make = string.IsNullOrWhiteSpace(l.Make) ? "GEN" : l.Make;

                l.CreateDt = today;
                l.CreateTm = now;
                l.CreateBy = "SYSTEM";
                l.CreateRemarks = string.Empty;

                l.UpdateDt = today;
                l.UpdateTm = now;
                l.UpdateBy = "SYSTEM";
                l.UpdateRemarks = string.Empty;
            }
        }

        private static PurchaseOrderDto ToDtoHeaderOnly(PurchaseOrder e) => new()
        {
            Fran = e.Fran,
            Branch = e.Branch,
            WarehouseCode = e.WarehouseCode,
            PoType = e.PoType,
            PoNumber = e.PoNumber,
            PoDate = e.PoDate.ToString("yyyy-MM-dd"),
            SupplierCode = e.SupplierCode,
            SupplierRefNo = e.SupplierRefNo,
            Currency = e.Currency,
            NoOfItems = (int)e.NoOfItems,
            Discount = e.Discount,
            TotalValue = e.TotalValue
        };

        private static PurchaseOrderDto ToDto(PurchaseOrder e)
        {
            var dto = ToDtoHeaderOnly(e);
            dto.Lines = e.Lines.Select(l => new PurchaseOrderLineDto
            {
                Fran = l.Fran,
                Branch = l.Branch,
                WarehouseCode = l.WarehouseCode,
                PoType = l.PoType,
                PoNumber = l.PoNumber,
                PoLineNumber = l.PoLineNumber,
                PoDate = l.PoDate.ToString("yyyy-MM-dd"),
                SupplierCode = l.SupplierCode,
                PlanType = l.PlanType,
                PlanNo = l.PlanNo,
                PlanSerial = l.PlanSerial.HasValue
                    ? (long?)decimal.ToInt64(l.PlanSerial.Value)
                    : null,
                Make = l.Make,
                PartCode = l.PartCode,
                Qty = l.Qty,
                UnitPrice = l.UnitPrice,
                Discount = l.Discount,
                VatPercentage = l.VatPercentage,
                VatValue = l.VatValue,
                DiscountValue = l.DiscountValue,
                TotalValue = l.TotalValue
            }).ToList();
            return dto;
        }

        private static PurchaseOrder ToEntityHeader(PurchaseOrderDto dto) => new()
        {
            Fran = dto.Fran,
            Branch = dto.Branch,
            WarehouseCode = dto.WarehouseCode,
            PoType = dto.PoType,
            PoNumber = dto.PoNumber,
            PoDate = DateOnly.Parse(dto.PoDate),
            SupplierCode = dto.SupplierCode,
            SupplierRefNo = dto.SupplierRefNo ?? string.Empty,
            Currency = dto.Currency,
            NoOfItems = dto.NoOfItems,
            Discount = dto.Discount,
            TotalValue = dto.TotalValue
        };

        private static PurchaseOrderLine ToEntityLine(PurchaseOrderLineDto dto) => new()
        {
            Fran = dto.Fran,
            Branch = dto.Branch,
            WarehouseCode = dto.WarehouseCode,
            PoType = dto.PoType,
            PoNumber = dto.PoNumber,
            PoLineNumber = dto.PoLineNumber,
            PoDate = DateOnly.Parse(dto.PoDate),
            SupplierCode = dto.SupplierCode,
            PlanType = dto.PlanType ?? string.Empty,
            PlanNo = dto.PlanNo ?? string.Empty,
            PlanSerial = dto.PlanSerial.HasValue ? (decimal?)dto.PlanSerial.Value : null,
            Make = dto.Make ?? string.Empty,
            PartCode = dto.PartCode,
            Qty = dto.Qty,
            UnitPrice = dto.UnitPrice,
            Discount = dto.Discount,
            VatPercentage = dto.VatPercentage,
            VatValue = dto.VatValue,
            DiscountValue = dto.DiscountValue,
            TotalValue = dto.TotalValue
        };
    }
}
