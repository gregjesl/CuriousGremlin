﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;

namespace CuriousGremlin.Client
{
    public class GraphClientPool : IGraphClient, IDisposable
    {
        public static GraphClientPool Pool = null;

        public int NumRetries = 2;

        protected bool PoolIsOpen = false;
        protected SemaphoreSlim PoolSemaphore;
        protected List<IGraphClient> ClientPool;
        protected Func<IGraphClient> ClientFactory = null;

        public static void CreatePool(Func<IGraphClient> clientFactory)
        {
            Pool = new GraphClientPool(clientFactory);
        }

        public static void DisposePool()
        {
            Pool.Dispose();
            Pool = null;
        }

        public static async Task<IEnumerable<T>> ExecuteOnPool<T>(TraversalQuery<GraphQuery, T> query)
        {
            if (Pool is null)
                throw new InvalidOperationException("Client pool has not been created");
            return await Pool.Execute(query);
        }

        public static async Task ExecuteOnPool(TerminalQuery<GraphQuery> query)
        {
            if (Pool is null)
                throw new InvalidOperationException("Client pool has not been created");
            await Pool.Execute(query);
        }

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

        public async Task<IEnumerable> Execute(string query)
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

            IEnumerable result = null;

            for(int i = 0; i < NumRetries; i++)
            {
                try
                {
                    result = await client.Execute(query);
                    break;
                }
                catch (SocketException)
                {
                    Trace.Write("Socket exception handled");
                }
            }

            if (result is null)
                throw new Exception("Could not execute query");

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
