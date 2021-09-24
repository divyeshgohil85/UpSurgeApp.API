using Core.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class MatchmakerUserPreferencesValues
    {
        public int UserId { get; set; }
        public LowMediumHigh? RiskLevel { get; set; }
        public LowMediumHigh? AnalystRating { get; set; }
        public LowMediumHigh? SocialMediaRating { get; set; }
        public LowMediumHigh? Horizon { get; set; }
        public LowMediumHigh? QuantitativeRating { get; set; }
        public bool? Dividend { get; set; }
        public string Popularity { get; set; }
        public string ProjectedGrowth { get; set; }
        public bool? MeanReversionDataUsed { get; set; }
        public string Keywords { get; set; }
    }

    public class MatchmakerUserPreference
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public MatchmakerUserPreferencesValues Values { get; set; }
    }
}
