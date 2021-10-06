using Infrastructure.SentiOne;
using Infrastructure.Sygnal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class SentiOneController : ControllerBase
    {
        private readonly SentiOneHttpClient _sentiOneHttpClient;
        private readonly SygnalHttpClient _sygnalHttpClient;

        public SentiOneController(SentiOneHttpClient sentiOneHttpClient, SygnalHttpClient sygnalHttpClient)
        {
            _sentiOneHttpClient = sentiOneHttpClient;
            _sygnalHttpClient = sygnalHttpClient;
        }

        [HttpGet("subscribed")]
        public async Task<string> GetSubscribedAsync()
        {
            var requestResult = await _sygnalHttpClient.GetSubscribedAsync();
            return requestResult;
        }

        [HttpGet("kpis")]
        public async Task<string> GetKpisAsync(string marketSymbol, Guid modeluuid)
        {
            var requestResult = await _sygnalHttpClient.GetKpisAsync(marketSymbol, modeluuid);
            return requestResult;
        }

        [HttpPost("SentimentTopics")]
        public async Task<ICollection<Topic>> SentimentTopicsAsync()
        {
            return await _sentiOneHttpClient.GetTopicsAsync();
        }

        [HttpPost("SentimentTime")]
        public async Task<SentimentTimeHistogramResponse> SentimentTimeAsync(HistogramRequest apiRequest)
        {
            return await _sentiOneHttpClient.SentimentTimeAsync(apiRequest);
        }

        [HttpPost("SentimentTotal")]
        public async Task<SentimentTotalResponse> SentimentTotalAsync(StatementsFilterRequest apiRequest)
        {
            return await _sentiOneHttpClient.SentimentTotalAsync(apiRequest);
        }

        [HttpPost("SentimentStrength")]
        public async Task<SentimentStrengthResponse> SentimentStrengthAsync(StatementsFilterRequest apiRequest)
        {
            return await _sentiOneHttpClient.SentimentStrengthAsync(apiRequest);
        }
    }
}
