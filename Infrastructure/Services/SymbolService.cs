using Core.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Quotemedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ISymbolService
    {
        Task Load();
        Task LoadDescriptions();
    }

    public class SymbolService : ISymbolService
    {
        private readonly ILogger<SymbolService> _logger;

        private readonly QuotemediaHttpClient _quotemediaHttpClient;
        private readonly UpSurgeAppDbContext _context;

        public SymbolService(QuotemediaHttpClient quotemediaHttpClient, UpSurgeAppDbContext context, ILogger<SymbolService> logger)
        {
            _quotemediaHttpClient = quotemediaHttpClient;
            _context = context;
            _logger = logger;
        }

        public async Task Load()
        {
            //_context.Truncate<EquityInfo>();

            var lastRecord = _context.EquityInfos.OrderByDescending(x => x.Id).FirstOrDefault();

            var firstChar = lastRecord != null ? lastRecord.StockTicker[0] + 1 : 'A';
            
            var startWiths = Enumerable.Range(firstChar, 'Z' - firstChar + 1).Select(x => (char)x).ToArray();
            foreach(var letter in startWiths)
            {
                _logger.LogInformation($"Start Loading Equity for {letter}");

                var symbols = await _quotemediaHttpClient.GetSymbolListModelAsync("EDGX", letter);

                _logger.LogInformation($"End Request Equity for {letter}.");

                var equities = symbols?.results?.lookupdata;
                if(equities != null)
                {
                    var entities = equities
                        .Select(x => new EquityInfo { StockTicker = x.symbolstring.Split(':')[0], LongName = x.equityinfo.longname })
                        .ToArray();

                    foreach(var e in entities)
                    {
                        _context.EquityInfos.Add(e);
                        await _context.SaveChangesAsync();
                    }

                    _logger.LogInformation($"End Saving Equities for {letter}. Total Count: {entities.Length}");
                }

                _logger.LogInformation($"End Loading Equity for {letter}");
            }
        }

        public async Task LoadDescriptions()
        {
            var equities = _context.EquityInfos.Where(x => x.LongDescription == null && x.QmDescription == null).ToArray();

            foreach(var equity in equities)
            {
                var companyInfo = await _quotemediaHttpClient.GetCompanyBySymbolModelAsync(equity.StockTicker);
                if(companyInfo?.results?.company?.profile != null)
                {
                    equity.LongDescription = companyInfo.results.company.profile.longdescription;
                    equity.QmDescription = companyInfo.results.company.profile.classification?.qmdescription;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
