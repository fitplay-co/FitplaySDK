using System.IO;
using Newtonsoft.Json;

public static class JsonHelper
{
    public static T Deserialize<T>(string filePath)
    {
        try {
            using(FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using(StreamReader streamReader = new StreamReader(fileStream))
                {
                    var json = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
        } catch {
            return default(T);
        }
    }

    public static bool Serialize<T>(string filePath, T obj)
    {
        try {
            using(FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using(StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(
                        JsonConvert.SerializeObject(
                            obj,
                            new JsonSerializerSettings()
                            { 
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }
                        )
                    );
                    return true;
                }
            }
        } catch {
            return false;
        }
    }
}