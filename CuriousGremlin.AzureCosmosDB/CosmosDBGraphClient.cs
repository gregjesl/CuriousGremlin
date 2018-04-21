using System;
using System.Collections;
using System.Text;
using Microsoft.Azure.Documents;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Documents.Client;
using CuriousGremlin;
using Newtonsoft.Json;
using CuriousGremlin.Objects;
using CuriousGremlin.Client;

namespace CuriousGremlin.AzureCosmosDB
{
    public class CosmosDBGraphClient : IGraphClient, IDisposable
    {
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public bool IsOpen { get; protected set; } = false;

        private DocumentClient client;
        private DocumentCollection graph;

        protected CosmosDBGraphClient() { }

        public CosmosDBGraphClient(string endpoint, string authKey)
        {
            client = new DocumentClient(new Uri(endpoint), authKey);
        }

        ~CosmosDBGraphClient()
        {
            if (IsOpen)
                Dispose();
        }

        public async Task Open(string database, string graph)
        {
            if (!IsOpen)
            {
                this.graph = await client.ReadDocumentCollectionAsync("/dbs/" + database + "/colls/" + graph);
                this.IsOpen = true;
            }
        }

        public virtual void Dispose()
        {
            IsOpen = false;
            client.Dispose();
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

        public async Task CreateDocumentCollectionAsync(string database, string collection, string PartitionKey)
        {
            await client.CreateDocumentCollectionAsync("/dbs/" + database, new DocumentCollection
            {
                Id = collection,
                PartitionKey = new PartitionKeyDefinition
                {
                    Paths = new System.Collections.ObjectModel.Collection<string> { PartitionKey }
                }
            });
        }

        public async Task CreateDocumentCollectionIfNotExistsAsync(string database, string collection)
        {
            await client.CreateDocumentCollectionIfNotExistsAsync("/dbs/" + database, new DocumentCollection { Id = collection });
        }

        public async Task CreateDocumentCollectionIfNotExistsAsync(string database, string collection, string PartitionKey)
        {
            await client.CreateDocumentCollectionIfNotExistsAsync("/dbs/" + database, new DocumentCollection
            {
                Id = collection,
                PartitionKey = new PartitionKeyDefinition
                {
                    Paths = new System.Collections.ObjectModel.Collection<string> { PartitionKey }
                }
            });
        }

        public async Task DeleteCollectionAsync(string database, string collection)
        {
            await client.DeleteDocumentCollectionAsync("/dbs/" + database + "/colls/" + collection);
        }
        #endregion

        #region Queries
        public async Task<IEnumerable> Execute(string queryString)
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
        #endregion
    }
}
