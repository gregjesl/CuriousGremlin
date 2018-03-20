using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CuriousGremlin.Query.Objects
{
    public class GraphEdge : GraphElement
    {
        public string inVLabel;
        public string outVLabel;
        public string inV;
        public string outV;
        public Dictionary<string, object> properties;

        public T Deserialize<T>()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (var property in properties)
            {
                object propertyValue = property.Value;
                if (propertyValue is string)
                    propertyValue = ((string)propertyValue).Replace(@"\'", @"'");
                dictionary.Add(property.Key, propertyValue);
            }
            var jobject = JObject.FromObject(dictionary);
            return jobject.ToObject<T>();
        }
    }
}
