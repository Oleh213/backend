using System;
using System.Text.Json.Serialization;
using WebShop.Main.Conext;

namespace WebShop.Main.Context
{
	public class Characteristics
	{
        public Guid CharacteristicsId { get; set; }

        public string CharacteristicName { get; set; }

        public string CharacteristicValue { get; set; }

        [JsonIgnore]
        public ICollection<Product> Product { get; set; }

    }
}


