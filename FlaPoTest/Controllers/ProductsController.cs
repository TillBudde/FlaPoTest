using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlaPoTest.Models;
using System.Net;
using Newtonsoft.Json;

namespace FlaPoTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        public Dictionary<string, IEnumerable<Product>> Products
        {
            get
            {
                if (_Products == null)
                    _Products = new Dictionary<string, IEnumerable<Product>>();
                return _Products;
            }
        }
        private Dictionary<string, IEnumerable<Product>> _Products;

        public ProductsController()
        { }


        // GET: api/Products/5
        [HttpGet("{id}")]
        public Product GetProduct(int id, string url)
        {
            return GetProductsFromUrl(url).FirstOrDefault(p => p.Id == id);
        }

        [HttpGet("mostExpensive")]
        public Result GetMostExpensiveProduct(string url)
        {
            var articles = GetArticlesAsTuple(url);
            decimal maximum;
            try
            {
                maximum = articles.Select(a => a.Item1.PricePerLiter).Max();
            }
            catch(Exception ex)
            {
                return Result.ToResult("Most Expensive");
            }
            return Result.ToResult("Most Expensive", articles.Where(a => a.Item1.PricePerLiter == maximum));
        }

        [HttpGet("cheapest")]
        public Result GetCheapestProduct(string url)
        {
            var articles = GetArticlesAsTuple(url);
            decimal minimum;
            try
            {
                minimum = articles.Select(a => a.Item1.PricePerLiter).Min();
            }
            catch(Exception ex)
            {
                return Result.ToResult("Cheapest");
            }
            return Result.ToResult("Cheapest", articles.Where(a => a.Item1.PricePerLiter == minimum));
        }

        [HttpGet("expensiveCheap")]
        public IEnumerable<Result> GetMostExpensiveCheapestProduct(string url)
        {
            yield return GetMostExpensiveProduct(url);
            yield return GetCheapestProduct(url);
        }


        [HttpGet("exactPrice/{price}")]
        public Result GetArticlesOfPrice(decimal price, string url)
        {
            var articles = GetArticlesAsTuple(url);
            return Result.ToResult($"Exact Price = {price}", articles.Where(a => a.Item1.Price == price).OrderBy(a => a.Item1.PricePerLiter));
        }

        [HttpGet("mostBottles")]
        public Result GetMostBottles(string url)
        {
            var articles = GetArticlesAsTuple(url);
            int maximum;
            try
            {
                maximum = articles.Select(a => a.Item1.NumberOfBottles).Max();
            }
            catch (Exception ex)
            {
                return Result.ToResult("Most Bottles");
            }
            return Result.ToResult("Most Bottles", articles.Where(a => a.Item1.NumberOfBottles == maximum));
        }

        [HttpGet("allQueries")]
        public IEnumerable<Result> GetImplementedQueries(string url)
        {
            foreach(Result result in GetMostExpensiveCheapestProduct(url))
                yield return result;
            yield return GetArticlesOfPrice(17.99m, url);
            yield return GetMostBottles(url);
        }

        /// <summary>
        /// Stores product-data from url in Dictionary. Only downloads data, if Dictionary does not contain an entry for the url.
        ///  Should only be used if urls contain static data. If its dynamic, we always need to request new data.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private IEnumerable<Product> GetProductsFromUrl(string url)
        {
            if (!Products.ContainsKey(url))
            {
                using (WebClient wc = new WebClient())
                {
                    string json;
                    try
                    {
                        json = wc.DownloadString(url);
                    }
                    catch(Exception ex)
                    {
                        json = "";
                    }
                    var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                    if(products != null)
                        Products.Add(url, products);
                }
            }
            if(Products.ContainsKey(url))
                return Products[url];
            return new List<Product>();
        }

        /// <summary>
        /// Create tuple of the articles and their corresponding product.
        /// Used to output productinformation for articles.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private IEnumerable<Tuple<Article, Product>> GetArticlesAsTuple(string url)
        {
            return GetProductsFromUrl(url).SelectMany(p => p.Articles.Select(a => new Tuple<Article, Product>(a, p)));
        }
    }
}
