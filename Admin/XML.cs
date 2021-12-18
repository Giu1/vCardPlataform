using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Admin
{
    public class XML
    {
        [XmlAttribute]
        public string logs { get; set; }

        public void Save(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(XML));
                XML.Serialize(stream, this);
            }
        }
    }
}
