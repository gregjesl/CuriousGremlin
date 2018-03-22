using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CuriousGremlin.Query.Objects
{
    public class GraphVertex : GraphElement
    {
        public Dictionary<string, List<Dictionary<string, object>>> properties { set; get; }

        public T Deserialize<T>(string value = "value")
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (var property in properties)
            {
                if (!property.Value[0].ContainsKey(value))
                    throw new NullReferenceException("Could not find value of property");
                object propertyValue = property.Value[0][value];
                if (propertyValue is string)
                    propertyValue = ((string)propertyValue).Replace(@"\'", @"'");
                dictionary.Add(property.Key, propertyValue);
            }
            var jobject = JObject.FromObject(dictionary);
            return jobject.ToObject<T>();
        }

        public static List<T> Deserialize<T>(IEnumerable<GraphVertex> vertices, string name = "value")
        {
            List<T> result = new List<T>();
            foreach(GraphVertex vertex in vertices)
            {
                result.Add(vertex.Deserialize<T>());
            }
            return result;
        }
    }
}
