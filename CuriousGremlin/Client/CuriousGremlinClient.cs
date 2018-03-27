using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Gremlin.Net;

namespace CuriousGremlin.Client
{
    public class CuriousGremlinClient : GraphClient, IDisposable
    {
        protected GremlinClient client;
        internal CuriousGremlinClientPool pool;

#region Constructors
        protected CuriousGremlinClient() { }

        public CuriousGremlinClient(string hostname)
        {
            var gremlinServer = new GremlinServer(hostname);
            client = new GremlinClient(gremlinServer);
        }

        public CuriousGremlinClient(string hostname, int port)
        {
            var gremlinServer = new GremlinServer(hostname, port);
            client = new GremlinClient(gremlinServer);
        }

        public CuriousGremlinClient(string hostname, int port, bool enableSSL)
        {
            var gremlinServer = new GremlinServer(hostname, port, enableSSL);
            client = new GremlinClient(gremlinServer);
        }

        public CuriousGremlinClient(string hostname, int port, bool enableSSL, string username)
        {
            var gremlinServer = new GremlinServer(hostname, port, enableSSL, username);
            client = new GremlinClient(gremlinServer);
        }

        public CuriousGremlinClient(string hostname, int port, bool enableSSL, string username, string password)
        {
            var gremlinServer = new GremlinServer(hostname, port, enableSSL, username, password);
            client = new GremlinClient(gremlinServer);
        }

        public CuriousGremlinClient(GremlinClientParameters parameters)
        {
            GremlinServer gremlinServer;
            if (string.IsNullOrEmpty(parameters.Endpoint))
                throw new ArgumentNullException(nameof(parameters.Endpoint));
            if (string.IsNullOrEmpty(parameters.Password))
            {
                if (string.IsNullOrEmpty(parameters.Username))
                    gremlinServer = new GremlinServer(parameters.Endpoint, parameters.Port, parameters.EnableSSL);
                else
                    gremlinServer = new GremlinServer(parameters.Endpoint, parameters.Port, parameters.EnableSSL, parameters.Username);
            }
            else
            {
                gremlinServer = new GremlinServer(parameters.Endpoint, parameters.Port, parameters.EnableSSL, parameters.Username, parameters.Password);
            }
            client = new GremlinClient(gremlinServer);
        }
#endregion

        public override async Task<IEnumerable<object>> Execute(string query)
        {
            return await client.SubmitAsync<object>(query);
        }

        public virtual void Dispose()
        {
            if(pool != null)
            {
                
            }
            client.Dispose();
        }
    }
}
