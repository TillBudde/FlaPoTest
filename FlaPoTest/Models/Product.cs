using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace FlaPoTest.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public string DescriptionText { get; set; }
        public List<Article> Articles { get; set; }
    }

    public static class ProductExt
    {
        /// <summary>
        /// Copy Data of the Product, without the articles.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Product CopyMeta(this Product p)
        {
            var product = new Product();
            product.Id = p.Id;
            product.BrandName = p.BrandName;
            product.Name = p.Name;
            product.DescriptionText = p.DescriptionText;
            product.Articles = new List<Article>();
            return product;
        }
    }
}
