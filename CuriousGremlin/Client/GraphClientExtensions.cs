using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Client
{
    public static class GraphClientExtensions
    {
        public static async Task<IEnumerable<T>> Execute<T>(this IGraphClient client, TraversalQuery<GraphQuery, T> query)
        {
            var results = await client.Execute(query.ToString());
            var resultList = new List<T>();
            var objList = new List<object>();

            foreach (var item in results)
            {
                objList.Add(item);
            }
            if (objList.Count == 0)
                return resultList;
            if (objList[0].GetType() == typeof(Newtonsoft.Json.Linq.JArray))
            {
                foreach (Newtonsoft.Json.Linq.JArray item in objList)
                {
                    resultList.Add(item.ToObject<T>());
                }
            }
            else if (objList[0].GetType() == typeof(Newtonsoft.Json.Linq.JObject))
            {
                foreach (Newtonsoft.Json.Linq.JObject item in objList)
                {
                    resultList.Add(item.ToObject<T>());
                }
            }
            else
            {
                foreach (T item in objList)
                {
                    resultList.Add(item);
                }
            }
            return resultList;
        }

        public static async Task Execute(this IGraphClient client, TerminalQuery<GraphQuery> query)
        {
            await client.Execute(query.ToString());
        }
    }
}
