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
        public static GraphClientPool Pool;

        protected bool PoolIsOpen = false;
        protected SemaphoreSlim PoolSemaphore;
        protected List<IGraphClient> ClientPool;
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
                    if (ClientPool is null)
                        return 0;
                    return ClientPool.Count;
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
            ClientPool = new List<IGraphClient>();
            ClientFactory = clientFactory;
            ClientPool.Add(clientFactory.Invoke());
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
                if (ClientPool.Count > 0)
                {
                    client = ClientPool[0];
                    ClientPool.Remove(client);
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
                ClientPool.Add(client);
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
                foreach (var client in ClientPool)
                {
                    if (client.GetType().GetInterfaces().Contains(typeof(IDisposable)))
                        (client as IDisposable).Dispose();
                }
                ClientPool.Clear();
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
