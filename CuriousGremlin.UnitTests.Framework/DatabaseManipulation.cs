using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuriousGremlin.AzureCosmosDB;
using CuriousGremlin;
using System;

namespace UnitTests.AzureCosmosDB
{
    [TestClass]
    public class DatabaseManipulation
    {
        [TestMethod]
        public void AzureCosmosDB_CreateAndDeleteDatabase()
        {
            const string db_name = "create_and_delete";
            CosmosDBGraphClient client = new CosmosDBGraphClient("https://localhost:8081/",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            using (client)
            {
                try
                {
                    client.DeleteDatabaseAsync(db_name).Wait();
                } catch(Exception)
                {

                }

                client.CreateDatabaseAsync(db_name).Wait();
                client.DeleteDatabaseAsync(db_name).Wait();
            }
        }

        [TestMethod]
        public void AzureCosmosDB_CreateAndDeleteIfNotExistsDatabase()
        {
            const string db_name = "create_and_delete_if_not_exists";
            CosmosDBGraphClient client = new CosmosDBGraphClient("https://localhost:8081/",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            using (client)
            {
                try
                {
                    client.DeleteDatabaseAsync(db_name).Wait();
                }
                catch (Exception)
                {

                }

                client.CreateDatabaseIfNotExistsAsync(db_name).Wait();
                client.CreateDatabaseIfNotExistsAsync(db_name).Wait();
                client.DeleteDatabaseAsync(db_name).Wait();
            }
        }

        [TestMethod]
        public void AzureCosmosDB_CreateAndDeleteIfNotExistsCollection()
        {
            const string name = "create_and_delete_collection";
            string db_name = name + "_db";
            CosmosDBGraphClient client = new CosmosDBGraphClient("https://localhost:8081/",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            using (client)
            {
                try
                {
                    client.DeleteDatabaseAsync(db_name).Wait();
                }
                catch (Exception)
                {

                }

                client.CreateDatabaseAsync(db_name).Wait();
                client.CreateDocumentCollectionAsync(db_name, name).Wait();
                client.DeleteCollectionAsync(db_name, name).Wait();
                client.DeleteDatabaseAsync(db_name).Wait();
            }
        }

        [TestMethod]
        public void AzureCosmosDB_CreateAndDeleteCollection()
        {
            const string name = "create_and_delete_if_not_exists_collection";
            string db_name = name + "_db";
            CosmosDBGraphClient client = new CosmosDBGraphClient("https://localhost:8081/",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            using (client)
            {
                try
                {
                    client.DeleteDatabaseAsync(db_name).Wait();
                }
                catch (Exception)
                {

                }

                client.CreateDatabaseAsync(db_name).Wait();
                client.CreateDocumentCollectionIfNotExistsAsync(db_name, name).Wait();
                client.CreateDocumentCollectionIfNotExistsAsync(db_name, name).Wait();
                client.DeleteDatabaseAsync(db_name).Wait();
            }
        }
    }
}
