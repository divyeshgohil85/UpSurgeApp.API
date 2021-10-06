using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Forecast
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string StockTicker { get; set; }
        public string ChartData { get; set; }
        public string CompanyName { get; set; }
        public decimal? DailyPriceChange { get; set; }
        public decimal? CurrentPrice { get; set; }
        public string AnalystRating { get; set; }
        public int? SocialMediaSentiment { get; set; } // -1 - Negative, 0 - Neutral, 1 - Positive
        public decimal? DailyPricePercentChange { get; set; }
        public string QuantRating { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public EquityInfo EquityInfo { get; set; }
    }
}
