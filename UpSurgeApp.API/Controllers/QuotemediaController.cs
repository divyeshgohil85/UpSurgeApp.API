using Core.Entities;
using Infrastructure.Quotemedia.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Quotemedia;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class QuotemediaController : ControllerBase
    {
        private readonly QuotemediaHttpClient _quotemediaHttpClient;
        private readonly IForecastService _forecastService;
        private readonly ISymbolService _symbolService;

        public QuotemediaController(QuotemediaHttpClient quotemediaHttpClient, IForecastService forecastService, ISymbolService symbolService)
        {
            _quotemediaHttpClient = quotemediaHttpClient;
            _forecastService = forecastService;
            _symbolService = symbolService;
        }

        [HttpGet("getMarketStats.json")]
        public async Task<string> GetAnalystAsync(string statExchange, string stat, int statTop)
        {
            var requestResult = await _quotemediaHttpClient.GetMarketStatsAsync(statExchange, stat, statTop);
            return requestResult;
        }

        [HttpGet("getCompanyLogos.json")]
        public async Task<string> GetCompanyLogoAsync(string symbols, string excode)
        {
            var requestResult = await _quotemediaHttpClient.GetCompanyLogoAsync(symbols, excode);
            return requestResult;
        }

        [HttpGet("GetCompanyBySymbol")]
        public async Task<Root> GetCompanyBySymbolAsync(string symbol)
        {
            var requestResult = await _quotemediaHttpClient.GetCompanyBySymbolModelAsync(symbol);
            return requestResult;
        }

        [HttpGet("getHeadlinesStory.json")]
        public async Task<string> GetNewsAsync(string topics, int perTopic, bool isThumbnailNeeded)
        {
            var requestResult = await _quotemediaHttpClient.GetNewsAsync(topics, perTopic, isThumbnailNeeded);
            return requestResult;
        }

        [HttpGet("getQuotes.json")]
        public async Task<string> GetStockSwings([FromQuery]string[] symbols)
        {
            var requestResult = await _quotemediaHttpClient.GetQuotesAsync(symbols);
            return requestResult;
        }

        [HttpGet("GetMarketStats")]
        public async Task<string> GetMarketStatsAsync()
        {
            return await _quotemediaHttpClient.GetMarketStatsAsync("nye", "va", 5);
        }

        [HttpGet("GetMarketStatsModel")]
        public async Task<Root> GetMarketStatsModelAsync()
        {
            return await _quotemediaHttpClient.GetMarketStatsModelAsync("nye", "va", 5);
        }

        [HttpGet("GetEnhancedChartData")]
        public async Task<string> GetEnhancedChartData(string symbol, DateTime startDate)
        {
            return await _quotemediaHttpClient.GetEnhancedChartData(symbol, startDate);
        }

        [HttpGet("GetAnalyst")]
        public async Task<Root> GetAnalyst()
        {
            return await _quotemediaHttpClient.GetAnalystModel("tsla");
        }

        //[HttpGet("LoadSymbols")]
        //public async Task<ActionResult> GetSymbols()
        //{
        //    await _symbolService.Load();
        //    return Ok();
        //}

        //[HttpGet("LoadDescriptions")]
        //public async Task<ActionResult> GetSymbols()
        //{
        //    await _symbolService.LoadDescriptions();
        //    return Ok();
        //}

        [HttpGet("Forecast")]
        public async Task<ActionResult> GetForecast()
        {
            try
            {
                await _forecastService.Calculate();
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }            
            return Ok();
        }

        [HttpGet("GetGainersForecast")]
        public async Task<IEnumerable<Forecast>> GetGainersForecast(int skip, int take)
        {
            return await _forecastService.GetGainers(skip, take);
        }

        [HttpGet("GetLoosersForecast")]
        public async Task<IEnumerable<Forecast>> GetLoosersForecast(int skip, int take)
        {
            return await _forecastService.GetLoosers(skip, take);
        }

    }
}
