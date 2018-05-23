using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace X.Helpers
{
    public class JsonHelper
    {
        public JsonSerializerSettings SerializerSettings { get; set; }

        public JsonHelper(bool formatting = true)
        {
            SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy-MM-ddThh:mm:ssZ"
            };
#if DEBUG
            if (formatting)
                SerializerSettings.Formatting = Formatting.Indented;
#endif
        }
        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, SerializerSettings);
            //return Regex.Replace(json, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
        }

        public object DeserializeObject<T>(dynamic value) where T : class
        {
            return JsonConvert.DeserializeObject<T>(value, SerializerSettings);           
        }

        public object DeserializeObject<T>(string value) where T : class
        {
            return JsonConvert.DeserializeObject<T>(value, SerializerSettings);
        }

        public object DeserializeObject(dynamic value, Type type)
        {
            return JsonConvert.DeserializeObject(value.ToString(), type, SerializerSettings);
        }

        public List<T> DeserializeFromDisk<T>(ICollection<string> filePaths) where T : class
        {
            var ret = new List<T>();
            foreach (var f in filePaths)
                ret.Add(DeserializeFromDisk<T>(f));
            return ret;
        }

        public T DeserializeFromDisk<T>(string filePath) where T : class
        {
            var f = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            return (T)DeserializeObject<T>(f);
        }

        public Newtonsoft.Json.Linq.JObject ConvertTo(object o)
        {
            return Newtonsoft.Json.Linq.JObject.Parse(SerializeObject(o));
        }
        object lockObj = new object();
        public void SerializeToDisk(string path, object value)
        {
            lock (lockObj)
            {
                System.IO.File.WriteAllText(path, this.SerializeObject(value));
            }
        }

        public static bool ValidateJson(string json)
        {
            try
            {
                JToken.Parse(json);
                return true;
            }
            catch (Exception)
            {                
                return false;
            }
        }
    }
}
