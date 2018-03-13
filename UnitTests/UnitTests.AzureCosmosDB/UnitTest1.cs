using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuriousGremlin.AzureCosmosDB;
using CuriousGremlin.Query;
using System;

namespace UnitTests.AzureCosmosDB
{
    [TestClass]
    public class ElementManipulation
    {
        [TestMethod]
        public void AzureCosmosDB_Insert_and_Delete()
        {
            GraphClientPool pool = new GraphClientPool(
                "https://localhost:8081/",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                "test_db",
                "test_collection"
                );
            try
            { 
                using (GraphClient client = pool.GetClient().Result)
                {
                    
                    // Kill it with fire
                    client.Execute(GraphQuery.Vertices().Drop().ToString()).Wait();

                    
                    // Assert that the database is clean
                    Assert.AreEqual(client.Execute(GraphQuery.Vertices().ToString()).Result.Count, 0);

                    // Insert an object
                    VertexQuery insert_query = GraphQuery.AddVertex("test_vertex").AddProperty("test_key", "test_value").AddProperty("test_key", "another_test_value");
                    var insert_result = client.Execute(insert_query).Result;
                    Assert.AreEqual(insert_result.Count, 1);
                    Assert.AreEqual(insert_result[0].label, "test_vertex");
                    Assert.IsTrue(insert_result[0].properties.ContainsKey("test_key"));
                    var properties = insert_result[0].properties["test_key"];
                    Assert.IsNotNull(properties.Find(k => k.Value.Equals("test_value")));
                    Assert.IsNotNull(properties.Find(k => k.Value.Equals("another_test_value")));

                    
                    // Insert a second object
                    var second_vertex_query = GraphQuery.AddVertex("second_vertex");
                    var second_vertex_result = client.Execute(second_vertex_query).Result;
                    Assert.AreEqual(second_vertex_result.Count, 1);

                    // Connect the two objects
                    var connect_query = GraphQuery.Vertex(insert_result[0].id).AddEdge("test_edge", second_vertex_result[0].id).AddProperty("test_key", "test_value");
                    var connect_result = client.Execute(connect_query).Result;
                    Assert.AreEqual(connect_result.Count, 1);
                    Assert.AreEqual(connect_result[0].label, "test_edge");
                    Assert.AreEqual(connect_result[0].inVLabel, "second_vertex");
                    Assert.AreEqual(connect_result[0].outVLabel, "test_vertex");
                    Assert.AreEqual(connect_result[0].inV, second_vertex_result[0].id);
                    Assert.AreEqual(connect_result[0].outV, insert_result[0].id);
                    Assert.IsTrue(connect_result[0].properties.ContainsKey("test_key"));
                    Assert.IsTrue(connect_result[0].properties.ContainsValue("test_value"));

                    // Reload the first object to check for edges
                    VertexQuery out_query = GraphQuery.Vertex(insert_result[0].id).Out();
                    var out_result = client.Execute(out_query).Result;
                    Assert.AreEqual(out_result.Count, 1);
                    Assert.AreEqual(out_result[0].id, second_vertex_result[0].id);

                    // Check the edge query
                    var count_result = client.Execute("g.V()").Result;
                }
            }
            finally
            {
                Assert.IsTrue(pool.PoolSize > 0);
                pool.Dispose();
            }
        }
    }
}
