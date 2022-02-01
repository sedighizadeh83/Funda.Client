using System.Collections.Generic;
using Newtonsoft.Json;

namespace Funda.Client.DTOs
{
    public class DataObject
    {
        [JsonProperty("Objects")]
        public List<PropertiesForSale> PropertiesForSale { get; set; } = new List<PropertiesForSale>();

        public int TotalPages { get; set; }

        [JsonProperty("TotaalAantalObjecten")]
        public int TotalNumberOfItems { get; set; }
    }
}
