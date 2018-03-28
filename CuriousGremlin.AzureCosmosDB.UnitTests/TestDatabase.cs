using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.AzureCosmosDB;
using CuriousGremlin;
using CuriousGremlin.Client;

namespace UnitTests.AzureCosmosDB
{
    class TestDatabase : CosmosDBGraphClient, IDisposable
    {
        public static readonly string db_name = "test_db";

        public TestDatabase(string endpoint, string authKey) : base(endpoint, authKey) { }

        private string collection;

        public static TestDatabase GetClient(string collection)
        {
            TestDatabase client = new TestDatabase("https://localhost:8081/",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            client.CreateDatabaseIfNotExistsAsync(db_name).Wait();
            client.collection = collection;
            client.CreateDocumentCollectionIfNotExistsAsync(db_name, collection).Wait();
            client.Open(db_name, collection).Wait();
            var query = VertexQuery.All().Drop();
            client.Execute(query).Wait();
            return client;
        }

        public override void Dispose()
        {
            var query = VertexQuery.All().Drop();
            this.Execute(query).Wait();
            this.DeleteCollectionAsync(db_name, collection).Wait();
            base.Dispose();
        }
    }
}
