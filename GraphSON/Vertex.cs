using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CuriousGremlin.GraphSON
{
    public class Vertex
    {
        public string id;

        public string label;

        public Dictionary<string, List<KeyValuePair<string,object>>> properties;

        public T Deserialize<T>(string value = "value")
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (var property in properties)
            {
                dictionary.Add(property.Key, property.Value.Find(k => k.Key.Equals(value)).Value);
            }
            var jobject = JObject.FromObject(dictionary);
            return jobject.ToObject<T>();
        }
    }
}
