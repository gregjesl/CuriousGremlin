using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuriousGremlin.AzureCosmosDB;
using CuriousGremlin.Query;
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
            GraphClient client = new GraphClient("https://localhost:8081/",
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
            GraphClient client = new GraphClient("https://localhost:8081/",
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
    }
}
