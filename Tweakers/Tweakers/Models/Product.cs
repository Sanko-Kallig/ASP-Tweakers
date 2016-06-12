using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        public string Name { get; set; }

        public double Price { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Specifications { get; set; }
        public PriceWatchCategory Category { get; set; }

        public List<Review> Reviews { get; set; }

        public Product()
        {
            
        }
        public Product(int productId, string name, string specifications, double price)
        {
            this.ProductID = productId;
            this.Name = name;
            this.Specifications = specifications;
            this.Price = price;
        }

        public Product GetProduct(int id)
        {
            return DatabaseManager.GetProduct(id);
        }

        public bool GetReviews(Product product)
        {
            Reviews = DatabaseManager.GetReviews(product);
            return Reviews != null;
        }

        public bool AddProduct(Product product, int catID)
        {
            return DatabaseManager.AddProduct(product, catID);
        }

        public bool UpdateProduct(Product product)
        {
            return DatabaseManager.UpdateProduct(product);
        }
    }
     
}