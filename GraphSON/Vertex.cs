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
                object propertyValue = property.Value.Find(k => k.Key.Equals(value)).Value;
                if (propertyValue is string)
                    propertyValue = ((string)propertyValue).Replace(@"\'", @"'");
                dictionary.Add(property.Key, propertyValue);
            }
            var jobject = JObject.FromObject(dictionary);
            return jobject.ToObject<T>();
        }
    }
}
