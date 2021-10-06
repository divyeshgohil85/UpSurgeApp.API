using Infrastructure.Quotemedia.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Quotemedia
{
    /*
     getAnalyst
https://quotemediasupport.freshdesk.com/a/solutions/articles/13000021029
XML Sample:
http://app.quotemedia.com/data/getAnalyst.xml?webmasterId=104194&symbol=msft
JSON Sample:
http://app.quotemedia.com/data/getAnalyst.json?webmasterId=104194&symbol=msft

getShareInfoBySymbols
https://quotemediasupport.freshdesk.com/a/solutions/articles/13000021344
XML Sample:
http://app.quotemedia.com/data/getShareInfoBySymbols.xml?webmasterId=104194&symbols=HYG:CA,%20AAPL,GOOG
JSON Sample:
http://app.quotemedia.com/data/getShareInfoBySymbols.json?webmasterId=104194&symbols=HYG:CA,%20AAPL,GOOG

getEnhancedEarnings
https://quotemediasupport.freshdesk.com/a/solutions/articles/13000021279
XML Sample:
http://app.quotemedia.com/data/getEnhancedEarnings.xml?excode=NSD&quarters=1&webmasterId=104194
JSON Sample:
http://app.quotemedia.com/data/getEnhancedEarnings.json?excode=NSD&quarters=1&webmasterId=104194

getQuotes
https://quotemediasupport.freshdesk.com/a/solutions/articles/13000020489
XML Sample:
http://app.quotemedia.com/data/getQuotes.xml?symbols=ORCL&webmasterId=104194
JSON Sample:
http://app.quotemedia.com/data/getQuotes.json?symbols=ORCL&webmasterId=104194

getExchangeHistory
https://quotemediasupport.freshdesk.com/a/solutions/articles/13000020865
XML Sample:
http://app.quotemedia.com/data/getExchangeHistory.xml?webmasterId=104194&exgroup=amx&date=2018-06-11
JSON Sample:
http://app.quotemedia.com/data/getExchangeHistory.json?webmasterId=104194&exgroup=amx&date=2018-06-11

getMarketStats
https://quotemediasupport.freshdesk.com/a/solutions/articles/13000021265
XML Sample:
http://app.quotemedia.com/data/getMarketStats.xml?webmasterId=104194&statExchange=nye&stat=va&statTop=1
JSON Sample:
http://app.quotemedia.com/data/getMarketStats.json?webmasterId=104194&statExchange=nye&stat=va&statTop=1

getEarningsEstimatesBySymbol
https://quotemediasupport.freshdesk.com/a/solutions/articles/13000021273
XML Sample:
http://app.quotemedia.com/data/getEarningsEstimatesBySymbol.xml?webmasterId=104194&symbol=MSFT
JSON Sample:
http://app.quotemedia.com/data/getEarningsEstimatesBySymbol.json?webmasterId=104194&symbol=MSFT

getIndustrySectorCodeList
https://quotemediasupport.freshdesk.com/a/solutions/articles/13000039936
XML Sample:
http://app.quotemedia.com/data/getIndustrySectorCodeList.xml?webmasterId=104194
JSON Sample:
http://app.quotemedia.com/data/getIndustrySectorCodeList.json?webmasterId=104194

getCompanyBySymbol
https://support.quotemedia.com/a/solutions/articles/13000021032
XML Sample:
http://app.quotemedia.com/data/getCompanyBySymbol.xml?webmasterId=104194&symbol=MSFT
JSON Sample:
http://app.quotemedia.com/data/getCompanyBySymbol.json?webmasterId=104194&symbol=MSFT

getHeadlinesStory
https://quotemediasupport.freshdesk.com/a/solutions/articles/13000020894
XML Sample:
http://app.quotemedia.com/data/getHeadlinesStory.xml?topic=msft&webmasterId=104194
JSON Sample:
http://app.quotemedia.com/data/getHeadlinesStory.json?topic=msft&webmasterId=104194

getFinancialsEnhancedBySymbol
https://quotemediasupport.freshdesk.com/support/solutions/articles/13000021450
XML Sample:
http://app.quotemedia.com/data/getFinancialsEnhancedBySymbol.xml?symbol=MSFT&reportType=A&numberOfReports=2&webmasterId=104194
JSON Sample:
http://app.quotemedia.com/data/getFinancialsEnhancedBySymbol.json?symbol=MSFT&reportType=A&numberOfReports=2&webmasterId=104194

    */
    public class QuotemediaHttpClient
    {
        const string WMID = "104194";

        private readonly HttpClient _httpClient;

        public QuotemediaHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Proxy

        public async Task<string> GetCompanyLogoAsync(string symbols, string excode)
        {
            var url = $"getCompanyLogos.json?webmasterId={WMID}&symbols={symbols}&excode={excode}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<string> GetNewsAsync(string topics, int perTopic, bool isThumbnailNeeded)
        {
            var url = $"getHeadlinesStory.json?webmasterId={WMID}&thumbnailurl={isThumbnailNeeded}&topics={topics}&perTopic={perTopic}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<string> GetQuotesAsync(params string[] symbols)
        {
            var symbolsStr = string.Join(",", symbols);
            var url = $"getQuotes.json?webmasterId={WMID}&symbols={symbolsStr}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<Root> GetQuotesModelAsync(params string[] symbols)
        {
            var symbolsStr = string.Join(",", symbols);
            var url = $"getQuotes.json?webmasterId={WMID}&symbols={symbolsStr}";

            var requestResult = await _httpClient.GetStringAsync(url);

            Root model = JsonConvert.DeserializeObject<Root>(requestResult);
            return model;
        }

        public async Task<string> GetShareInfoBySymbolsAsync(string p)
        {
            var url = $"/getShareInfoBySymbols.json?{p}&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<string> GetEnhancedEarningsAsync(string p)
        {
            var url = $"/getEnhancedEarnings.json?{p}&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<string> GetExchangeHistoryAsync(string p)
        {
            var url = $"/data/getExchangeHistory.json?{p}&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<string> GetMarketStatsAsync(string statExchange, string stat, int statTop)
        {
            var url = $"getMarketStats.json?webmasterId={WMID}&statExchange={statExchange}&stat={stat}&statTop={statTop}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<Root> GetMarketStatsModelAsync(string statExchange, string stat, int statTop)
        {
            var url = $"getMarketStats.json?webmasterId={WMID}&statExchange={statExchange}&stat={stat}&statTop={statTop}";
            var requestResult = await _httpClient.GetStringAsync(url);
            Root model = JsonConvert.DeserializeObject<Root>(requestResult);
            return model;
        }


        public async Task<string> GetEarningsEstimatesBySymbolAsync(string p)
        {
            var url = $"/data/getEarningsEstimatesBySymbol.json?{p}&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<string> GetIndustrySectorCodeListAsync(string p)
        {
            var url = $"/data/getIndustrySectorCodeList.json?{p}&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<Root> GetCompanyBySymbolModelAsync(string symbol)
        {
            var url = $"/data/getCompanyBySymbol.json?symbol={symbol}&exclude=keyratios,shareinfo&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            Root model = JsonConvert.DeserializeObject<Root>(requestResult);
            return model;
        }

        public async Task<string> GetHeadlinesStoryAsync(string p)
        {
            var url = $"/data/getHeadlinesStory.json?{p}&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<string> GetFinancialsEnhancedBySymbolAsync(string p)
        {
            var url = $"/data/getFinancialsEnhancedBySymbol.json?{p}&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<string> GetSymbolListAsync(string exgroup, char startWith)
        {
            var url = $"/data/getSymbolList.json?exgroup={exgroup}&webmasterId={WMID}&startsWith={startWith}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<Root> GetSymbolListModelAsync(string exgroup, char startWith)
        {
            var url = $"/data/getSymbolList.json?exgroup={exgroup}&webmasterId={WMID}&startsWith={startWith}";
            var requestResult = await _httpClient.GetStringAsync(url);
            var model = JsonConvert.DeserializeObject<Root>(requestResult);
            return model;
        }

        public async Task<string> GetEnhancedChartData(string symbol, DateTime startDate)
        {
            var url = $"/data/getEnhancedChartData.json?startDate={startDate:yyyy-MM-dd}&symbol={symbol}&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            return requestResult;
        }

        public async Task<Root> GetAnalystModel(string symbol)
        {
            var url = $"/data/getAnalyst.json?symbol={symbol}&webmasterId={WMID}";
            var requestResult = await _httpClient.GetStringAsync(url);
            var model = JsonConvert.DeserializeObject<Root>(requestResult);
            return model;
        }

        #endregion
    }
}
