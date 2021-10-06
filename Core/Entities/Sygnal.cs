using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Sygnal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "alert1d")]
        public bool Alert1d { get; set; }

        [JsonProperty(PropertyName = "alert7d")]
        public bool Alert7d { get; set; }

        [JsonProperty(PropertyName = "createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty(PropertyName = "dateofSignalGeneration")]
        public DateTime DateofSignalGeneration { get; set; }

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

        [JsonProperty(PropertyName = "smodelUuid")]
        public Guid ModelUuid { get; set; }

        [JsonProperty(PropertyName = "timeHorizon")]
        public string TimeHorizon { get; set; }

        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }
    }
}
