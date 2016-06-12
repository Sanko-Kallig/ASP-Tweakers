using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Reaction
    {
        public int Id { get; set; }
        public Account Account { get; set; }

        public DateTime PostTime { get; set; }

        public string Context { get; set; }

        public List<Reaction> SubReactions { get; set; }

        public Reaction(int id, Account account, DateTime postTime, string context)
        {
            this.Id = id;
            this.Account = account;
            this.PostTime = postTime;
            this.Context = context;
        }
    }
}