using Newtonsoft.Json;

namespace ConsoleApp1.serializers
{
    public class jsonSer : ISerializer
    {
        public string serialize(TraceResult result)
        {
            //optional
            //return JsonConvert.SerializeObject(result);
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
    }
}