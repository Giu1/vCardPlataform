using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using vCardPlatformAPI.Models;

namespace Admin
{
    [Serializable]
    internal class Local_Admin : ISerializable
    {
        public Local_Admin(string id, string email, string password, string nome, int enabled)
        {
            Id = id;
            Email = email;
            Password = password;
            Nome = nome;
            Enabled = enabled;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nome { get; set; }

        public int Enabled { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("Email", Email);
            info.AddValue("Password", Password);
            info.AddValue("Nome", Nome);
            info.AddValue("Enabled", Enabled);
        }
    }
}
