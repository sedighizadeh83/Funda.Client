using System;
using Newtonsoft.Json;

namespace Funda.Client.DTOs
{
    public class PropertiesForSale
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("GlobalId")]
        public int GlobalId { get; set; }

        [JsonProperty("Adres")]
        public string Address { get; set; }

        [JsonProperty("Soort-aanbod")]
        public string PropertyType { get; set; }

        [JsonProperty("MakelaarId")]
        public int RealEstateAgentId { get; set; }

        [JsonProperty("MakelaarNaam")]
        public string RealEstateAgentName { get; set; }
    }
}
