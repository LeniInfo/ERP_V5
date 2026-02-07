using ERP.Contracts.Master;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Persistence.Repositories
{
    public class QuotationRepository
    {
        private readonly IConfiguration _configuration;

        public QuotationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<QuotationRes> SaveQuotation(QuotationReq request)
        {
            List<ParamResponse> result = new();

            using SqlConnection con =
                new SqlConnection(_configuration.GetConnectionString("ERP"));

            using SqlCommand cmd = new SqlCommand("SP_GetParams", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@Mode", SqlDbType.NVarChar, 50).Value = "SaleinvoiceParams";
            cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = request.Type;
            cmd.Parameters.Add("@Fran", SqlDbType.NVarChar, 10).Value = request.Fran;

            await con.OpenAsync();

            using SqlDataReader rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                result.Add(new ParamResponse
                {
                    PARAMVALUE = rdr["PARAMVALUE"]?.ToString() ?? string.Empty,
                    PARAMDESC = rdr["PARAMDESC"]?.ToString() ?? string.Empty
                });
            }

            return new QuotationRes
            {
                Params = result
            };
        }
    }
}
