using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Documents.Client;
using CuriousGremlin.Query;
using Newtonsoft.Json;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.AzureCosmosDB
{
    public class GraphClient : IDisposable
    {
        private static GraphClientPool Pool;

        public static void CreatePool(string endpoint, string authKey, string database, string graph)
        {
            Pool = new GraphClientPool(endpoint, authKey, database, graph);
        }

        public static void DisposePool()
        {
            if (Pool == null)
                throw new NullReferenceException("The client pool has not been insantiated");
            Pool.Dispose();
            Pool = null;
        }

        public static async Task<GraphClient> FromPool()
        {
            if (Pool == null)
                throw new NullReferenceException("The client pool has not been insantiated");
            return await Pool.GetClient();
        }

        public bool IsOpen { get; private set; } = false;
        internal GraphClientPool pool;
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private DocumentClient client;
        private DocumentCollection graph;

        public GraphClient(string endpoint, string authKey)
        {
            client = new DocumentClient(new Uri(endpoint), authKey);
        }

        internal GraphClient(GraphClientPool pool)
        {
            this.pool = pool;
            client = new DocumentClient(new Uri(pool.Endpoint), pool.AuthKey);
        }

        ~GraphClient()
        {
            if(IsOpen)
                Dispose();
        }

        public async Task Open(string database, string graph)
        {
            if(!IsOpen)
            {
                this.graph = await client.ReadDocumentCollectionAsync("/dbs/" + database + "/colls/" + graph);
                this.IsOpen = true;
            }
        }

        public virtual void Dispose()
        { 
            if (pool != null)
                pool.ReturnToPool(this);
            else
            {
                IsOpen = false;
                client.Dispose();
            }
        }
        #region Database Operations
        public async Task CreateDatabaseAsync(string id)
        {
            await client.CreateDatabaseAsync(new Database { Id = id });
        }

        public async Task CreateDatabaseIfNotExistsAsync(string id)
        {
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = id });
        }

        public async Task DeleteDatabaseAsync(string id)
        {
            await client.DeleteDatabaseAsync("/dbs/" + id);
        }
        #endregion

        #region Collection Operations
        public async Task CreateDocumentCollectionAsync(string database, string collection)
        {
            await client.CreateDocumentCollectionAsync("/dbs/" + database, new DocumentCollection { Id = collection });
        }

        public async Task CreateDocumentCollectionIfNotExistsAsync(string database, string collection)
        {
            await client.CreateDocumentCollectionIfNotExistsAsync("/dbs/" + database, new DocumentCollection { Id = collection });
        }

        public async Task DeleteCollectionAsync(string database, string collection)
        {
            await client.DeleteDocumentCollectionAsync("/dbs/" + database + "/colls/" + collection);
        }
        #endregion

        #region Queries
        public async Task<FeedResponse<object>> Execute(string queryString)
        {
            if (!IsOpen)
                throw new Exception("Client must be opened prior to executing a query");

            // Generate the query from the query string
            var query = GraphExtensions.CreateGremlinQuery(client, graph, queryString);

            // Get the semaphore
            await semaphore.WaitAsync();
            try
            {
                return await query.ExecuteNextAsync<dynamic>();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<List<T>> Execute<T>(ITraversalQuery<GraphQuery, T> query)
        {
            var results = await Execute("g." + query.ToString());
            var resultList = new List<T>();
            var objList = new List<object>();
            if (results.Count == 0)
                return resultList;
            foreach(var item in results)
            {
                objList.Add(item);
            }
            if (objList[0].GetType() == typeof(Newtonsoft.Json.Linq.JArray))
            {
                foreach (Newtonsoft.Json.Linq.JArray item in objList)
                {
                    resultList.Add(item.ToObject<T>());
                }
            }
            else if (objList[0].GetType() == typeof(Newtonsoft.Json.Linq.JObject))
            {
                foreach(Newtonsoft.Json.Linq.JObject item in objList)
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

        public async Task Execute(TerminalQuery<Graph> query)
        {
            await Execute("g." + query.ToString());
        }
        #endregion
    }
}
