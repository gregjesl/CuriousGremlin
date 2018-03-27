using System;
using System.Threading;
using System.Collections.Generic;

namespace CuriousGremlin.Client
{
    public class CuriousGremlinClientPool : IDisposable
    {
        bool IsDisposed = false;
        protected GremlinClientParameters Parameters;
        private List<CuriousGremlinClient> Pool = new List<CuriousGremlinClient>();
        private List<CuriousGremlinClient> Workers = new List<CuriousGremlinClient>();

        protected CuriousGremlinClientPool() { }

        public CuriousGremlinClientPool(GremlinClientParameters parameters)
        {
            this.Parameters = parameters;
        }

        public virtual CuriousGremlinClient GetClient()
        {
            CuriousGremlinClient client = null;
            // Attempt to pull a client from the pool
            lock(Pool)
            {
                if(Pool.Count > 0)
                {
                    client = Pool[0];
                    Pool.Remove(client);
                }
            }
            // Create a new client if there was not one available in the pool
            if(client is null)
            {
                client = new CuriousGremlinClient(Parameters)
                {
                    pool = this
                };
            }
            // Add the worker to the worker pool
            lock (Workers)
            {
                Workers.Add(client);
            }
            return client;
        }

        internal void ReturnToPool(CuriousGremlinClient client)
        {
            if (IsDisposed)
                return;
            lock (Workers)
            {
                Workers.Remove(client);
            }
            lock (Pool)
            {
                Pool.Add(client);
            }
                
        }

        public void Dispose()
        {
            // Remove all works from the pool
            lock (Workers)
            {
                foreach (var client in Workers)
                {
                    lock (client.pool)
                    {
                        /* By setting the pool to null, when the client is 
                         * disposed it will actually dispose the client instead 
                         * of sending it back to the pool */
                        client.pool = null;
                    }
                }
                // Empty the list of workers
                Workers.Clear();
            }
            lock (Pool)
            {
                // Dispose of all of the clients in the pool
                foreach (var client in Pool)
                {
                    client.pool = null;
                    client.Dispose();
                }
                Pool.Clear();
            }
            IsDisposed = true;
        }
    }
}
