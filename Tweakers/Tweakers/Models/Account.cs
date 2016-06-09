using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tweakers.Models
{
    public class Account
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        public string Type { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Function { get; set; }

        public string Education { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool CreateAccount(Account account)
        {
            return DatabaseManager.CreateAccount(account);
        }

        public bool UpdateAccount(Account account)
        {
            return DatabaseManager.UpdateAccount(account);
        }

        public bool DisableAccount(Account account)
        {
            return DatabaseManager.DisableAccount(account);
        }

        public Account GetAccount(Account account)
        {
            return DatabaseManager.GetAccount(account);
        }
    }
}