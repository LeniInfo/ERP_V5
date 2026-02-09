using ERP.Contracts.Orders;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Data;

namespace ERP.Infrastructure.Repositories
{
    public class WorkInvHdrRepository : IWorkInvHdrRepository
    {
        private readonly ErpDbContext _context;

        public WorkInvHdrRepository(ErpDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkInvHdr>> GetAllAsync()
        {
            return await _context.WorkInvHdrs
                .FromSqlRaw("EXEC SP_GetAndUpdateWorkInvoice @Action",
                    new SqlParameter("@Action", "GETALL"))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(WorkInvHdr Header, List<WorkInvDet> Details)> GetByIdAsync(string fran, string brch, string workshop, string workInvType, string workInvNo)
        {
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SP_GetAndUpdateWorkInvoice";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@Action", "GETHDRDET"));
            cmd.Parameters.Add(new SqlParameter("@WORKINVNO", workInvNo));
            cmd.Parameters.Add(new SqlParameter("@FRAN", fran));
            cmd.Parameters.Add(new SqlParameter("@BRCH", brch));
            cmd.Parameters.Add(new SqlParameter("@WORKSHOP", workshop));
            cmd.Parameters.Add(new SqlParameter("@WORKINVTYPE", workInvType));
            cmd.Parameters.Add(new SqlParameter("@JSONData", DBNull.Value));

            using var reader = await cmd.ExecuteReaderAsync();

            // ---------- HEADER ----------
            WorkInvHdr? header = null;

            if (await reader.ReadAsync())
            {
                header = new WorkInvHdr
                {
                    Fran = reader["FRAN"].ToString(),
                    Brch = reader["BRCH"].ToString(),
                    Workshop = reader["WORKSHOP"].ToString(),
                    WorkInvType = reader["WORKINVTYPE"].ToString(),
                    WorkInvNo = reader["WORKINVNO"].ToString(),
                    Customer = reader["CUSTOMER"].ToString(),
                    WorkInvDt = Convert.ToDateTime(reader["WORKINVDT"]),
                    Currency = reader["CURRENCY"].ToString(),
                    NoOfParts = Convert.ToDecimal(reader["NOOFPARTS"]),
                    NoOfJobs = Convert.ToDecimal(reader["NOOFJOBS"]),
                    Discount = Convert.ToDecimal(reader["DISCOUNT"]),
                    TotalValue = Convert.ToDecimal(reader["TOTALVALUE"])
                };
            }

            // ---------- DETAILS ----------
            await reader.NextResultAsync();

            var details = new List<WorkInvDet>();

            while (await reader.ReadAsync())
            {
                details.Add(new WorkInvDet
                {
                    Fran = reader["FRAN"].ToString(),
                    Brch = reader["BRCH"].ToString(),
                    Workshop = reader["WORKSHOP"].ToString(),
                    WorkId = Convert.ToDecimal(reader["WORKID"]),
                    WorkInvType = reader["WORKINVTYPE"].ToString(),
                    WorkInvNo = reader["WORKINVNO"].ToString(),
                    WorkInvSrl = Convert.ToDecimal(reader["WORKINVSRL"]),
                    RepairSrl = Convert.ToDecimal(reader["REPAIRSRL"]),
                    WorkType = reader["WORKTYPE"].ToString(),
                    WorkInvDt = Convert.ToDateTime(reader["WORKINVDT"]),
                    UnitPrice = Convert.ToDecimal(reader["UNITPRICE"]),
                    Discount = Convert.ToDecimal(reader["DISCOUNT"]),
                    TotalValue = Convert.ToDecimal(reader["TOTALVALUE"]),
                    Qty = Convert.ToDecimal(reader["Qty"])
                });
            }

            return (header!, details);
        }

