using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CuriousGremlin.Query.Objects
{
    public class GraphVertex : GraphElement
    {
        public Dictionary<string, List<KeyValuePair<string, object>>> properties { set; get; }

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
