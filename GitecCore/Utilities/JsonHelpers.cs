using Newtonsoft.Json;

namespace Gitec.Utilities;

public static class JsonHelpers
{
    public static string SerializeObject(object obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }

    public static T DeserializeObject<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json) ?? throw new InvalidOperationException();
    }

    public static object DeserializeObject(string json, Type type)
    {
        return JsonConvert.DeserializeObject(json, type) ?? throw new InvalidOperationException();
    }
    
}