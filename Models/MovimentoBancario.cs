using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;

namespace vCardPlatform.Models
{
    public class MovimentoBancario
    {
        public MovimentoBancario()
        {
            this.Amount = float.MinValue;
        }
        public string Id { get; set; }

        public TypeOfMoviment Type { get; set; }

        
        public string IdSender { get; set; }
        public string BankRefSender { get; set; }
        public string IdReceiver { get; set; }
        public string BankRefReceiver { get; set; }

        public string Description { get; set; }

        public float Amount { get; set; }

        public string Date { get; set; }


    }

    
}