using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CuriousGremlin.AzureCosmosDB
{
    public class GraphClientPool : IDisposable
    {
        public int PoolSize
        {
            get
            {
                lock(pool)
                {
                    return pool.Count;
                }
            }
        }
        public int WorkerSize
        {
            get
            {
                lock (workers)
                {
                    return workers.Count;
                }
            }
        }

        internal string Endpoint { get; }
        internal string AuthKey { get; }
        internal string Database { get; }
        internal string Graph { get; }

        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private List<GraphClient> pool = new List<GraphClient>();
        private List<GraphClient> workers = new List<GraphClient>();
        
        public GraphClientPool(string endpoint, string authKey, string database, string graph)
        {
            this.Endpoint = endpoint;
            this.AuthKey = authKey;
            this.Database = database;
            this.Graph = graph;
        }

        public async Task<GraphClient> GetClient()
        {
            GraphClient client = null;

            // Attempt to pull a client from the pool
            lock(pool)
            {
                if(pool.Count > 0)
                {
                    client = pool[0];
                    pool.Remove(client);
                }
            }

            // Create a new client if none were available
            if(client is null)
            {
                client = new GraphClient(this);
            }

            // Open the client if it is not open
            if(!client.IsOpen)
                await client.Open(this.Database, this.Graph);

            lock(workers)
            {
                workers.Add(client);
            }

            return client;
        }

        internal void ReturnToPool(GraphClient client)
        {
            lock(workers)
            lock(pool)
            {
                workers.Remove(client);
                pool.Add(client);
            }
        }

        public void Dispose()
        {
            lock (workers)
            {
                foreach(var client in workers)
                {
                    lock(client.pool)
                    {
                        client.pool = null;
                    }
                }
                workers.Clear();
            }
            lock (pool)
            {
                foreach(var client in pool)
                {
                    client.pool = null;
                    client.Dispose();
                }
                pool.Clear();
            }
        }
    }
}
