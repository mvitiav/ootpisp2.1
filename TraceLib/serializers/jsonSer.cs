//using Newtonsoft.Json;

using Newtonsoft.Json;

namespace TraceLib.serializers
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