        public async Task AddAsync(WorkInvoiceCreateDto dto)
        {
            try
            {
                var jsonPayload = new
                {
                    header = new
                    {
                        Fran = dto.Header.Fran,
                        Brch = dto.Header.Brch,
                        Workshop = dto.Header.Workshop,
                        WorkInvType = dto.Header.WorkInvType,
                        WorkInvDt = dto.Header.WorkInvDt,
                        Customer = dto.Header.Customer,
                        VehicleId = dto.Header.VehicleId,
                        Currency = dto.Header.Currency,
                        NoOfJobs = dto.Header.NoOfJobs,
                        NoOfParts = dto.Header.NoOfParts,
                        Discount = dto.Header.Discount,
                        TotalValue = dto.Header.TotalValue,
                        CreateBy = dto.Header.CreateBy,
                        CreateRemarks = dto.Header.CreateRemarks
                    },
                    details = dto.Details.Select(d => new
                    {
                        Fran = dto.Header.Fran,
                        Brch = dto.Header.Brch,
                        Workshop = dto.Header.Workshop,
                        WorkInvType = dto.Header.WorkInvType,
                        Customer = dto.Header.Customer,
                        RepairSrl = d.RepairSrl,
                        WorkType = d.WorkType,
                        WorkInvDt = d.WorkInvDt,
                        UnitPrice = d.UnitPrice,
                        Discount = d.Discount,
                        TotalValue = d.TotalValue,
                        Qty = d.Qty
                    }).ToList()
                };

                string jsonData = JsonSerializer.Serialize(jsonPayload);

                var pFran = new SqlParameter("@psFran", dto.Header.Fran);
                var pPrefix = new SqlParameter("@psDOCPrefix", "WI");   // or JOB / RO
                var pMode = new SqlParameter("@Mode", "CREATE");
                var pJson = new SqlParameter("@JSONData", SqlDbType.NVarChar, -1)
                {
                    Value = jsonData
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_Save_WorkInvoice @psFran, @psDOCPrefix, @Mode, @JSONData",
                    pFran, pPrefix, pMode, pJson
                );
            }
            catch
            {
                throw;
            }
        }

        public async Task UpdateAsync(WorkInvoiceUpdateDto dto)
        {
            var jsonPayload = new
            {
                header = new
                {
                    Fran = dto.Header.Fran,
                    NoOfParts = dto.Header.NoOfParts,
                    NoOfJobs = dto.Header.NoOfJobs,
                    Discount = dto.Header.Discount,
                    TotalValue = dto.Header.TotalValue,
                    WorkInvType = dto.Header.WorkInvType,
                    Customer = dto.Header.Customer,
                    Brch = dto.Header.Brch,
                    Workshop = dto.Header.Workshop
                },
                details = dto.Details.Select(d => new
                {
                    RepairSrl = d.RepairSrl,
                    WorkInvType = d.WorkInvType,
                    WorkId = d.WorkId,
                    WorkType = d.WorkType,
                    WorkInvDt = d.WorkInvDt,
                    UnitPrice = d.UnitPrice,
                    Discount = d.Discount,
                    TotalValue = d.TotalValue,
                    Qty = d.Qty,
                }).ToList()
            };


            string jsonData = JsonSerializer.Serialize(jsonPayload);

            var parameters = new[]
            {
                new SqlParameter("@Action", "UPDATE"),
                new SqlParameter("@WORKINVNO", dto.Header.WorkInvNo),
                new SqlParameter("@FRAN", dto.Header.Fran),
                new SqlParameter("@WORKINVTYPE", dto.Header.WorkInvType),
                new SqlParameter("@BRCH", dto.Header.Brch),
                new SqlParameter("@WORKSHOP", dto.Header.Workshop),
                new SqlParameter("@JSONData", SqlDbType.NVarChar, -1)
                {
                    Value = jsonData
                }
        };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_GetAndUpdateWorkInvoice @Action,@WORKINVNO,@WORKINVTYPE,@BRCH,@WORKSHOP,@FRAN,@JSONData",
                parameters
            );
        }

        public async Task DeleteAsync(string fran, string brch, string workshop, string workInvType, string workInvNo)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", "DELETE"),
                new SqlParameter("@WORKINVNO", workInvNo),
                new SqlParameter("@FRAN", fran),
                 new SqlParameter("@BRCH", brch),
                new SqlParameter("@WORKINVTYPE", workInvType),
                new SqlParameter("@WORKSHOP", workshop),
                new SqlParameter("@JSONData", DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_GetAndUpdateWorkInvoice @Action,@WORKINVNO,@FRAN,@BRCH,@WORKINVTYPE,@WORKSHOP,@JSONData",
                parameters
            );
        }
    }
}

