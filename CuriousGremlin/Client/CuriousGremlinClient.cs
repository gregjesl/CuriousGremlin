using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Gremlin.Net;

namespace CuriousGremlin.Client
{
    public class CuriousGremlinClient : GraphClient, IDisposable
    {
        protected GremlinClient client;

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

        public CuriousGremlinClient(string endpoint, int port, string database, string collection, string authKey)
        {
            var gremlinServer = new GremlinServer(endpoint, port, enableSsl: true,
                                                username: "/dbs/" + database + "/colls/" + collection,
                                                password: authKey);
            client = new GremlinClient(gremlinServer);
        }

        public static CuriousGremlinClient AzureCosmosDBClient(string endpoint, string database, string collection, string authKey)
        {
            return AzureCosmosDBClient(endpoint, 443, database, collection, authKey);
        }

        public static CuriousGremlinClient AzureCosmosDBClient(string endpoint, int port, string database, string collection, string authKey)
        {
            return new CuriousGremlinClient(
                endpoint,
                port,
                true,
                "/dbs/" + database + "/colls/" + collection,
                authKey
                );
        }

        public override async Task<IEnumerable<object>> Execute(string query)
        {
            return await client.SubmitAsync<object>(query);
        }

        public virtual void Dispose()
        {
            client.Dispose();
        }
    }
}
