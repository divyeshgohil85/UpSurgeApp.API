using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class AnalystRating
    {
        [Key]
        public string Symbol { get; set; }
        public string Rating { get; set; }
    }
}
