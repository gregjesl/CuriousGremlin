using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CuriousGremlin;

namespace CuriousGremlin
{
    public abstract class GraphClient
    {
        public abstract Task<IEnumerable<object>> Execute(string query);

        public async Task<List<T>> Execute<T>(ITraversalQuery<GraphQuery, T> query)
        {
            var results = await Execute(query.ToString());
            var resultList = new List<T>();
            var objList = new List<object>();

            foreach (var item in results)
            {
                objList.Add(item);
            }
            if(objList.Count == 0)
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

        public async Task Execute(TerminalQuery<GraphClient> query)
        {
            await Execute(query.ToString());
        }
    }
}
