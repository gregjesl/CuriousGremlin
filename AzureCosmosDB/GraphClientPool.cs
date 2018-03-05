using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CuriousGremlin.AzureCosmosDB
{
    public class GraphClientPool
    {
        private string endpoint;
        private string authKey;
        private string database;
        private string graph;

        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private List<GraphClient> pool = new List<GraphClient>();
        
        public GraphClientPool(string endpoint, string authKey, string database, string graph)
        {
            this.endpoint = endpoint;
            this.authKey = authKey;
            this.database = database;
            this.graph = graph;
        }
    }
}
