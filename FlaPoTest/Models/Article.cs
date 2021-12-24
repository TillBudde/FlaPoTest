using System.Text.Json.Serialization;

namespace FlaPoTest.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        [JsonIgnore]
        public int NumberOfBottles
        {
            get
            {
                return int.Parse(ShortDescription.Split(" ")[0]);
            }
        }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string PricePerUnitText { get; set; }
        [JsonIgnore]
        public decimal PricePerLiter
        {
            get
            {
                var priceText = String.Concat(PricePerUnitText.Split(" ")[0].Skip(1));
                return Decimal.Parse(priceText);
            }
        }
        public string Image { get; set; }
    }
}
