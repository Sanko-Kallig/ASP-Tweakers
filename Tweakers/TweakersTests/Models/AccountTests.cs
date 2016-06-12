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
    public class AccountTests
    { 
        [TestMethod()]
        public void GetAccountTest()
        {
            Account account = new Account();
            account.Email = "Sanko@test.nl";
            account.Password = "testwachtwoord";
            account = account.GetAccount(account);
            Assert.IsTrue(account.FirstName != null);
        }

        [TestMethod()]
        public void DisableAccountTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateAccountTest()
        {
            Account account = new Account();
            account.Email = "Sanko@test.nl";
            account.Password = "testwachtwoord";
            account = account.GetAccount(account);
            account.Education = "HBO";
            Assert.IsTrue(account.UpdateAccount(account));
        }

        [TestMethod()]
        public void CreateAccountTest()
        {
            Assert.Fail();
        }
    }
}