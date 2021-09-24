using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class EquityInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string StockTicker { get; set; }
        public string LongName { get; set; }
        public string QmDescription { get; set; }
        public string LongDescription { get; set; }

        public Forecast Forecast { get; set; }
    }
}
