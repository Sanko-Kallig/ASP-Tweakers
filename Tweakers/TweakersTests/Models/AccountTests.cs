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
            Account account = new Account();
            account.UserName = "TestUser";
            account.Email = "tester@test.nl";
            account.FirstName = "Mr Test";
            account.Password = "testmethod";
            Assert.IsTrue(account.CreateAccount(account));
        }
    }
}