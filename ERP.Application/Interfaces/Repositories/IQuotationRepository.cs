using ERP.Contracts.Master;
using ERP.Domain.Entities;

namespace ERP.Application.Interfaces.Repositories;

public interface IQuotationRepository

{
    Task<List<QuoteRes>> SaveQuotation(QuotationReq request);
}
