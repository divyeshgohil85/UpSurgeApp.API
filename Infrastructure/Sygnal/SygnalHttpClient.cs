using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Sygnal
{
    public class SygnalHttpClient
    {
        private readonly HttpClient _httpClient;

        public SygnalHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetSubscribedAsync()
        {
            var url = "signals/subscribed";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<Sygnal[]> GetSubscribedModelAsync()
        {
            var url = "signals/subscribed";
            var requestResult = await _httpClient.GetStringAsync(url);
            var model = JsonConvert.DeserializeObject<Sygnal[]>(requestResult);
            return model;
        }

        public async Task<string> GetKpisAsync(string marketSymbol, Guid modeluuid)
        {
            var url = $"signals/kpis?marketsymbol={marketSymbol}&smodeluuid={modeluuid}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }
    }

    public class Sygnal
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "alert1d")]
        public bool Alert1d { get; set; }

        [JsonProperty(PropertyName = "alert7d")]
        public bool Alert7d { get; set; }

        [JsonProperty(PropertyName = "hedgeDirection")]
        public string HedgeDirection { get; set; }

        [JsonProperty(PropertyName = "instrument")]
        public string Instrument { get; set; }

        [JsonProperty(PropertyName = "marketSymbol")]
        public string MarketSymbol { get; set; }

        [JsonProperty(PropertyName = "modelCategory")]
        public string ModelCategory { get; set; }

        [JsonProperty(PropertyName = "ret_pa")]
        public decimal RetPa { get; set; }

        [JsonProperty(PropertyName = "smodelName")]
        public string ModelName { get; set; }

        [JsonProperty(PropertyName = "timeHorizon")]
        public string TimeHorizon { get; set; }

        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }
    }
}
