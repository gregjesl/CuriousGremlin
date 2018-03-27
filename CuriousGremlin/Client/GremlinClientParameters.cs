using System;
namespace CuriousGremlin.Client
{
    public class GremlinClientParameters
    {
        public string Endpoint { set; get; }
        public int Port { set; get; }
        public bool EnableSSL { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }

        public GremlinClientParameters()
        {
        }

        public static GremlinClientParameters AzureCosmosDBClient(string endpoint, string database, string collection, string authKey)
        {
            return AzureCosmosDBClient(endpoint, 443, database, collection, authKey);
        }

        public static GremlinClientParameters AzureCosmosDBClient(string endpoint, int port, string database, string collection, string authKey)
        {
            return new GremlinClientParameters()
            {
                Endpoint = endpoint,
                Port = port,
                EnableSSL = true,
                Username = "/dbs/" + database + "/colls/" + collection,
                Password = authKey
            };
        }
    }
}
