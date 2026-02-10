using ERP.Contracts.Orders;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace ERP.Infrastructure.Repositories
{
    public class RepairOrderRepository : IRepairOrderRepository
    {
        private readonly ErpDbContext _context;

        public RepairOrderRepository(ErpDbContext context)
        {
            _context = context;
        }

        public async Task<List<RepairHdr>> GetAllAsync()
        {
            return await _context.RepairHdrs
                .FromSqlRaw("EXEC SP_GetAndUpdateRepairOrder @Action",
                    new SqlParameter("@Action", "GETALL"))
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<(RepairHdr Header, List<RepairOrder> Details)> GetHdrDetAsync(
    string fran,
    string brch,
    string workshop,
    string repairType,
    string repairNo,
    string customer)
        {
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SP_GetAndUpdateRepairOrder";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@Action", "GETHDRDET"));
            cmd.Parameters.Add(new SqlParameter("@REPAIRNO", repairNo));
            cmd.Parameters.Add(new SqlParameter("@FRAN", fran));
            cmd.Parameters.Add(new SqlParameter("@BRCH", brch));
            cmd.Parameters.Add(new SqlParameter("@WORKSHOP", workshop));
            cmd.Parameters.Add(new SqlParameter("@REPAIRTYPE", repairType));
            cmd.Parameters.Add(new SqlParameter("@CUSTOMER", customer));
            cmd.Parameters.Add(new SqlParameter("@JSONData", DBNull.Value));

            using var reader = await cmd.ExecuteReaderAsync();

            // ---------- HEADER ----------
            RepairHdr? header = null;

            if (await reader.ReadAsync())
            {
                header = new RepairHdr
                {
                    Fran = reader["FRAN"].ToString(),
                    Brch = reader["BRCH"].ToString(),
                    Workshop = reader["WORKSHOP"].ToString(),
                    RepairType = reader["REPAIRTYPE"].ToString(),
                    RepairNo = reader["REPAIRNO"].ToString(),
                    Customer = reader["CUSTOMER"].ToString(),
                    RepairDt = Convert.ToDateTime(reader["REPAIRDT"]),
                    Currency = reader["CURRENCY"].ToString(),
                    NoOfParts = Convert.ToDecimal(reader["NOOFPARTS"]),
                    NoOfJobs = Convert.ToDecimal(reader["NOOFJOBS"]),
                    Discount = Convert.ToDecimal(reader["DISCOUNT"]),
                    TotalValue = Convert.ToDecimal(reader["TOTALVALUE"])
                };
            }

            // ---------- DETAILS ----------
            await reader.NextResultAsync();

            var details = new List<RepairOrder>();

            while (await reader.ReadAsync())
            {
                details.Add(new RepairOrder
                {
                    Fran = reader["FRAN"].ToString(),
                    Brch = reader["BRCH"].ToString(),
                    Workshop = reader["WORKSHOP"].ToString(),
                    WorkId = Convert.ToDecimal(reader["WORKID"]),
                    RepairType = reader["REPAIRTYPE"].ToString(),
                    RepairNo = reader["REPAIRNO"].ToString(),
                    RepairSrl = reader["REPAIRSRL"].ToString(),
                    WorkType = reader["WORKTYPE"].ToString(),
                    WorkDt = Convert.ToDateTime(reader["WORKDT"]),
                    NoOfWorks = Convert.ToDecimal(reader["NOOFWORKS"]),
                    UnitPrice = Convert.ToDecimal(reader["UNITPRICE"]),
                    Discount = Convert.ToDecimal(reader["DISCOUNT"]),
                    TotalValue = Convert.ToDecimal(reader["TOTALVALUE"]),
                    Qty = Convert.ToDecimal(reader["Qty"]),
                    WorkDesc = reader["WorkDesc"].ToString(),
                    WorkDescAr = reader["WorkDescAr"].ToString()
                });
            }

            return (header!, details);
        }


        public async Task AddAsync(RepairOrderCreateDto dto)
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
                        RepairType = dto.Header.RepairType,
                        RepairDt = dto.Header.RepairDt,
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
                        RepairType = dto.Header.RepairType,
                        Customer = dto.Header.Customer,
                        RepairSrl = d.RepairSrl,
                        WorkType = d.WorkType,
                        WorkDt = d.WorkDt,
                        NoOfWorks = d.NoOfWorks,
                        UnitPrice = d.UnitPrice,
                        Discount = d.Discount,
                        TotalValue = d.TotalValue,
                        Qty = d.Qty,
                        WorkDesc = d.WorkDesc,
                        WorkDescAr = d.WorkDescAr
                    }).ToList()
                };

                string jsonData = JsonSerializer.Serialize(jsonPayload);

                var pFran = new SqlParameter("@psFran", dto.Header.Fran);
                var pPrefix = new SqlParameter("@psDOCPrefix", "WO");   // or JOB / RO
                var pMode = new SqlParameter("@Mode", "CREATE");
                var pJson = new SqlParameter("@JSONData", SqlDbType.NVarChar, -1)
                {
                    Value = jsonData
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_Save_RepairOrder @psFran, @psDOCPrefix, @Mode, @JSONData",
                    pFran, pPrefix, pMode, pJson
                );
            }
            catch
            {
                throw;
            }
        }


        public async Task UpdateAsync(RepairOrderUpdateDto dto)
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
                    RepairType = dto.Header.RepairType,
                    Customer = dto.Header.Customer,
                    Brch = dto.Header.Brch,
                    Workshop = dto.Header.Workshop
                },
                details = dto.Details.Select(d => new
                {
                    RepairSrl = d.RepairSrl,
                    RepairType = d.RepairType,
                    WorkId = d.WorkId,
                    WorkType = d.WorkType,
                    WorkDt = d.WorkDt,
                    NoOfWorks = d.NoOfWorks,
                    UnitPrice = d.UnitPrice,
                    Discount = d.Discount,
                    TotalValue = d.TotalValue,
                    CreateBy = d.CreateBy,
                    CreateRemarks = d.CreateRemarks,
                    Qty = d.Qty,
                    WorkDesc = d.WorkDesc,
                    WorkDescAr = d.WorkDescAr
                }).ToList()
            };


            string jsonData = JsonSerializer.Serialize(jsonPayload);

            var parameters = new[]
            {
                new SqlParameter("@Action", "UPDATE"),
                new SqlParameter("@REPAIRNO", dto.Header.RepairNo),
                new SqlParameter("@FRAN", dto.Header.Fran),
                new SqlParameter("@CUSTOMER", dto.Header.Customer),
                new SqlParameter("@REPAIRTYPE", dto.Header.RepairType),
                new SqlParameter("@BRCH", dto.Header.Brch),
                new SqlParameter("@WORKSHOP", dto.Header.Workshop),
                new SqlParameter("@JSONData", SqlDbType.NVarChar, -1)
                {
                    Value = jsonData
                }
        };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_GetAndUpdateRepairOrder @Action,@REPAIRNO,@REPAIRTYPE,@BRCH,@WORKSHOP,@FRAN,@CUSTOMER,@JSONData",
                parameters
            );
        }

        public async Task DeleteAsync(string fran, string customer, string repairNo)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", "DELETE"),
                new SqlParameter("@REPAIRNO", repairNo),
                new SqlParameter("@FRAN", fran),
                new SqlParameter("@CUSTOMER", customer),
                new SqlParameter("@JSONData", DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SP_GetAndUpdateRepairOrder @Action,@REPAIRNO,@FRAN,@CUSTOMER,@JSONData",
                parameters
            );
        }
    }
}

