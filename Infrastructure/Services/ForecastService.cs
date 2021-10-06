using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Quotemedia.Models;
using Infrastructure.SentiOne;
using Infrastructure.Sygnal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quotemedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ForecastService : IForecastService
    {
        private readonly ILogger<ForecastService> _logger;

        private readonly string[] _exchanges = new[] { /*"nsd", "nye", "amx", "oto"*/ "nye" };
        readonly DateTime _startDate = DateTime.Today.AddDays(-1);

        string sentimentFrom = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd 00:00:00.000 CET");
        string sentimentTo = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.000 CET");

        private readonly QuotemediaHttpClient _quotemediaHttpClient;
        private readonly SentiOneHttpClient _sentiOneHttpClient;
        private readonly SygnalHttpClient _sygnalHttpClient;

        private readonly UpSurgeAppDbContext _context;

        public ForecastService(
            QuotemediaHttpClient quotemediaHttpClient,
            SentiOneHttpClient sentiOneHttpClient,
            SygnalHttpClient sygnalHttpClient,
            UpSurgeAppDbContext context, 
            ILogger<ForecastService> logger)
        {
            _quotemediaHttpClient = quotemediaHttpClient;
            _sentiOneHttpClient = sentiOneHttpClient;
            _sygnalHttpClient = sygnalHttpClient;
            _context = context;
            _logger = logger;
        }

        public async Task Calculate()
        {
            Sygnal.Sygnal[] sygnals = null;

            // If no data for sygnals just skip it.
            try
            {
                sygnals = await _sygnalHttpClient.GetSubscribedModelAsync();
            }
            catch
            {

            }            

            var forecasts = await _context.Forecasts.Include("EquityInfo")
                .Where(x => x.UpdatedDate == null || x.UpdatedDate.Value.AddDays(1) < DateTime.UtcNow)
                .ToArrayAsync();

            var sentimentTopics = await _context.SentioneTopics.ToArrayAsync();

            var failsCount = 0;
            for (var i = 0; i < forecasts.Length || failsCount >= 100; )
            {
                var forecast = forecasts[i];
                try
                {
                    await UpdateForecastAsync(forecast, sentimentTopics, sygnals);
                    _context.SaveChanges();

                    ++i;
                }                
                catch
                {
                    failsCount++;
                }
            }
        }

        private async Task UpdateForecastAsync(Forecast forecast, ICollection<SentioneTopic> sentimentTopics, Sygnal.Sygnal[] sygnals)
        {
            var symbol = forecast.StockTicker;

            var quote = (await _quotemediaHttpClient.GetQuotesModelAsync(symbol)).results?.quote?.FirstOrDefault();
            if (quote?.pricedata != null)
            {
                forecast.CurrentPrice = (decimal)quote.pricedata.last;
                forecast.DailyPriceChange = (decimal)quote.pricedata.change;
                forecast.DailyPricePercentChange = (decimal)quote.pricedata.changepercent;
            }


            var analyst = await _quotemediaHttpClient.GetAnalystModel(symbol);
            if (analyst?.results?.analyst != null)
            {
                var numReporting = analyst.results.analyst.meanRecommend?.numReporting;
                if (numReporting.HasValue)
                {
                    var upperBoundForHold = numReporting.Value * 0.55;

                    var totalSell = analyst.results.analyst.strongSell?.current + analyst.results.analyst.moderateSell?.current;
                    var totalBuy = analyst.results.analyst.strongBuy?.current + analyst.results.analyst.moderateBuy?.current;

                    if (totalSell > upperBoundForHold)
                        forecast.AnalystRating = "Sell";
                    else if (totalBuy > upperBoundForHold)
                        forecast.AnalystRating = "Buy";
                    else
                        forecast.AnalystRating = "Hold";
                }
            }

            forecast.ChartData = await _quotemediaHttpClient.GetEnhancedChartData(symbol, _startDate);

            // Sentiments
            var sentimentTopic = sentimentTopics.FirstOrDefault(x => x.Name == symbol);
            if (sentimentTopic != null)
            {
                var cnt = 5;
                while (cnt > 0)
                {
                    try
                    {
                        var sentimentTotals = await _sentiOneHttpClient.SentimentTotalAsync(new StatementsFilterRequest { TopicId = sentimentTopic.Id, From = sentimentFrom, To = sentimentTo });
                        if (sentimentTotals.Success)
                        {
                            var result = sentimentTotals.Result;
                            if (result.Positive > result.Negative && result.Positive > result.Neutral)
                                forecast.SocialMediaSentiment = 1;
                            else if (result.Negative > result.Positive && result.Negative > result.Neutral)
                                forecast.SocialMediaSentiment = -1;

                            break;
                        }
                    }
                    catch
                    {
                    }

                    await Task.Delay(1000);
                    cnt--;

                    _logger.LogInformation($"Sentiment: {sentimentTopic.Name} {cnt}");
                }
            }

            // Sygnal
            //// Sygnal > "timeHorizon": "SHORT_TERM", "modelCategory": ["TREND_FOLLOWING", "MEAN_REVERSION" - if value != 0]   
            if(sygnals != null)
            {
                var symbolSygnals = sygnals
                    .Where(x =>
                    {
                        var s = x.MarketSymbol.Split(':');
                        return s[0] == symbol && x.TimeHorizon == "SHORT_TERM";
                    })
                    .ToArray();

                var symbolSygnalTF = symbolSygnals.FirstOrDefault(x => x.ModelCategory == "TREND_FOLLOWING");
                var symbolSygnalMR = symbolSygnals.FirstOrDefault(x => x.ModelCategory == "MEAN_REVERSION");

                if (symbolSygnalMR != null && symbolSygnalMR.Value != 0)
                {
                    if (symbolSygnalMR.Value >= 0.5m && symbolSygnalMR.Value <= 1)
                        forecast.QuantRating = "Strong Buy";
                    else if (symbolSygnalMR.Value > 0 && symbolSygnalMR.Value < 0.5m)
                        forecast.QuantRating = "Buy";
                    else if (symbolSygnalMR.Value >= -1 && symbolSygnalMR.Value <= -0.5m)
                        forecast.QuantRating = "Strong Sell";
                    else if (symbolSygnalMR.Value > -0.5m && symbolSygnalMR.Value < 0)
                        forecast.QuantRating = "Sell";
                    else
                        forecast.QuantRating = "Neutral";
                }
                else if (symbolSygnalTF != null)
                {
                    if (symbolSygnalTF.Value >= 0.6m && symbolSygnalTF.Value <= 1)
                        forecast.QuantRating = "Strong Buy";
                    else if (symbolSygnalTF.Value >= 0.2m && symbolSygnalTF.Value < 0.6m)
                        forecast.QuantRating = "Buy";
                    else if (symbolSygnalTF.Value > -0.2m && symbolSygnalTF.Value < 0.2m)
                        forecast.QuantRating = "Neutral";
                    else if (symbolSygnalTF.Value > -0.6m && symbolSygnalTF.Value <= -0.2m)
                        forecast.QuantRating = "Sell";
                    else if (symbolSygnalTF.Value >= -1 && symbolSygnalTF.Value <= -0.6m)
                        forecast.QuantRating = "Strong Sell";
                }
            }

            forecast.UpdatedDate = DateTime.UtcNow;
        }

        public async Task<IEnumerable<Forecast>> GetGainers(int skip, int take)
        {
            var gainers = await GetPercentGainersAllExchangesAsync();
            var gainerSymbols = gainers.Select(x => x.symbolstring).ToArray();

            var forecasts = await _context.Forecasts.Where(x => gainerSymbols.Contains(x.StockTicker)).ToArrayAsync();

            var result = from g in gainerSymbols
                         join f in forecasts on g equals f.StockTicker
                         select f;

            return result.Skip(skip).Take(take);
        }

        public async Task<IEnumerable<Forecast>> GetLoosers(int skip, int take)
        {
            var loosers = await GetPercentLoosersAllExchangesAsync();
            var looserSymbols = loosers.Select(x => x.symbolstring).ToArray();

            var forecasts = await _context.Forecasts.Where(x => looserSymbols.Contains(x.StockTicker)).ToArrayAsync();

            var result = from g in looserSymbols
                         join f in forecasts on g equals f.StockTicker
                         select f;

            return result.Skip(skip).Take(take);
        }


        private async Task<IList<Quote>> GetPercentGainersAllExchangesAsync()
        {
            var quotes = new List<Quote>();

            foreach(var exchange in _exchanges)
            {
                var root = await _quotemediaHttpClient.GetMarketStatsModelAsync(exchange, "pg", 10);

                var exchangeQuotes = root.results.quote;
                if(exchangeQuotes != null)
                    quotes.AddRange(exchangeQuotes);
            }

            return quotes.Take(10).ToArray();
        }


        private async Task<IList<Quote>> GetPercentLoosersAllExchangesAsync()
        {
            var quotes = new List<Quote>();

            foreach (var exchange in _exchanges)
            {
                var root = await _quotemediaHttpClient.GetMarketStatsModelAsync(exchange, "pl", 10);

                var exchangeQuotes = root.results.quote;
                if (exchangeQuotes != null)
                    quotes.AddRange(exchangeQuotes);
            }

            return quotes.Take(10).ToArray();
        }

    }
}
