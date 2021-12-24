namespace FlaPoTest.Models
{
    /// <summary>
    /// Manages the Output for HTML-queries. Holds those products that articles matches the query.
    /// Products do not contain all articles of the original ones, but only those who are result of the query.
    /// </summary>
    public class Result
    {
        public string Query { get; set; }
        public List<Product> Products { get; set; }

        public Result(string query)
        {
            this.Query = query;
            this.Products = new List<Product>();
        }

        /// <summary>
        /// Adding a product and article to the result list. If the list does not contain the product yet, we create a Copy of the product, without the List of article (just metadata).
        /// Then we add the article to the product.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="article"></param>
        public void AddProduct(Product product, Article article)
        {
            var p = Products.FirstOrDefault(p => p.Id == product.Id);
            if(p == null)
            {
                p = product.CopyMeta();
                Products.Add(p);
            }
            if (!p.Articles.Contains(article))
                p.Articles.Add(article);
        }

        /// <summary>
        /// Create a Result from a list of tuples contain articles and products.
        /// </summary>
        /// <param name="query">Describes the query of the Result.</param>
        /// <param name="tuples">Articles that result by the query, with the corresponding product.</param>
        /// <returns></returns>
        public static Result ToResult(string query, IEnumerable<Tuple<Article, Product>> tuples)
        {
            var result = new Result(query);
            foreach (var tuple in tuples)
                result.AddProduct(tuple.Item2, tuple.Item1);
            return result;
        }

        public static Result ToResult(string query)
        {
            return ToResult(query, new List<Tuple<Article, Product>>());
        }
    }
}
