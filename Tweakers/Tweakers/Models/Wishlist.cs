using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Wishlist
    {
        public List<Product> Products { get; set; }

        public Account Owner { get; set; }

        public double TotalPrice { get; set; }

        public Wishlist(List<Product> products, Account owner)
        {
            Products = products;
            Owner = owner;
        }

        public void CalculateTotalPrice()
        {
            TotalPrice = 0;
        }
    }
}