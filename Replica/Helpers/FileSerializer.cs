using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace R.Helpers
{
    public class FileSerializer
    {
        //ReadFile using UTF8 encoding
        public string GetXmlFile(string fileName)
        {
            //string fileName = @"c:\projects\testarosa\XServer\wwwroot\models\productType.xml";
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using (var sr = new StreamReader(fileStream, Encoding.UTF8))
            {
                return ConvertXmlToJson(sr.ReadToEnd());
            }
        }

        public string GetTextFile(string fileName)
        {
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using (var sr = new StreamReader(fileStream, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        //Save file as UTF8
        public void SaveFile(string fileName, string content)
        {
            SaveFile(fileName, content, Encoding.UTF8);
        }
        public void SaveFile(string fileName, string content, Encoding encoding)
        {
            var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            using (var sw = new StreamWriter(fileStream, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }

        //Convert XML to JSON
        public string ConvertXmlToJson(string xml)
        {
            var json = JsonConvert.SerializeXNode(XDocument.Parse(xml), Newtonsoft.Json.Formatting.Indented, true);
            return Regex.Replace(json, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
        }

        //enumerate files from path
        public IEnumerable<string> GetFileNames(string path, string extensionToRemove = ".xml")
        {
            List<string> ret = new List<string>();
            foreach (var s in System.IO.Directory.EnumerateFiles(path))
            {
                ret.Add(s.Substring(path.Length).Replace(extensionToRemove, ""));
            }
            return ret;
        }

        public void SerializeToDisk(object obj, string fileName, SerializationType serialization = SerializationType.JSON)
        {
            SerializeToDisk(obj, fileName, serialization, Encoding.UTF8);
        }
        public void SerializeToDisk(object obj, string fileName, SerializationType serialization, Encoding encoding)
        {
            if (serialization == SerializationType.JSON)
            {
                var content = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
                SaveFile(fileName, content, encoding);
            }
            else
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                var file = System.IO.File.Create(fileName);
                using (var writer = new System.IO.StreamWriter(file))
                {
                    var xwSettings = new XmlWriterSettings
                    {
                        Encoding = new System.Text.UTF8Encoding(false),
                        Indent = true,
                        IndentChars = "\t"
                    };

                    using (XmlWriter xmlWriter = XmlWriter.Create(writer, xwSettings))
                    {
                        serializer.Serialize(xmlWriter, obj);
                    }
                }
            }
        }
        public T DeserializeFromDisk<T>(string fileName, SerializationType serialization, Encoding encoding) where T : class
        {
            if (serialization == SerializationType.XML)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                T t;
                using (XmlReader reader = XmlReader.Create(fileName))
                {
                    t = (T)serializer.Deserialize(reader);
                }
                return t;
            }
            else
            {
                var j = new JsonHelper();
                var t=(T)j.DeserializeObject<T>(System.IO.File.ReadAllText(fileName, encoding));
                return t;
            }
        }
        public List<T> DeserializeFromDisk<T>(ICollection<string> fileNames, SerializationType serialization, Encoding encoding) where T : class
        {
            if (serialization == SerializationType.XML)
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                List<T> ret = new List<T>();
                T t;
                foreach (var f in fileNames)
                {
                    using (XmlReader reader = XmlReader.Create(f))
                    {
                        t = (T)serializer.Deserialize(reader);
                        ret.Add(t);
                    }
                }
                return ret;
            }
            else
            {
                var j = new JsonHelper();
                List<T> ret = new List<T>();
                foreach (var f in fileNames)
                {
                    ret.Add((T)j.DeserializeObject<T>(System.IO.File.ReadAllText(f, encoding)));
                }
                return ret;
            }
        }
    }
    public enum SerializationType
    {
        JSON = 0,
        XML = 1
    }
}
