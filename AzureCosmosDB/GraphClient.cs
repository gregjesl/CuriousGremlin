using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Documents.Client;
using CuriousGremlin.GraphSON;
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

        public void Dispose()
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

        public async Task<FeedResponse<object>> Execute(ITraversalQuery query)
        {
            return await Execute(query.ToString());
        }

        public async Task<List<Vertex>> Execute(VertexQuery query)
        {
            var vertices = new List<Vertex>();
            var results = await Execute(query.ToString());
            foreach(Newtonsoft.Json.Linq.JObject result in results)
            {
                vertices.Add(result.ToObject<Vertex>());
            }
            return vertices;
        }

        public async Task<List<Edge>> Execute(EdgeQuery<Graph> query)
        {
            var edges = new List<Edge>();
            var results = await Execute(query.ToString());
            foreach (Newtonsoft.Json.Linq.JObject result in results)
            {
                edges.Add(result.ToObject<Edge>());
            }
            return edges;
        }

        public async Task<List<T>> Execute<T>(ValueQuery<T, Graph> query)
        {
            var items = new List<T>();
            var results = await Execute(query.ToString());
            foreach (Newtonsoft.Json.Linq.JObject result in results)
            {
                items.Add(result.ToObject<T>());
            }
            return items;
        }

        public async Task<List<List<T>>> Execute<T>(ListQuery<T, Graph> query)
        {
            var items = new List<List<T>>();
            var results = await Execute(query.ToString());
            foreach (Newtonsoft.Json.Linq.JObject result in results)
            {
                items.Add(result.ToObject<List<T>>());
            }
            return items;
        }

        public async Task<List<List<KeyValuePair<TKey,TValue>>>> Execute<TKey,TValue>(DictionaryQuery<TKey, TValue, Graph> query)
        {
            var items = new List<List<KeyValuePair<TKey, TValue>>>();
            var results = await Execute(query.ToString());
            foreach (Newtonsoft.Json.Linq.JObject result in results)
            {
                items.Add(result.ToObject<List<KeyValuePair<TKey,TValue>>>());
            }
            return items;
        }

        public async Task<bool> Execute(BooleanQuery<Graph> query)
        {
            var items = new List<bool>();
            var results = await Execute(query.ToString());
            foreach (Newtonsoft.Json.Linq.JObject result in results)
            {
                items.Add(result.ToObject<bool>());
            }
            return items[0];
        }

        public async Task Execute(TerminalQuery<Graph> query)
        {
            var results = await Execute(query.ToString());
        }
        #endregion
    }
}
