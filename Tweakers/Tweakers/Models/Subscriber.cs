using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Subscriber : Account
    {
        public string BankNumber { get; set; }

        public string Address { get; set; }

        public Subscriber(string bankNumber, string address)
        {
            BankNumber = bankNumber;
            Address = address;
        }
    }
}