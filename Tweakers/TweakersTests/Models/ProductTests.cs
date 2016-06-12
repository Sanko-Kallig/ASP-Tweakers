using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweakers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweakers.Models.Tests
{
    [TestClass()]
    public class ProductTests
    {
        [TestMethod()]
        public void GetProductTest()
        {
            Product product = new Product();
            product = product.GetProduct(1);
            Assert.IsNotNull(product.Name);
        }

        [TestMethod()]
        public void GetPriceWatchTest()
        {
            ViewModel viewModel = new ViewModel();
            viewModel.PriceWatch = viewModel.GetPriceWatch();
            Assert.IsNotNull(viewModel.PriceWatch);
        }

        [TestMethod()]
        public void GetReviewsTest()
        {
            Product product = new Product();
            product.ProductID = 2;
            Assert.IsTrue(product.GetReviews(product));
        }
    }
}