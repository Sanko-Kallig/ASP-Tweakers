using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class PriceWatchCategory
    {
        public int ID { get; set; }

        public string Name { get; set; }
        
        public int SubID { get; set; }
        public PriceWatchCategory ParentCat { get; set; }

        public List<Product> CatProducts { get { return DatabaseManager.GetCatProducts(ID); } }

        public PriceWatchCategory(int id, string name, int subId, PriceWatchCategory parentCat)
        {
            this.ID = id;
            this.Name = name;
            this.SubID = subId;
            this.ParentCat = parentCat;
        }
    }
}