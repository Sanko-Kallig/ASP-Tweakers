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
    public class ArticleTests
    {
        [TestMethod()]
        public void AddReactionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddSubReactionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetArticlesTest()
        {
            ViewModel viewModel = new ViewModel();

            viewModel.Articles = viewModel.GetArticles();

            Assert.IsNotNull(viewModel.Articles);
        }
    }
}