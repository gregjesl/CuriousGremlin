using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Reflection;

namespace CuriousGremlin.Objects
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

                // Check the destination member
                PropertyInfo objProp = null;
                if (Attribute.GetCustomAttribute(typeof(T), typeof(DataContractAttribute)) != null)
                {
                    objProp = typeof(T)
                        .GetProperties()
                        .Where(p => p.GetCustomAttributes(true).Where(a => a is DataMemberAttribute).Select(a => a as DataMemberAttribute).Where(a => a.Name == property.Key).Any())
                        .FirstOrDefault();
                }
                if(objProp is null)
                {
                    objProp = typeof(T).GetProperties().Where(p => p.Name == property.Key).FirstOrDefault();
                }
                if (objProp is null)
                    continue;
                if (objProp.PropertyType != typeof(string) && objProp.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    dictionary.Add(property.Key, values);
                }
                else
                {
                    dictionary.Add(property.Key, values[0]);
                }
            }
            var jobject = JObject.FromObject(dictionary);
            return jobject.ToObject<T>();
        }

        public static List<T> Deserialize<T>(IEnumerable<GraphVertex> vertices, string name = "value")
            where T: IVertexObject
        {
            List<T> result = new List<T>();
            foreach(GraphVertex vertex in vertices)
            {
                var item = vertex.Deserialize<T>();
                item.ID = vertex.id;
                result.Add(item);
            }
            return result;
        }
    }
}
