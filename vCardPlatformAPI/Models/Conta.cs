using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vCardPlatformAPI.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public string AccountOwner { get; set; }
        public float Balance { get; set; }
        public string CreatedAt { get; set; }
        public string Email { get; set; }
        public int ConfirmationCode { get; set; }

        public string Password { get; set; }
        public string Photo { get; set; } 
        public string BankId { get; set; }
        public string BankRef { get; set; }

    }
}