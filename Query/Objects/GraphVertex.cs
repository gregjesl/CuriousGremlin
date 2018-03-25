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
                List<object> values = new List<object>();
                foreach (var item in property.Value)
                {
                    if (!item.ContainsKey(value))
                        throw new NullReferenceException("Could not find value of property");
                    object propertyValue = item[value] is string ? ((string)item[value]).Replace(@"\'", @"'") : item[value];
                    values.Add(propertyValue);
                }
                if(values.Count > 1)
                    dictionary.Add(property.Key, values);
                else
                    dictionary.Add(property.Key, values[0]);
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
