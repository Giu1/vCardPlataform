using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace vCardPlatformAPI.Models
{
    public class MovimentoBancario
    {
        public int Id { get; set; }

        public string Type { get; set; }
        public string IdSender { get; set; }
        public string IdBankSender { get; set; }
        public string IdReceiver { get; set; }
        public string IdBankReceiver { get; set; }

        public string Description { get; set; }
        public float Amount { get; set; }

        public string Date { get; set; }


    }

    public enum TypeOfMoviment
    {
        CREDIT,
        DEBIT
    }
}