
using ERP.Application.Interfaces.Repositories;
using ERP.Contracts.Master;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace ERP.Repositories.SALES
{
    public class SaleInvoiceRepository: ISaleInvoiceRepository
    {
        private readonly IConfiguration _configuration;

        public SaleInvoiceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> SaveSaleInvoiceAsync(SaleInvoiceDtos request)
        {
            int result = 0;

            //warning changes(02-01-2026)

            await using SqlConnection con =
                new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            await using SqlCommand cmd =
                new SqlCommand("SP_SaleinvoiceSave", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

            string json = JsonConvert.SerializeObject(request);

            cmd.Parameters.Add("@Mode", SqlDbType.NVarChar, 50).Value = "InsertCredit";
            cmd.Parameters.Add("@JSONData", SqlDbType.NVarChar).Value = json;

            await con.OpenAsync();

            await using SqlDataReader rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                result = 1;
            }

            //using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            //{
            //    using (SqlCommand cmd = new SqlCommand("SP_SaleinvoiceSave", con))
            //    {
            //        string req = JsonConvert.SerializeObject(request);
            //        // Use your actual stored procedure name
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.AddWithValue("@Mode", "InsertCredit");
            //        cmd.Parameters.AddWithValue("@JSONData", req);

            //        con.Open();
            //        using (SqlDataReader rdr = cmd.ExecuteReader())
            //        {
            //            if (rdr.HasRows)
            //            {
            //                result = 1;
            //            }
            //        }
            //    }
            //}

            return result;
        }


        public async Task<List<ParamResponse>> GetParams(ParamRequest request)
        {
            List<ParamResponse> result = new List<ParamResponse>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ERP")))
            using (SqlCommand cmd = new SqlCommand("SP_GetParams", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Mode", SqlDbType.NVarChar, 50).Value = "SaleinvoiceParams";
                cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = request.Type;
                cmd.Parameters.Add("@Fran", SqlDbType.NVarChar, 10).Value = request.Fran;

                await con.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        //warning changes(02-01-2026)
                        result.Add(new ParamResponse
                        {
                            PARAMVALUE = rdr["PARAMVALUE"] as string ?? string.Empty,
                            PARAMDESC = rdr["PARAMDESC"] as string ?? string.Empty
                        });

                        //result.Add(new paramres
                        //{
                        //    PARAMVALUE = rdr["PARAMVALUE"]?.ToString(),
                        //    PARAMDESC = rdr["PARAMDESC"]?.ToString()
                        //});
                    }
                }
            }

            return result;
        } 
        
        public async Task<List<workmasRes>> getworkmas(WorkmasReq request)
        {
            List<workmasRes> result = new List<workmasRes>();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ERP")))
            using (SqlCommand cmd = new SqlCommand("SP_GetWorkMaster", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Mode", SqlDbType.NVarChar, 50).Value = "getWork";
                cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = request.Type;
                cmd.Parameters.Add("@Fran", SqlDbType.NVarChar, 10).Value = request.Fran;

                await con.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        //warning changes(02-01-2026)
                        result.Add(new workmasRes
                        {
                            WORKID = rdr["WORKID"] as string ?? string.Empty,
                            Name = rdr["Name"] as string ?? string.Empty,
                            UNITPRICE = rdr["UNITPRICE"] as string ?? string.Empty,
                            ESTIMATED = rdr["ESTIMATED"] as string ?? string.Empty,
                            WORKTYPE = rdr["WORKTYPE"] as string ?? string.Empty

                        });

                        //result.Add(new paramres
                        //{
                        //    PARAMVALUE = rdr["PARAMVALUE"]?.ToString(),
                        //    PARAMDESC = rdr["PARAMDESC"]?.ToString()
                        //});
                    }
                }
            }

            return result;
        }

    }
}
