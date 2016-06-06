using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class ViewModel
    {
        public List<PriceWatchCategory> PriceWatch = new List<PriceWatchCategory>();

        public ViewModel()
        {
            PriceWatch = GetPriceWatch();
        }

        private List<Product> GetProducts()
        {
            return DatabaseManager.GetProducts();
        }

        public List<PriceWatchCategory> GetPriceWatch()
        {
            return DatabaseManager.GetPriceWatch();
        }

    }
}