using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Reaction
    {
        public Account Account { get; set; }

        public DateTime PostTime { get; set; }

        public string Context { get; set; }

        private List<Reaction> SubReactions { get; set; }

        public Reaction(Account account, DateTime postTime, string context)
        {
            this.Account = account;
            this.PostTime = postTime;
            this.Context = context;
        }
    }
}