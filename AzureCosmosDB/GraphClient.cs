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

namespace CuriousGremlin.AzureCosmosDB
{
    public class GraphClient : IDisposable
    {
        public bool IsOpen { get; private set; } = false;
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private DocumentClient client;
        private DocumentCollection graph;

        public GraphClient(string endpoint, string authKey)
        {
            client = new DocumentClient(new Uri(endpoint), authKey);
        }

        ~GraphClient()
        {
            if(IsOpen)
                Dispose();
        }

        public async Task Open(string database, string graph)
        {
            this.graph = await client.ReadDocumentCollectionAsync("/dbs/" + database + "/colls/" + graph);
        }

        public void Dispose()
        {
            client.Dispose();
            IsOpen = false;
        }

        public async Task<FeedResponse<object>> Execute(string queryString)
        {
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

        public async Task<List<Vertex>> Execute(VertexQuery query)
        {
            var vertices = new List<Vertex>();
            var results = await Execute(query.Query);
            foreach(Newtonsoft.Json.Linq.JObject result in results)
            {
                vertices.Add(result.ToObject<Vertex>());
            }
            return vertices;
        }

        public async Task<List<Edge>> Execute(EdgeQuery query)
        {
            var edges = new List<Edge>();
            var results = await Execute(query.Query);
            foreach (Newtonsoft.Json.Linq.JObject result in results)
            {
                edges.Add(result.ToObject<Edge>());
            }
            return edges;
        }
    }
}
