using Infrastructure.Data;
using Quotemedia;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IAnalystRatingService
    {
        Task Load();
    }

    public class AnalystRatingService : IAnalystRatingService
    {
        private readonly QuotemediaHttpClient _quotemediaHttpClient;
        private readonly UpSurgeAppDbContext _context;

        public AnalystRatingService(QuotemediaHttpClient quotemediaHttpClient, UpSurgeAppDbContext context)
        {
            _quotemediaHttpClient = quotemediaHttpClient;
            _context = context;
        }

        public async Task Load()
        {
            var analystRatings = _context.AnalystRatings.ToArray();
            foreach(var analystRating in analystRatings)
            {
                analystRating.Rating = null;

                var analyst = await _quotemediaHttpClient.GetAnalystModel(analystRating.Symbol);
                if (analyst?.results?.analyst != null)
                {
                    var numReporting = analyst.results.analyst.meanRecommend?.numReporting;
                    if (numReporting.HasValue)
                    {
                        var upperBoundForHold = numReporting.Value * 0.55;

                        var totalSell = analyst.results.analyst.strongSell?.current + analyst.results.analyst.moderateSell?.current;
                        var totalBuy = analyst.results.analyst.strongBuy?.current + analyst.results.analyst.moderateBuy?.current;

                        if (totalSell > upperBoundForHold)
                            analystRating.Rating = "Sell";
                        else if (totalBuy > upperBoundForHold)
                            analystRating.Rating = "Buy";
                        else
                            analystRating.Rating = "Hold";
                    }
                }

                _context.SaveChanges();
            }
        }
    }
}
