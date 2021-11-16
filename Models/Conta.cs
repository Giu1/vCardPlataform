using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vCardPlatform.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public string AccountOwner { get; set; }
        public float Balance { get; set; }
        public string CreatedAt{get;set;}


    }
}