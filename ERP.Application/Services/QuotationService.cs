using ERP.Application.Abstractions.Logging;
using ERP.Application.Interfaces.Repositories;
using ERP.Application.Interfaces.Services;
using ERP.Contracts.Master;
using ERP.Domain.Entities;
using System.Collections.Generic;

namespace ERP.Application.Services;

public sealed class QuotationService : IQuotationService
{

    private readonly IQuotationRepository _repo;
    private readonly IAppLogger<QuotationService> _log;

    public QuotationService(
        IQuotationRepository repo,
        IAppLogger<QuotationService> log)
    {
        _repo = repo;
        _log = log;
    }


    public async Task<List<QuoteRes>> SaveQuotation(QuotationReq request)
    {
        List < QuoteRes > result =await  _repo.SaveQuotation(request);
        return result;
    }

   
}
