using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin;
using CuriousGremlin.Client;
using CuriousGremlin.AzureCosmosDB;
using CuriousGremlin.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuriousGremlin.AzureCosmosDB.UnitTests
{
    [TestClass]
    public class GraphClientPoolTests
    {
        [TestMethod]
        public void AzureCosmosDB_ExecuteFromPool()
        {
            Func<IGraphClient> clientFactory = new Func<IGraphClient>(() =>
            {
                return TestDatabase.GetClient("graph_client_pool");
            });

            var pool = new GraphClientPool(clientFactory);

            var query = VertexQuery.All();
            var result = pool.Execute(query).Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
            Assert.AreEqual(pool.PoolSize, 1);

            pool.Dispose();
            Assert.AreEqual(pool.PoolSize, 0);
        }
    }
}
