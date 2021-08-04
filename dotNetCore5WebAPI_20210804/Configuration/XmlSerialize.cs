using dotNetCore5WebAPI_0323.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace dotNetCore5WebAPI_0323
{
    public class XmlSerialize : ISerialize
    {
        public string Serialize(object o)
        {
            var xml = new XmlSerializer(o.GetType());
            using (var ms = new MemoryStream())
            {
                xml.Serialize(ms, o);
                return Utils.ToString(ms.ToArray());
            }
        }
    }
}
