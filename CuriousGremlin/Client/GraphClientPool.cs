using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Client
{
    public class GraphClientPool : IGraphClient, IDisposable
    {
        protected bool PoolIsOpen = false;
        protected SemaphoreSlim PoolSemaphore;
        protected List<IGraphClient> Pool;
        protected Func<IGraphClient> ClientFactory = null;

        public int PoolSize
        {
            get
            {
                if (!PoolIsOpen)
                    return 0;
                PoolSemaphore.Wait();
                try
                {
                    if (Pool is null)
                        return 0;
                    return Pool.Count;
                }
                finally
                {
                    PoolSemaphore.Release();
                }
            }
        }

        protected GraphClientPool() { }

        public GraphClientPool(Func<IGraphClient> clientFactory)
        {
            Pool = new List<IGraphClient>();
            ClientFactory = clientFactory;
            Pool.Add(clientFactory.Invoke());
            PoolSemaphore = new SemaphoreSlim(1, 1);
            PoolIsOpen = true;
        }

        ~GraphClientPool()
        {
            if (PoolIsOpen)
                Dispose();
        }

        public async Task<IEnumerable<object>> Execute(string query)
        {
            if (!PoolIsOpen)
                throw new InvalidOperationException("Pool has not been opened");

            IGraphClient client = null;
            // Attempt to get a client from the pool
            await PoolSemaphore.WaitAsync();
            try
            {
                if (Pool.Count > 0)
                {
                    client = Pool[0];
                    Pool.Remove(client);
                }
            }
            finally
            {
                PoolSemaphore.Release();
            }

            // Create a new client if one was not available
            if (client is null)
            {
                if (ClientFactory is null)
                    throw new Exception("Client factory has not been established");
                client = ClientFactory.Invoke();
                if (client == null)
                    throw new Exception("Unable to create a client using the client factory");
            }

            var result = await client.Execute(query);

            // If the pool is closed, then exit
            if (!PoolIsOpen)
                return result;

            // Return the client to the pool
            await PoolSemaphore.WaitAsync();
            try
            {
                Pool.Add(client);
            }
            finally
            {
                PoolSemaphore.Release();
            }

            return result;
        }

        public virtual void Dispose()
        {
            PoolSemaphore.Wait();
            try
            {
                foreach (var client in Pool)
                {
                    if (client.GetType().GetInterfaces().Contains(typeof(IDisposable)))
                        (client as IDisposable).Dispose();
                }
                Pool.Clear();
                PoolIsOpen = false;
            }
            finally
            {
                PoolSemaphore.Release();
            }
            PoolSemaphore.Dispose();
            PoolSemaphore = null;
        }
    }
}
