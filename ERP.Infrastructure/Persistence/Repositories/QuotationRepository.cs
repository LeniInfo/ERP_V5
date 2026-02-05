using ERP.Application.Interfaces.Repositories;
using ERP.Contracts.Master;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Persistence.Repositories
{
    public class QuotationRepository: IQuotationRepository
    {
        private readonly IConfiguration _configuration;

        public QuotationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<QuoteRes>> SaveQuotation(QuotationReq request)
        {
            List<QuoteRes> result = new List<QuoteRes>();
            string json = JsonConvert.SerializeObject(request);
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ERP")))
            using (SqlCommand cmd = new SqlCommand("SP_QuoteSave", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Mode", SqlDbType.NVarChar, 50).Value = "InsertCredit";
                cmd.Parameters.Add("@JSONData", SqlDbType.NVarChar).Value = json;

                await con.OpenAsync();

                using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                {
                    while (await rdr.ReadAsync())
                    {
                        result.Add(new QuoteRes
                        {
                            FRAN = rdr["FRAN"]?.ToString() ?? string.Empty,
                            BRCH = rdr["BRCH"]?.ToString() ?? string.Empty,
                            WHSE = rdr["WHSE"]?.ToString() ?? string.Empty,
                            QUOTATIONTYPE = rdr["QUOTATIONTYPE"]?.ToString() ?? string.Empty,
                            QUOTATIONNO = rdr["QUOTATIONNO"]?.ToString() ?? string.Empty,
                            QUOTATIONSRL = rdr["QUOTATIONSRL"]?.ToString() ?? string.Empty,
                            QUOTATIONDT = rdr["QUOTATIONDT"]?.ToString() ?? string.Empty,

                            MAKE = rdr["MAKE"]?.ToString() ?? string.Empty,
                            PART = rdr["PART"]?.ToString() ?? string.Empty,
                            WORKID = rdr["WORKID"]?.ToString() ?? string.Empty,

                            QTY = rdr["QTY"]?.ToString() ?? string.Empty,
                            ACCPQTY = rdr["ACCPQTY"]?.ToString() ?? string.Empty,
                            NOTAVLQTY = rdr["NOTAVLQTY"]?.ToString() ?? string.Empty,
                            UNITPRICE = rdr["UNITPRICE"]?.ToString() ?? string.Empty,
                            DISCOUNT = rdr["DISCOUNT"]?.ToString() ?? string.Empty,
                            VATPERCENTAGE = rdr["VATPERCENTAGE"]?.ToString() ?? string.Empty,
                            VATVALUE = rdr["VATVALUE"]?.ToString() ?? string.Empty,
                            DISCOUNTVALUE = rdr["DISCOUNTVALUE"]?.ToString() ?? string.Empty,
                            TOTALVALUE = rdr["TOTALVALUE"]?.ToString() ?? string.Empty,

                            CREATEDT = rdr["CREATEDT"]?.ToString() ?? string.Empty,
                            CREATETM = rdr["CREATETM"]?.ToString() ?? string.Empty,
                            CREATEBY = rdr["CREATEBY"]?.ToString() ?? string.Empty,
                            CREATEREMARKS = rdr["CREATEREMARKS"]?.ToString() ?? string.Empty,

                            UPDATEDT = rdr["UPDATEDT"]?.ToString() ?? string.Empty,
                            UPDATETM = rdr["UPDATETM"]?.ToString() ?? string.Empty,
                            UPDATEBY = rdr["UPDATEBY"]?.ToString() ?? string.Empty,
                            UPDATEREMARKS = rdr["UPDATEREMARKS"]?.ToString() ?? string.Empty
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
