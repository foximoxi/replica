using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using R.Component;
using R.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace R.Services
{
    public class PseudoDbService: IPseudoDbService
    {
        public Dictionary<string, Collection> ObjectCollections { get; set; } = new Dictionary<string, Collection>();

        public void AddCollection(string name,string identityName,string path)
        {
            if (!ObjectCollections.ContainsKey(name))
            {
                var coll = new Collection() { Name = name, IdentityFieldName = identityName, CollectionPath = path };
                coll.Init();
                ObjectCollections[name] = coll;
            }
        }
    }

    public class Collection
    {
        public string Name { get; set; }
        public string CollectionPath { get; set; }
        public Dictionary<string, JObject> Values { get; set; }
        public string IdentityFieldName { get; set; }
        public Type IdentityFieldType { get; set; }

        public void Init()
        {
            IndexCollection();
        }

        public void Insert(string json)
        {
            var jo = JObject.Parse(json);
            var id = (string)jo[IdentityFieldName];
            if (id == null)
                jo[IdentityFieldName] = SetNewIdentity();
            Save(jo, id);
        }
        
        public void Update(string json)
        {
            var jo = JObject.Parse(json);
            var id = (string)jo[IdentityFieldName];
            if (id == null)
                throw new Exception("Not existing resource");
            else
                Save(jo, id);
        }

        public void Delete(string identity)
        {
            if (Values.ContainsKey(identity))
            {
                var path = Path.Combine(CollectionPath, identity, ".json");
                File.Delete(path);
            }
        }

        public string Get(string identity)
        {
            return JsonConvert.SerializeObject(Values[identity]);
        }

        public string GetAll(string identity)
        {
            var array = new JArray();
            foreach (var v in Values.Values)
                array.Add(v);            
            return JsonConvert.SerializeObject(array);
        }

        void Save(JObject j, string id)
        {
            var path=Path.Combine(CollectionPath, id, ".json");
            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(j), Encoding.UTF8);
        }

        string SetNewIdentity()
        {
            string ret = "";
            switch (IdentityFieldType)
            {
                case Type t when t==typeof(Int64):
                        ret = (Convert.ToInt64(Values.Keys.Max()) + 1).ToString();                    
                    break;
            }
            return ret;
        }

        void IndexCollection()
        {
            Values = new Dictionary<string, JObject>();
            foreach (var f in Directory.EnumerateFiles(CollectionPath, "*.json"))
            {
                var json = File.ReadAllText(f, Encoding.UTF8);
                var jo = JObject.Parse(json);
                var id = (string)jo[IdentityFieldName];
                Values[id] = jo;
            }
        }
    }
}