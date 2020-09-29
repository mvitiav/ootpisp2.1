using System.IO;
using System.Xml.Serialization;

namespace ConsoleApp1.serializers
{
    public class xmlSer : ISerializer
    {
        private XmlSerializer xmlSerializer = new XmlSerializer(typeof(TraceResult));

        public string serialize(TraceResult result)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, result);
                return textWriter.ToString();
            }
        }
    }
}