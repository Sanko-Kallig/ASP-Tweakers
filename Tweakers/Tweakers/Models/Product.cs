using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }

        public string Specifications { get; set; }
        public PriceWatchCategory Category { get; set; }

        public Product(int productId, string name, string specifications, double price)
        {
            this.ProductID = productId;
            this.Name = name;
            this.Specifications = specifications;
            this.Price = price;
        }
    }
     
}