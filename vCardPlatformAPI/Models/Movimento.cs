using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vCardPlatform.Models;

namespace vCardPlatformAPI.Models
{
    public class Movimento
    {

        public Movimento()
        {
            this.Amount = float.MinValue;
        }
        public string Id { get; set; }

        public TypeOfMoviment Type { get; set; }
        public string IdSender { get; set; }

        public string IdReceiver { get; set; }

        public string Description { get; set; }
        public float Amount { get; set; }

        public string Date { get; set; }


    }
    
}